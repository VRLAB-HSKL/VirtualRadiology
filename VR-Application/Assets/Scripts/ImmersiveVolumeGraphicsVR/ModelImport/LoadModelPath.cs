using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmersiveVolumeGraphics.ModelImport;
using UnityVolumeRendering;
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
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void LoadPath()
            {
                //Sets the model´s path 
                // ToDo: Set init path of first found asset on startup
                Debug.Log("SetInitPath: " + DropDown.options[DropDown.value].text);
                ImportRAWModel.SetModelPath(DropDown.options[DropDown.value].text);
                //

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
            public void setPath(string location)
            {

                Path = location;


            }

            private void Start()
            {
                Debug.Log("Init model path load");
                string initPath = "Skull";
        
                ImportRAWModel.SetModelPath(initPath);
                
                //Reads the MetaInformation in 
                DICOMMetaReader.ReadDICOMMetaInformation();
                Debug.Log("Path + MetaInfo loaded");
                
                //StartCoroutine()

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
