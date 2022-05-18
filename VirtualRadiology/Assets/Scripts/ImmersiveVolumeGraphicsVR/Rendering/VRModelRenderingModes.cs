using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

namespace ImmersiveVolumeGraphics
{


    /// <summary>
    /// This namespace containts all components that interact with the Renderingpipeline of the 3D-Model
    /// </summary>
    namespace Rendering {



        /// <summary>
        /// This class enables changing the RenderingModes
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>UnityVolumeRendering.RenderMode 0 = DirectVolumeRendering</li>
        /// <li>UnityVolumeRendering.RenderMode 1 = MaximumIntensityRendering</li>
        /// <li>UnityVolumeRendering.RenderMode 2 = IsosurfaceRendering</li>
        /// </ul> 
        /// </remarks>

        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] RenderMode </li>
        /// </ul>
        /// </seealso>


        public class VRModelRenderingModes : MonoBehaviour
        {
            /// <summary>
            /// Enables DirectVolumeRendering
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the VolumeObject</li>
            /// <li>Set the rendermode to DirectVolumeRendering </li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>

            public void EnableDirectVolumeRendering()
            {
                VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                if (volobj != null)
                {
                    volobj.SetRenderMode((UnityVolumeRendering.RenderMode)0);
                }
            }


            /// <summary>
            /// Enables MaximumIntensityRendering
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the VolumeObject</li>
            /// <li>Set the rendermode to MaximumIntensityRendering </li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void EnableMaximumIntensityRendering()
            {

                VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                if (volobj != null)
                {
                    volobj.SetRenderMode((UnityVolumeRendering.RenderMode)1);
                }
            }

            /// <summary>
            /// Enables IsosurfaceRendering
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the VolumeObject</li>
            /// <li>Set the rendermode to IsosurfaceRendering </li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void EnableIsosurfaceRendering()
            {

                VolumeRenderedObject volobj = GameObject.FindObjectOfType<VolumeRenderedObject>();
                if (volobj != null)
                {
                    volobj.SetRenderMode((UnityVolumeRendering.RenderMode)2);
                }



            }




        }

    }
}