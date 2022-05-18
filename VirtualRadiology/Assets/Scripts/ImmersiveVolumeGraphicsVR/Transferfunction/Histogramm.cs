using UnityEngine;
using UnityVolumeRendering;

namespace ImmersiveVolumeGraphics
{
    /// <summary>
    /// This namespace containts all components of the transferfunctions of the 3D-Model
    /// </summary>
    namespace Transferfunctions {

        /// <summary>
        /// This class handles the histogramfeature in VR
        /// </summary>
        public class Histogramm : MonoBehaviour
        {

            /// <summary>
            /// This is the VolumeObject (the 3D-Model)
            /// </summary>
            static VolumeRenderedObject volumeObject;
            
            /// <summary>
            /// This is the transfer function material of the 3D-Model
            /// </summary>
            private static Material tfGUIMat;
            /// <summary>
            /// This is the color material for the transfer function
            /// </summary>
            private static Material tfPaletteGUIMat;
            /// <summary>
            /// This is the histogramtexture
            /// </summary>
            private static Texture2D histTex;
            /// <summary>
            /// This is the used transferfunction
            /// </summary>
            private static  TransferFunction transferfunction;


            //For OnClickListener


            /// <summary>
            /// Create  the histogram
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the TransferFunctionGUIMat and set the transferfunctionmaterial</li>
            /// <li>Find the VolumeObject and check if it exists</li>
            /// <li>Set the transferfunction and generate the Texture</li>
            /// <li>Generate the histogramtexture and set it</li>
            /// <li>Set the _TFTex-Texture of the Shader from the local transferfunction </li>
            /// <li>Set the _HistTex-Texture of the Shader from the local histogramtexture </li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            public void CreateHistogramm()
            {
                tfGUIMat = Resources.Load<Material>("TransferFunctionGUIMat");
                tfPaletteGUIMat = Resources.Load<Material>("TransferFunctionPaletteGUIMat");
                volumeObject = FindObjectOfType<VolumeRenderedObject>();

                if (volumeObject != null)
                {
                    transferfunction = volumeObject.transferFunction;
                    transferfunction.GenerateTexture();
                    if (histTex == null)
                        histTex = HistogramTextureGenerator.GenerateHistogramTexture(volumeObject.dataset);

                    tfGUIMat.SetTexture("_TFTex", transferfunction.GetTexture());
                    tfGUIMat.SetTexture("_HistTex", histTex);
                    tfPaletteGUIMat.SetTexture("_TFTex", transferfunction.GetTexture());
                }
            }

            /// <summary>
            /// Create  the histogram
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the TransferFunctionGUIMat and set the transfer function material</li>
            /// <li>Find the VolumeObject and check if it exists</li>
            /// <li>Set the transfer function and generate the Texture</li>
            /// <li>Generate the histogram texture and set it</li>
            /// <li>Set the _TFTex-Texture of the Shader from the local transfer function </li>
            /// <li>Set the _HistTex-Texture of the Shader from the local histogram texture </li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            public static void LoadHistogramm()
            {
                tfGUIMat = Resources.Load<Material>("TransferFunctionGUIMat");
                tfPaletteGUIMat = Resources.Load<Material>("TransferFunctionPaletteGUIMat");
                volumeObject = FindObjectOfType<VolumeRenderedObject>();

                if (volumeObject != null)
                {
                    transferfunction = volumeObject.transferFunction;
                    transferfunction.GenerateTexture();
                    if (histTex == null)
                        histTex = HistogramTextureGenerator.GenerateHistogramTexture(volumeObject.dataset);

                    tfGUIMat.SetTexture("_TFTex", transferfunction.GetTexture());
                    tfGUIMat.SetTexture("_HistTex", histTex);
                    tfPaletteGUIMat.SetTexture("_TFTex", transferfunction.GetTexture());

                    /*
                    for (int iAlpha = 0; iAlpha < transferfunction.alphaControlPoints.Count; iAlpha++)
                    {

                        Debug.Log("Alpha alpha   "+ transferfunction.alphaControlPoints[iAlpha].alphaValue);
                            Debug.Log("Alpha datavalue   " + transferfunction.alphaControlPoints[iAlpha].dataValue);
                        }

                    for (int iAlpha = 0; iAlpha < transferfunction.colourControlPoints.Count; iAlpha++)
                    {

                        Debug.Log("Color value   "+ transferfunction.colourControlPoints[iAlpha].colourValue);
                            Debug.Log("Color datavalue   " + transferfunction.colourControlPoints[iAlpha].dataValue);
                        }

                        Debug.Log(transferfunction.alphaControlPoints.Count);
                        Debug.Log(transferfunction.colourControlPoints.Count);
                    */
                }
                
            }
        }
    }
}
