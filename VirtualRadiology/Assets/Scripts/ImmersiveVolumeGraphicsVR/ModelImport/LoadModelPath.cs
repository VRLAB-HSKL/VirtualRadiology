using System;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

namespace ImmersiveVolumeGraphics {

    namespace ModelImport
    {
        public class LoadModelPath : MonoBehaviour
        {
            /// <summary>
            /// Loading the model´s path and DICOM-Information
            /// </summary>
            
            /// <summary>
            /// The DropDown-Object
            /// </summary>
            public TMP_Dropdown DropDown;

            /// <summary>
            /// The model´s path
            /// </summary>
            public static string Path;
            // Start is called before the first frame update

            /// <summary>
            /// The OnValueChanged-Listener for the DropDown-Object
            /// </summary>
            /// <remarks>
            /// Loading the model´s path and DICOM-Information
            /// <ul>
            /// <li>Sets the model´s path </li>
            /// <li>Get the path from the DropDown-Object and set the path-variable</li>
            /// <li>Read the DICOM-Information</li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            public void LoadPath()
            {
                //Sets the model´s path 
                // ToDo: Set init path of first found asset on startup
                //Debug.Log("SetInitPath: " + DropDown.options[DropDown.value].text);

                var dropDownValue = DropDown.options[DropDown.value].text;
                
                //ImportRAWModel.SetModelPath(dropDownValue);

                ImportRAWModel.ModelName = dropDownValue;
                
                Path = dropDownValue;
                
                //Reads the MetaInformation in 
                DICOMMetaReader.ReadDICOMMetaInformation();
                Debug.Log("Path + MetaInfo loaded");
            }

            //setter-Method for the path variable
            /// <summary>
            /// Setter-Method for the path-variable
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Sets the model´s path </li>
            /// </ul> 
            /// </remarks>
            /// <param name="location"></param>
            /// <returns>void</returns>
            public void SetPath(string location)
            {
                Path = location;
            }

            private void Start()
            {
                //ToDo: Parse command line arguments after we have a concept
                var args = Environment.GetCommandLineArgs();

                //var modelPath = string.Empty;
                // var assetFolderPath = string.Empty;
                // var modelName = string.Empty;
                
                for(var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    Debug.Log("CLI Argument [" + i + "]: " + arg);
                    
                    switch (arg)
                    {
                        case "-asset_folder_path":
                        case "-ap":
                            
                            if (i + 1 >= args.Length)
                            {
                                //Debug.LogError("No path given to -p flag!");
                                Console.WriteLine("No asset folder path given to -ap flag!");
                                return;
                            }

                            ImportRAWModel.AssetFolderPath = args[i + 1];
                            Console.WriteLine("Using asset folder: " + ImportRAWModel.AssetFolderPath);
                            
                            break;
                        
                        case "-model":
                        case "-m":
                            
                            if (i + 1 >= args.Length)
                            {
                                Console.WriteLine("No model name given to -m flag!");
                                return;
                            }

                            ImportRAWModel.ModelName = args[i + 1];
                            Console.WriteLine("Using model: " + ImportRAWModel.ModelName);
                            
                            break;
                    }
                    
                    
                }

                if (!string.IsNullOrEmpty(ImportRAWModel.AssetFolderPath))
                {
                    // If only asset folder path is set, use first file in folder as model name
                    if (string.IsNullOrEmpty(ImportRAWModel.ModelName))
                    {
                        var firstFileName = GetFirstFilenameInPath(ImportRAWModel.AssetFolderPath);
                        ImportRAWModel.ModelName = firstFileName;
                    }
                    
                    //modelPath += "/" + modelName;
                    Console.WriteLine("Using default model at " + ImportRAWModel.ModelPath);
                }
                else
                {
                    //On no path set, default to first file in folder for now    
                    ImportRAWModel.AssetFolderPath = Application.streamingAssetsPath;
                    if (string.IsNullOrEmpty(ImportRAWModel.ModelName))
                    {
                        var firstFilename = GetFirstFilenameInPath(ImportRAWModel.AssetFolderPath);
                        ImportRAWModel.ModelName = firstFilename;
                        //modelPath = Application.streamingAssetsPath + "/" + ImportRAWModel.DefaultModelName;    
                    }
                    
                }
                
                
                //ImportRAWModel.SetModelPath(modelPath);
                Debug.Log("Model path load - " + ImportRAWModel.ModelPath);
                
                //Reads the MetaInformation
                DICOMMetaReader.ReadDICOMMetaInformation();
                Debug.Log("Path + MetaInfo loaded");

                var importer = FindObjectOfType<ImportRAWModel>();
                if (importer != null)
                {
                    Debug.Log("Importing initial data");
                    //importer.OpenRAWData();    
                    StartCoroutine(importer.OpenRawDataRoutine());
                }
                
            }


            private string GetFirstFilenameInPath(string path)
            {
                // Attempt to get the first file in the target folder
                var di = new DirectoryInfo(path);
                var firstFileName = di.GetFiles().Select(fi => fi.Name).FirstOrDefault();
                firstFileName = firstFileName.Substring(0, firstFileName.Length - 4);
                return firstFileName;
            }
            
        }
    }
}
