using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;
using UnityEngine.Serialization;
using UnityVolumeRendering;


namespace ImmersiveVolumeGraphics
{
    /// <summary>
    /// This namespace containts all VR-Interactioncomponents: Buttons, Handles etc.
    /// </summary>
    namespace Interactions
    {
        /// <summary>
        /// This class handles Logic of the Down button
        /// </summary>
        public class VRDownButton : MonoBehaviour, IColliderEventPressEnterHandler, IColliderEventPressExitHandler
        {
            
            [FormerlySerializedAs("m_activeButton")] [SerializeField]
            private ColliderButtonEventData.InputButton mActiveButton = ColliderButtonEventData.InputButton.Trigger;

            /// <summary>
            /// This Vector shows how much the Button will be displaced while pressing
            /// </summary>
            [FormerlySerializedAs("ButtonDownDisplacement")] public Vector3 buttonDownDisplacement;

            /// <summary>
            /// This Vector shows how much the 3D-Model will be displaced while pressing
            /// </summary>
            public Vector3 ObjectDisplacement;
            /// <summary>
            /// This is the Buttonobject
            /// </summary>
            public Transform ButtonObject;
            
            /// <summary>
            /// 
            /// </summary>
            private bool _getDownwards;

            /// <summary>
            /// The current 3D-Model 
            /// </summary>
            private VolumeRenderedObject _volumeObject;
            
            /// <summary>
            /// The base of the Console
            /// </summary>
            private GameObject _consoleBase;
            
            /// <summary>
            /// Side panels without the Sliders
            /// </summary>
            private GameObject _regulator1;
            
            /// <summary>
            /// Side panels without the Sliders
            /// </summary>
            private GameObject _regulator2;
            
            /// <summary>
            /// Side panels without the Sliders
            /// </summary>
            private GameObject _regulator3;
            
            /// <summary>
            /// 
            /// </summary>
            private GameObject _sliceRenderer;


            /// <summary>
            /// Finding the GameObjects in the Scene
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Finding the  consoleBase</li>
            /// <li>Finding the left Regulator</li>
            /// <li>Finding the right Regulator</li>
            /// <li>Finding the front Regulator</li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>
            /// 
            private void Start()
            {
                _consoleBase = GameObject.Find("ConsoleBase");
                _regulator1 = GameObject.Find("Regulator");
                _regulator2 = GameObject.Find("Regulator (1)");
                _regulator3 = GameObject.Find("Regulator (2)");
                _sliceRenderer = GameObject.Find("EditSliceRenderer3");
            }

            /// <summary>
            /// Handles the button press when the object is entered
            /// </summary>
            /// <remarks>
            /// Behaviour when the Button is pressed:
            /// <ul>
            /// 
            /// <li>
            /// The lower part of the Button will be displaced depending on the value of buttonDownDisplacement
            /// </li>
            /// <li>Finding the current VolumeObject</li>
            /// <li>Downward-Movement (getDownwards) will be enabled</li>
            /// </ul> 
            /// </remarks>
            /// <param name="eventData"></param>
            /// <returns>void</returns>
            public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
            {
                if (eventData.button == mActiveButton)
                {
                    _volumeObject = FindObjectOfType<VolumeRenderedObject>();
                    ButtonObject.localPosition += buttonDownDisplacement;
                    _getDownwards = true;
                }
            }

            /// <summary>
            /// Handles the buttonpress when the object is exited
            /// </summary>
            /// <remarks>
            /// Behaviour when the Button is pressed:
            /// <ul>
            /// 
            /// <li>Resets the Buttonposition </li>
            /// <li>Disables the Downward-Movement</li>
            /// </ul> 
            /// </remarks>
            /// <param name="eventData"></param>
            /// <returns>void</returns>
            public void OnColliderEventPressExit(ColliderButtonEventData eventData)
            {
                ButtonObject.localPosition -= buttonDownDisplacement;
                _getDownwards = false;
            }

            /// <summary>
            /// Handles the ObjectDisplacement
            /// </summary>
            /// <remarks>
            /// Behaviour when Downward-Movement is enabled, the VolumeObject is not null and the localPosition of the
            /// console base is greater than -1.3 (bottom position)
            /// <ul>
            /// <li>The console base, the regulators and the 3D-Model will be displaced depending on objectDisplacement
            /// and Time.deltaTime every Frame</li>
            /// </ul> 
            /// </remarks>
            /// <param name="eventData"></param>
            /// <returns>void</returns>
            private void Update()
            {
                if (_getDownwards)
                {
                    if (_volumeObject != null)
                    {
                        if (_consoleBase.transform.localPosition.y > -1.3f)
                        {
                            _consoleBase.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            _regulator1.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            _regulator2.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            _regulator3.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            _sliceRenderer.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            _volumeObject.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                        }
                    }
                }
            }
        }
    }
}