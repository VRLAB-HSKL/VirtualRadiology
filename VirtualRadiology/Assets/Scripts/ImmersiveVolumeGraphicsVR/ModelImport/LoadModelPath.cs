using System;
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
                
                ImportRAWModel.SetModelPath(DropDown.options[DropDown.value].text);
                
                Path = DropDown.options[DropDown.value].text;
                
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

                var modelPath = string.Empty;
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

                            ImportRAWModel.DefaultModelName = args[i + 1];
                            Console.WriteLine("Using model: " + ImportRAWModel.DefaultModelName);
                            
                            break;
                    }
                    
                    
                }

                if (!string.IsNullOrEmpty(ImportRAWModel.AssetFolderPath))
                {
                    modelPath = ImportRAWModel.AssetFolderPath;

                    if (!string.IsNullOrEmpty(ImportRAWModel.DefaultModelName))
                    {
                        modelPath += "\\" + ImportRAWModel.DefaultModelName;
                    }
                    else
                    {
                        modelPath += "\\Skull";
                    }
                    
                    Console.WriteLine("Using default model at " + modelPath);
                }
                
                Debug.Log("Init model path load");

                //ToDo: On no path set, default to skull for now
                if (string.IsNullOrEmpty(modelPath))
                {
                    ImportRAWModel.AssetFolderPath = Application.streamingAssetsPath;
                    ImportRAWModel.DefaultModelName = "Skull";
                    modelPath = Application.streamingAssetsPath + "\\" + ImportRAWModel.DefaultModelName;    
                }
                
                ImportRAWModel.SetModelPath(modelPath);
                
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
            
        }
    }
}
