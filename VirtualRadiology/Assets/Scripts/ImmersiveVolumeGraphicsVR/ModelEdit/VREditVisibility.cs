using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;

namespace ImmersiveVolumeGraphics
{
    namespace ModelEdit {
        
        /// <summary>
        /// This class provides the Function of changing the Visibility of the 3D-Model
        /// </summary>
        public class VREditVisibility : MonoBehaviour
        {
            /// <summary>
            ///  Slider for the GUI in VR
            /// </summary>
            public Slider Slider;

            /// <summary>
            /// Edits the minimum Visibility 
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the Volume Object i.e. our model</li>
            /// <li>Get the visibily information</li>
            /// <li>Set the visibility according to the Slider´s value</li>
            /// </ul>
            /// </remarks>
            /// <returns>void</returns>
            /// 
            /// <seealso>
            /// <ul>
            /// <li>Sources:</li>
            /// <li>ValueRangeEditorWindow</li>
            /// </ul>
            /// </seealso>
            public void EditMinVisibility()
            {
                // Find the Volume Object i.e. our model
                var  volumeObject = FindObjectOfType<VolumeRenderedObject>();
                if (volumeObject != null)
                {
                    // Get the visibily information
                    var visibilityWindow = volumeObject.GetVisibilityWindow();
                    
                    // Set the visibility according to the slider´s value
                    visibilityWindow.x = Slider.value;
                    volumeObject.SetVisibilityWindow(visibilityWindow);
                }
            }

            /// <summary>
            /// Edits the maximum Visibility 
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Find the Volume Object i.e. our model</li>
            /// <li>Get the visibily information</li>
            /// <li>Set the visibility according to the Slider´s value</li>
            /// </ul>
            /// </remarks>
            /// <returns>void</returns>
            public void EditMaxVisibility()
            {
                // Find the Volume Object i.e. our model
                var volumeObject = FindObjectOfType<VolumeRenderedObject>();
                if (volumeObject != null)
                {
                    // Get the visibily information
                    var visibilityWindow = volumeObject.GetVisibilityWindow();
                    
                    // Set the visibility according to the slider´s value
                    visibilityWindow.y = Slider.value;
                    volumeObject.SetVisibilityWindow(visibilityWindow);
                }
            }

        }
    }
}