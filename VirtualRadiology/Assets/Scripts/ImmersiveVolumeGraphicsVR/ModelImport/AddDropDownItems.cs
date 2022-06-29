using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ImmersiveVolumeGraphics {
    
    namespace ModelImport
    {
        /// <summary>
        /// Adding the Modelnames to the Dropdownobject 
        /// </summary>
        /// 
        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] https://answers.unity.com/questions/16433/get-list-of-all-files-in-a-directory.html </li>
        /// <li> [2] https://forum.unity.com/threads/remove-text-from-string.57431/ </li>
        /// <li> [3] https://docs.unity3d.com/Manual/StreamingAssets.html </li>
        /// </ul>
        /// </seealso>
        public class AddDropDownItems : MonoBehaviour
        {
            /// <summary>
            /// The Dropdownobject 
            /// </summary>
            public TMP_Dropdown Dropdown;
            /// <summary>
            /// <ul>
            /// <li>This is the Path to the 3D-Models in the Unity folder structure</li>
            /// <li>Application.dataPath + "/StreamingAssets/" </li>
            /// </ul>
            /// </summary>
            private string _path = string.Empty;

            /// <summary>
            /// Adding Options to the DropDownObject
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Creates a new drop down list</li>
            /// <li>Lists just the 3D-Models ending in .raw</li>
            /// <li>Adds the Names of the Models to the List </li>
            /// <li>Adds the List to the DropDown as available options</li>
            /// </ul>
            /// </remarks>
            /// <returns>void</returns>
            private void Start()
            {

// #if UNITY_EDITOR
//                 _path = Application.streamingAssetsPath; //string.Empty + Application.dataPath + "/StreamingAssets/";
// #else
//                 string path = string.Empty + Application//string.Empty + Application.dataPath + "/StreamingAssets/";
//#endif

                _path = ImportRAWModel.AssetFolderPath; //Application.streamingAssetsPath;

                //Creates a new drop down list
                var dropDownOptions = new List<string>();

                //Length of the whole path
                var pathLength = _path.Length;

                //Lists all files 
                foreach (var file in System.IO.Directory.GetFiles(_path))
                {
                    //Listing just the Raw models ending in .raw
                    if (file.EndsWith(".raw"))
                    {
                        // removes the path and just leaves "Name.raw" 
                        var file2 = file.Remove(0, pathLength);
                        file2 = file2.Remove(file2.Length - 4, 4);
                        
                        // asb
                        
                        // Pop slash character at the beginning
                        file2 = file2.Remove(0, 1);
                                              
                        Debug.Log("Set dropdown entry: " + file2);
                        
                        //Adds the Names of the Models to the DropDownLists 
                        dropDownOptions.Add(file2);
                    }
                }

                // Adds the List to the DropDown as available options
                Dropdown.AddOptions(dropDownOptions);
            }

        }
    }

}
