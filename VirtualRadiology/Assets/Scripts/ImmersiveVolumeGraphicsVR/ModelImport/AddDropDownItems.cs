using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
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
            /// <li>This is the Path to the 3D-Models in the Unityfolderstructure</li>
            /// <li>Application.dataPath + "/StreamingAssets/" </li>
            /// </ul>
            /// </summary>
            private string path="";



            /// <summary>
            /// Adding Options to the DropDownObject
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Creates a new DropDownlist</li>
            /// <li>Lists just the 3D-Models ending in .raw</li>
            /// <li>Adds the Names of the Models to the List </li>
            /// <li>Adds the List to the DropDown as available options</li>
            /// </ul>
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            void Start()
            {

#if UNITY_EDITOR

                

                 path = "" + Application.dataPath + "/StreamingAssets/";

#else
         string path = "" + Application.dataPath + "/StreamingAssets/";

#endif

                //Creates a new DropDownlist
                List<string> dropDownOptions = new List<string>();

                //Length of the whole path
                int pathLength = path.Length;

                //Lists all files 
                foreach (string file in System.IO.Directory.GetFiles(path))
                {

                    //Listing just the Rawmodels ending in .raw
                    if (file.EndsWith(".raw"))
                    {

                        // removes the path and just leaves "Name.raw" 
                        string file2 = file.Remove(0, pathLength);
                        file2 = file2.Remove(file2.Length - 4, 4);
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
