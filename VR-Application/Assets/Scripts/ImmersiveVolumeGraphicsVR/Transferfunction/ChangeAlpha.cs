using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;
using ImmersiveVolumeGraphics.Transferfunctions;
namespace ImmersiveVolumeGraphics
{
    namespace Transferfunctions
{
        /// <summary>
        /// This class changes the AlphaControlPoints of the Transferfunction
        /// </summary>


        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] TransferFunctionEditorWindow </li>
        /// </ul>
        /// </seealso>


        public class ChangeAlpha : MonoBehaviour
        {




            /// <summary>
            /// This is the VolumeObject (the 3D Model)
            /// </summary>
            static VolumeRenderedObject volumeObject;

            /// <summary>
            /// This is the transferfunction of the VolumeObject
            /// </summary>
            private static TransferFunction transferfunction = null;

            /// <summary>
            /// This is the slider used to change the alphavalue
            /// </summary>
            public Slider AlphaSlider;

            /// <summary>
            /// This represents an alphapoint of the transferfunction
            /// </summary>
            private TFAlphaControlPoint alphaPoint = new TFAlphaControlPoint();

            /// <summary>
            /// This is the material used for the transferfunction
            /// </summary>
            private static Material tfGUIMat = null;

            /// <summary>
            /// Imports a 3D-RAW-Model into the scene by using voice commands 
            /// </summary>
            /// <remarks>
            /// 
            /// <ul>
            /// <li>Find the VolumeObject </li>
            /// <li>Load the TransferfunctionMaterial</li>
            /// <li>Set the alphapointsvalue according to the slider´s value</li>
            /// <li>Set the datavalue according to the current AlphaControlPoint´s datavalue</li>
            /// <li>Swap the oldpoint with the new AlphaControlPoint</li>
            /// <li>Update the Materials</li>
            /// </ul>
            public void ChangeAlphaValue()
            {


                volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                tfGUIMat = Resources.Load<Material>("TransferFunctionGUIMat");

                if (volumeObject != null &&tfGUIMat!=null)
                {
                    transferfunction = volumeObject.transferFunction;
                  

                    /*
                    for (int iAlpha = 0; iAlpha < transferfunction.alphaControlPoints.Count; iAlpha++)
                    {

                        Debug.Log("Alpha alpha   " + transferfunction.alphaControlPoints[iAlpha].alphaValue);
                        Debug.Log("Alpha datavalue   " + transferfunction.alphaControlPoints[iAlpha].dataValue);
                    }

                    for (int iAlpha = 0; iAlpha < transferfunction.colourControlPoints.Count; iAlpha++)
                    {

                        Debug.Log("Color value   " + transferfunction.colourControlPoints[iAlpha].colourValue);
                        Debug.Log("Color datavalue   " + transferfunction.colourControlPoints[iAlpha].dataValue);
                    }

                    Debug.Log(transferfunction.alphaControlPoints.Count);
                    Debug.Log(transferfunction.colourControlPoints.Count);
                    */

                    alphaPoint.alphaValue = AlphaSlider.value;
                    alphaPoint.dataValue = transferfunction.alphaControlPoints[TransferfunctionAlphaPointList.getIndex()].dataValue;
                    transferfunction.alphaControlPoints[TransferfunctionAlphaPointList.getIndex()] = alphaPoint;
                    transferfunction.GenerateTexture();
                    tfGUIMat.SetTexture("_TFTex", transferfunction.GetTexture());

                   
                    GameObject slicingPlane1 = GameObject.Find("SlicingPlane1");
                    GameObject slicingPlane2 = GameObject.Find("SlicingPlane2");
                    GameObject slicingPlane3 = GameObject.Find("SlicingPlane3");
                  
                    //Sets new Texture to SlicingPlanes' Material 
                    slicingPlane1.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", tfGUIMat.GetTexture("_TFTex"));
                    slicingPlane2.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", tfGUIMat.GetTexture("_TFTex"));
                    slicingPlane3.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_TFTex", tfGUIMat.GetTexture("_TFTex"));
                 

                }






            }
        }

    }

}
