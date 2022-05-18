using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using Debug = UnityEngine.Debug;

namespace UnityVolumeRendering
{
    public enum DataContentFormat
    {
        Int8,
        Uint8,
        Int16,
        Uint16,
        Int32,
        Uint32
    }

    public enum Endianness
    {
        LittleEndian,
        BigEndian
    }
    
    
    

    public class RawDatasetImporter : DatasetImporterBase
    {
        string filePath;
        private int dimX;
        private int dimY;
        private int dimZ;
        private DataContentFormat contentFormat;
        private Endianness endianness;
        private int skipBytes;
        
        /// <summary>
        /// Number of import steps taken in one coroutine timeframe
        /// </summary>
        private int CoRoutineIterationLength = 0;
        

        public RawDatasetImporter(string filePath, int dimX, int dimY, int dimZ, DataContentFormat contentFormat, Endianness endianness, int skipBytes)
        {
            this.filePath = filePath;
            this.dimX = dimX;
            this.dimY = dimY;
            this.dimZ = dimZ;
            this.contentFormat = contentFormat;
            this.endianness = endianness;
            this.skipBytes = skipBytes;
        }
        
        
        public override VolumeDataset Import()
        {
            // Check that the file exists
            if(!File.Exists(filePath))
            {
                Debug.LogError("The file does not exist: " + filePath);
                return null;
            }

            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);

            // Check that the dimension does not exceed the file size
            long expectedFileSize = (long)(dimX * dimY * dimZ) * GetSampleFormatSize(contentFormat) + skipBytes;
            if (fs.Length < expectedFileSize)
            {
                Debug.LogError($"The dimension({dimX}, {dimY}, {dimZ}) exceeds the file size. Expected file " +
                               "size is {expectedFileSize} bytes, while the actual file size is {fs.Length} bytes");
                reader.Close();
                fs.Close();
                return null;
            }

            VolumeDataset dataset = new VolumeDataset();
            dataset.datasetName = Path.GetFileName(filePath);
            dataset.dimX = dimX;
            dataset.dimY = dimY;
            dataset.dimZ = dimZ;

            // Skip header (if any)
            if (skipBytes > 0)
                reader.ReadBytes(skipBytes);

            int uDimension = dimX * dimY * dimZ;
            dataset.data = new int[uDimension];

            // Read the data/sample values
            for (int i = 0; i < uDimension; i++)
            {
                dataset.data[i] = ReadDataValue(reader);
            }
          
            Debug.Log("Loaded dataset in range: " + dataset.GetMinDataValue() + "  -  " + dataset.GetMaxDataValue());

            reader.Close();
            fs.Close();
            return dataset;
        }

