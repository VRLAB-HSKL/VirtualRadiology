using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;
using ImmersiveVolumeGraphics.ModelImport;
namespace ImmersiveVolumeGraphics
{
    namespace Transferfunctions
    {
        /// <summary>
        /// This class handles the savingprogress of the transferfunction
        /// </summary>
        public class TransferfunctionSave : MonoBehaviour
        {

            /// <summary>
            /// Save the modified Transferfunction
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the VolumeObject</li>
            /// <li>Check if the VolumeObject exists</li>
            /// <li>Get the current Transferfunction</li>
            /// <li>Save the Transferfunction</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            /// 
            public void SaveTransferFunction()
            {
              
                // Find the VolumeObject
                VolumeRenderedObject volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                // Check if the  VolumeObject exists
                if (volumeObject != null)
                {
                    // Set the Path for Saving
                    string filePath = Application.dataPath + "/StreamingAssets/TransferFunctions/" + LoadModelPath.Path + ".tf";
                    // Get the current TransferFunction of the VolumeObject
                    TransferFunction newTF = volumeObject.transferFunction;
                    // Check if the Filepath is not empty
                    if (filePath != "")
                        // Use the Helperfunction of the UnityVolumeRendering namespace to save it
                        TransferFunctionDatabase.SaveTransferFunction(newTF, filePath);

                }


            }



        }
    }
}

