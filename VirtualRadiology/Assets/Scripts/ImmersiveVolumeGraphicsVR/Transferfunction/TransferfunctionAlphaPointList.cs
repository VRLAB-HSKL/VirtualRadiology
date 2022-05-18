using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;
namespace ImmersiveVolumeGraphics {
    namespace Transferfunctions
    {
        public class TransferfunctionAlphaPointList : MonoBehaviour
        {
            /// <summary>
            /// Contains the Options for the DropdownMenu
            /// </summary>
            List<string>  DropDownOptions = new List<string>();
            /// <summary>
            /// The DropDown-Element
            /// </summary>
            public TMP_Dropdown DropDown;
            /// <summary>
            /// The VolumeObject
            /// </summary>
            static VolumeRenderedObject   volobject;
            /// <summary>
            /// The current index of a AlphaPoint
            /// </summary>
            public static int AlphaPointIndex = 0;




            /// <summary>
            /// Reload the DropDownList
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Finds the VolumeObject</li>
            /// <li>Clears the Lists for Resets</li>
            /// <li>Check if the VolumeObject exists</li>
            /// <li>Add the indices of the transferfunction´s AlphaPoints to the DropDownOptionsList</li>
            /// <li>Add List to the DropDownMenu</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>

            public void ReloadList()
            {
                volobject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                DropDown.ClearOptions();
                DropDownOptions.Clear();
              
                if (volobject != null)
                {
                    for (int i = 0; i < volobject.transferFunction.alphaControlPoints.Count; i++)
                    {
                        DropDownOptions.Add(i + "");


                    }
                    DropDown.AddOptions(DropDownOptions);
                }

               
            }


            /// <summary>
            /// Change the Index of the AlphaPoints
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Set the AlphaPointIndex to the DropDown´s value</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>

            public void changeIndex()
            {

                AlphaPointIndex = DropDown.value;
                    


            }

            /// <summary>
            /// Get the current Index of the AlphaPoints
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Returns the current AlphaPointIndex</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>int</returns>
            public static int getIndex()
            {

                return AlphaPointIndex;
            }








        }
    }
}
