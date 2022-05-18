using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;


namespace ImmersiveVolumeGraphics { 
    namespace Transferfunctions {


        /// <summary>
        /// Loads a transferfunction from filepath and updates the corresponding transferfunction and  materials 
        /// </summary>

        public class TransferfunctionLoad : MonoBehaviour
    {

            /// <summary>
            /// The DropDown-Object of the TransferfunctionMenu
            /// </summary>
            public TMP_Dropdown dropdown;
            /// <summary>
            /// The Button-Object of the TransferfunctionMenu
            /// </summary>
            public Button Button;
            /// <summary>
            /// The filepath where the transferfunctions are stored
            /// </summary>
            public string FilePath;
            /// <summary>
            /// The VolumeObject
            /// </summary>
            private VolumeRenderedObject volumeObject;
            /// <summary>
            /// The Material for the SliceRenderers
            /// </summary>
            private Material sliceRendererMat = null;



            /// <summary>
            /// Loads a transferfunction from filepath and updates the corresponding transferfunction and  materials  
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Set the path in the project for the transferfunctions</li>
            /// <code> Application.dataPath + "/TransferFunctions/" + dropdown.options[dropdown.value].text + ".tf"</code>
            /// <li>Find the VolumeObject</li>
            /// <li>Use the Helperfunction of the UnityVolumeRendering-Project to Load the Transferfunction</li>
            /// <li>Update to render 1 dimensional transferfunctions</li>
            /// <li>Updates histogramm and transferfunctionviewer according to newest transferfunctions</li>
            /// <li>Find the SlicingPlanes</li>
            /// <li>Set the new texture to SlicingPlanes' material </li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void load() {

                //Path in the Project for the Transferfunctions
                FilePath = Application.dataPath + "/StreamingAssets/TransferFunctions/" + dropdown.options[dropdown.value].text + ".tf";



                //Finding our VolumeObject
                volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();




            if (volumeObject != null)
            {
                if (FilePath != "")
                {
                    //Helperfunction of the UnityVolumeRendering-Project to Load the Transferfunction
                    TransferFunction newTF = TransferFunctionDatabase.LoadTransferFunction(FilePath);
                    if (newTF != null)
                            //Sets the new Transferfunction for our VolumeObject
                            volumeObject.transferFunction = newTF;
                        //Update to Render 1 dimensional Transferfunctions
                        volumeObject.SetTransferFunctionMode(TFRenderMode.TF1D);


                    //Updates Histogramm and Transferfunctionviewer according to newest Transferfunction
                    Histogramm.LoadHistogramm();

                    /*
                    GameObject ImageViewer = GameObject.Find("ImageViewer1");
                    GameObject ImageViewer2 = GameObject.Find("ImageViewer2");
                    GameObject ImageViewer3 = GameObject.Find("ImageViewer3");

                    ImageViewer.GetComponent<MeshRenderer>().material.SetTexture("_TFTex", newTF.GetTexture());
                    ImageViewer2.GetComponent<MeshRenderer>().material.SetTexture("_TFTex", newTF.GetTexture());
                    ImageViewer3.GetComponent<MeshRenderer>().material.SetTexture("_TFTex", newTF.GetTexture());
                    */

                    //Find the Slicing Planes
                    GameObject slicingPlane1 = GameObject.Find("SlicingPlane1");
                    GameObject slicingPlane2 = GameObject.Find("SlicingPlane2");
                    GameObject slicingPlane3 = GameObject.Find("SlicingPlane3");

                    //Sets new Texture to SlicingPlanes' Material 
                    slicingPlane1.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", newTF.GetTexture());
                    slicingPlane2.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", newTF.GetTexture());
                    slicingPlane3.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", newTF.GetTexture());



                }
            }





        }


    }
  }

}