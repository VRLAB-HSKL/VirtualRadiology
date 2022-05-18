using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;


namespace ImmersiveVolumeGraphics {


    namespace ModelEdit
    {

        /// <summary>
        /// This class makes it possible to edit the rotation of the 3D-Model in VR 
        /// </summary>
        public class VREditRotation : MonoBehaviour
        {
           
            /// <summary>
            /// Slider for the GUI in VR
            /// </summary>
            public Slider Slider;




            /// <summary>
            /// OnValueChangedListener for the slider, Edits the Rotation of the X-Axis in VR
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Try to find a VolumeObject in the Scene</li>
            /// <li>When the VolumeObject is not null:</li>
            /// <li>Calculate the rotation considering the Slider´s value</li>
            /// <li>Apply the rotation to the VolumeObject</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void EditRotationX()
            {
                // Find the Volume Object i.e. our model
                VolumeRenderedObject volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                // Do we have a model?
                if (volumeObject != null)
                {
                    // Rotate it 
                    Vector3 rotation = new Vector3(0, Slider.value, 0);
                    volumeObject.gameObject.transform.rotation = Quaternion.Euler(rotation);
                }
            }


            /// <summary>
            /// OnValueChangedListener for the slider, Edit the Rotation of the Y-Axis in VR
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Try to find a VolumeObject in the Scene</li>
            /// <li>When the VolumeObject is not null:</li>
            /// <li>Calculate the rotation considering the Slider´s value</li>
            /// <li>Apply the rotation to the VolumeObject</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>

            public void EditRotationY()
            {
                // Find the Volume Object i.e. our model
                VolumeRenderedObject volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                // Do we have a model?
                if (volumeObject != null)
                {
                    // Rotate it 
                    Vector3 rotation = new Vector3(Slider.value, 0, 0);
                    volumeObject.gameObject.transform.rotation = Quaternion.Euler(rotation);
                }
            }


            /// <summary>
            /// OnValueChangedListener for the slider, Edit the Rotation of the Z-Axis in VR
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Try to find a VolumeObject in the Scene</li>
            /// <li>When the VolumeObject is not null:</li>
            /// <li>Calculate the rotation considering the Slider´s value</li>
            /// <li>Apply the rotation to the VolumeObject</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            public void EditRotationZ()
            {
                // Find the Volume Object i.e. our model
                VolumeRenderedObject volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();
                // Do we have a model?
                if (volumeObject != null)
                {
                    // Rotate it 
                    Vector3 rotation = new Vector3(0, 0, Slider.value);
                    volumeObject.gameObject.transform.rotation = Quaternion.Euler(rotation);
                }
            }





        }
    }
}