        public IEnumerator ImportRoutine(Action<VolumeDataset> ds)
        {
            Stopwatch sw = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            
            sw.Start();
            
            // Check that the file exists
            if(!File.Exists(filePath))
            {
                Debug.LogError("The file does not exist: " + filePath);
                //return null;
            }
            else
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        sw.Stop();
                        sb.AppendLine("IO_Time: " + sw.ElapsedMilliseconds + " ms");
                        sw.Restart();
                        
                        
                        
                        // Check that the dimension does not exceed the file size
                        long expectedFileSize = (long)(dimX * dimY * dimZ) * GetSampleFormatSize(contentFormat) + skipBytes;
                        if (fs.Length < expectedFileSize)
                        {
                            Debug.LogError($"The dimension({dimX}, {dimY}, {dimZ}) exceeds the file size. Expected file " +
                                           "size is {expectedFileSize} bytes, while the actual file size is {fs.Length} bytes");
                            reader.Close();
                            fs.Close();
                            //return null;
                        }
                        else
                        {
                            var dataset = new VolumeDataset();
                            dataset.datasetName = Path.GetFileName(filePath);
                            dataset.dimX = dimX;
                            dataset.dimY = dimY;
                            dataset.dimZ = dimZ;
                            
                            Debug.Log("test1");

                            // Skip header (if any)
                            if (skipBytes > 0)
                                reader.ReadBytes(skipBytes);

                            
                            
                            int uDimension = dimX * dimY * dimZ;
                            dataset.data = new int[uDimension];

                            //Debug.Log("uDimension: " + uDimension);
                            
                            sw.Stop();
                            sb.AppendLine("Precalc_Time: " + sw.ElapsedMilliseconds + " ms");
                            sw.Restart();

                            CoRoutineIterationLength = (int)Math.Floor(uDimension * 0.01f);
                            
                            // Read the data/sample values
                            for (int i = 0; i < uDimension; i++)
                            {
                                dataset.data[i] = ReadDataValue(reader);
                                //Debug.Log("Iteration " + i);
                                if (i % CoRoutineIterationLength == 0)
                                {
                                    //Debug.Log("Iteration: " + i + "/" + uDimension);
                                    yield return null;
                                }
                            }

                            sw.Stop();
                            sb.AppendLine("DataRead_Time: " + sw.ElapsedMilliseconds + " ms");
                            sw.Restart();
                            
                            Debug.Log("Loaded dataset in range: " + dataset.GetMinDataValue() + "  -  " + dataset.GetMaxDataValue());

                            //reader.Close();
                            //fs.Close();
                            //return dataset;
                            ds(dataset);

                            sw.Stop();
                            sb.AppendLine("Cleanup_Time: " + sw.ElapsedMilliseconds + " ms");
                            
                            //Debug.Log(sb);
                            
                            yield return null;   
                        }
                    }    
                }
                

                
            }
            
            
        }
        
        
        private int ReadDataValue(BinaryReader reader)
        {
            switch (contentFormat)
            {
                case DataContentFormat.Int8:
                    {
                        sbyte dataval = reader.ReadSByte();
                        return (int)dataval;
                    }
                case DataContentFormat.Int16:
                    {
                        short dataval = reader.ReadInt16();
                        if (endianness == Endianness.BigEndian)
                        {
                            byte[] bytes = BitConverter.GetBytes(dataval);
                            Array.Reverse(bytes, 0, bytes.Length);
                            dataval = BitConverter.ToInt16(bytes, 0);
                        }
                        return (int)dataval;
                    }
                case DataContentFormat.Int32:
                    {
                        int dataval = reader.ReadInt32();
                        if (endianness == Endianness.BigEndian)
                        {
                            byte[] bytes = BitConverter.GetBytes(dataval);
                            Array.Reverse(bytes, 0, bytes.Length);
                            dataval = BitConverter.ToInt32(bytes, 0);
                        }
                        return (int)dataval;
                    }
                case DataContentFormat.Uint8:
                    {
                        return (int)reader.ReadByte();
                    }
                case DataContentFormat.Uint16:
                    {
                        ushort dataval = reader.ReadUInt16();
                        if (endianness == Endianness.BigEndian)
                        {
                            byte[] bytes = BitConverter.GetBytes(dataval);
                            Array.Reverse(bytes, 0, bytes.Length);
                            dataval = BitConverter.ToUInt16(bytes, 0);
                        }
                        return (int)dataval;
                    }
                case DataContentFormat.Uint32:
                    {
                        uint dataval = reader.ReadUInt32();
                        if (endianness == Endianness.BigEndian)
                        {
                            byte[] bytes = BitConverter.GetBytes(dataval);
                            Array.Reverse(bytes, 0, bytes.Length);
                            dataval = BitConverter.ToUInt32(bytes, 0);
                        }
                        return (int)dataval;
                    }
                default:
                    throw new NotImplementedException("Unimplemented data content format");
            }
        }

        private int GetSampleFormatSize(DataContentFormat format)
        {
            switch (format)
            {
                case DataContentFormat.Int8:
                    return 1;
                    break;
                case DataContentFormat.Uint8:
                    return 1;
                    break;
                case DataContentFormat.Int16:
                    return 2;
                    break;
                case DataContentFormat.Uint16:
                    return 2;
                    break;
                case DataContentFormat.Int32:
                    return 4;
                    break;
                case DataContentFormat.Uint32:
                    return 4;
                    break;
            }
            throw new NotImplementedException();
        }
    }
}