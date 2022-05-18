using UnityEngine;
using UnityVolumeRendering;
using RenderMode = UnityVolumeRendering.RenderMode;

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
        /// <li>UnityVolumeRendering.RenderMode 2 = IsoSurfaceRendering</li>
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
            /// <returns>void</returns>
            public void EnableDirectVolumeRendering()
            {
                var volObj = FindObjectOfType<VolumeRenderedObject>();
                if (volObj != null)
                {
                    volObj.SetRenderMode(0);
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
            /// <returns>void</returns>
            public void EnableMaximumIntensityRendering()
            {
                var volObj = FindObjectOfType<VolumeRenderedObject>();
                if (volObj != null)
                {
                    volObj.SetRenderMode((RenderMode)1);
                }
            }

            /// <summary>
            /// Enables IsoSurfaceRendering
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the VolumeObject</li>
            /// <li>Set the render mode to IsoSurfaceRendering </li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            public void EnableIsoSurfaceRendering()
            {
                var volObj = FindObjectOfType<VolumeRenderedObject>();
                if (volObj != null)
                {
                    volObj.SetRenderMode((RenderMode)2);
                }
            }

        }
    }
}