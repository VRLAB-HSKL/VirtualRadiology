using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ImmersiveVolumeGraphics {

    namespace Transferfunctions
    {
        public class TransferfunctionDropDown : MonoBehaviour
        {

            /// <summary>
            /// The DropdownMenu-Element
            /// </summary>
            public TMP_Dropdown DropDown;

            /// <summary>
            /// Adds the DropDownList on Start
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Adds the DropDownList on Start</li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            private void Start()
            {
                AddDropDownList();
            }

            /// <summary>
            /// Adds the DropDownList on Start
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Creates a new DropDownlist</li>
            /// <li>Creates a new DropDownlist</li>
            /// <li>Lists all Transferfunctionfiles</li>
            /// <li>Add the list of available transferfunctions to the DropDown</li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            public void  AddDropDownList()
            {
                //string path = "" + Application.dataPath + "/StreamingAssets/TransferFunctions/";
                var path = Application.streamingAssetsPath + "/TransferFunctions/";
                
                //Creates a new DropDownlist
                var DropDownOptions = new List<string>();

                //Length of the whole path
                var pathlength = path.Length;

                //Reset
                DropDown.ClearOptions();
                

                //Lists all files 
                foreach (string file in System.IO.Directory.GetFiles(path))
                {

                    //Lists all Transferfunctions
                    if (file.EndsWith(".tf"))
                    {

                        // removes the path and just leaves "Name.tf" 
                        string file2 = file.Remove(0, pathlength);
                        file2 = file2.Remove(file2.Length - 3, 3);
                        //Adds the Names of the Transferfunctions to the DropDownLists 
                        DropDownOptions.Add(file2);
                    }
                }

                // Adds the List to the DropDown as available options
                DropDown.AddOptions(DropDownOptions);
            }
            
        }
    }
}


