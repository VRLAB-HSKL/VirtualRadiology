using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class VolumeDatasetTextureCalc : MonoBehaviour
    {
        private static int minDataValue = int.MaxValue;
        private static int maxDataValue = int.MinValue;

        private static int[] _data;
        private static int _dimX;
        private static int _dimY;
        private static int _dimZ;
        
        public static IEnumerator CreateTextureInternalRoutine(int dimX, int dimY, int dimZ, int[] data, Action<Texture3D> texCallable)
        {
            _data = data;
            _dimX = dimX;
            _dimY = dimY;
            _dimZ = dimZ;
            
            TextureFormat texformat = SystemInfo.SupportsTextureFormat(TextureFormat.RHalf) ? TextureFormat.RHalf : TextureFormat.RFloat;
            Texture3D texture = new Texture3D(dimX, dimY, dimZ, texformat, false);
            texture.wrapMode = TextureWrapMode.Clamp;

            int minValue = GetMinDataValue();
            int maxValue = GetMaxDataValue();
            int maxRange = maxValue - minValue;


            yield return null;

            int numSteps = dimX * dimY * dimZ;
            var batchSizeFactor = 0.001f;
            int numberofRowsInRoutineStep = (int) Mathf.Floor(numSteps * batchSizeFactor);
            
            Debug.Log("numstep: " + numSteps + 
                      ", back to game loop each " + numberofRowsInRoutineStep + " iterations");
            
            if (dimZ > 500)
            {
                for (int x = 0; x < dimX; x++)
                {
                    for (int y = 0; y < dimY; y++)
                    {
                        // for (int z = 0; z < (dimZ / 2) - 1; z++)
                        // {
                        //     int iData = x + y * dimX + z * (dimX * dimY);
                        //     texture.SetPixel(x, y, z, new Color((float)(data[iData] - minValue) / maxRange, 0.0f, 0.0f, 0.0f));
                        //     
                        //     
                        // }
                        //
                        // for (int z = dimZ / 2; z < dimZ; z++)
                        // {
                        //     int iData = x + y * dimX + z * (dimX * dimY);
                        //     texture.SetPixel(x, y, z, new Color((float)(data[iData] - minValue) / maxRange, 0.0f, 0.0f, 0.0f));
                        // }
                        
                        for (int z = 0; z < dimZ; z++)
                        {
                            int iData = x + y * dimX + z * (dimX * dimY);
                            texture.SetPixel(x, y, z, new Color((float)(data[iData] - minValue) / maxRange, 0.0f, 0.0f, 0.0f));

                            int currentStep = iData; //(x * dimX + y * dimY + dimZ * z);
                            if (currentStep % numberofRowsInRoutineStep == 0)
                            {
                                // Debug.Log("[" + currentStep + "/" + numSteps + "]:" + 
                                //           " " + currentStep + " % " + numberofRowsInRoutineStep +
                                //           " x: " + x + "/" + dimX + 
                                //           " y: " + y + "/" + dimY + 
                                //           " z: " + z + "/" + dimZ);
                                yield return null;    
                            }
                            
                        }
                        
                        
                        
                        
                    }

                    
                    
                }

                texture.Apply();
                texCallable(texture);
            }
            else
            {
                Color[] cols = new Color[data.Length];

                for (int x = 0; x < dimX; x++)
                {
                    for (int y = 0; y < dimY; y++)
                    {
                        for (int z = 0; z < dimZ; z++)
                        {
                            int iData = x + y * dimX + z * (dimX * dimY);
                            // cols[iData] = new Color((float)(data[iData] - minValue) / maxRange, 0.0f, 0.0f, 0.0f);
                            texture.SetPixel(x, y, z, new Color((float)(data[iData] - minValue) / maxRange, 0.0f, 0.0f, 0.0f));

                            var currentStep = iData; //(x * dimX + y * dimY + dimZ * z);
                            if (currentStep % numberofRowsInRoutineStep == 0)
                            {
                                // Debug.Log("[" + currentStep + "/" + numSteps + "]:" + 
                                //           " " + currentStep + " % " + numberofRowsInRoutineStep +
                                //           " x: " + x + "/" + dimX + 
                                //           " y: " + y + "/" + dimY + 
                                //           " z: " + z + "/" + dimZ);
                                yield return null;
                            }
                        }
                        
                    }

                    
                }

                //texture.SetPixels(cols);

                texture.Apply();
                texCallable(texture);
            }

              
        }
        
        public static int GetMinDataValue()
        {
            if (minDataValue == int.MaxValue)
                CalculateValueBounds();
            return minDataValue;
        }

        public static int GetMaxDataValue()
        {
            if (maxDataValue == int.MinValue)
                CalculateValueBounds();
            return maxDataValue;
        }
        
        private static void CalculateValueBounds()
        {
            minDataValue = int.MaxValue;
            maxDataValue = int.MinValue;
            int dim = _dimX * _dimY * _dimZ;
            for (int i = 0; i < dim; i++)
            {
                int val = _data[i];
                minDataValue = Math.Min(minDataValue, val);
                maxDataValue = Math.Max(maxDataValue, val);
            }
        }

        private static void CurrentStep(float stepVal, int modVal)
        {
            if (stepVal % modVal == 0)
            {
                // Debug.Log("[" + stepVal + "/" + numSteps + "]:" + 
                //           " x: " + x + "/" + dimX + 
                //           " y: " + y + "/" + dimY + 
                //           " z: " + z + "/" + dimZ);
            }    
        }
        
        
    }
}