using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using UnityVolumeRendering;



namespace ImmersiveVolumeGraphics
{
    /// <summary>
    /// This namespace containts all VR-Interactioncomponents:  Buttons, Handles etc.
    /// </summary>
    namespace Interactions
    {

        /// <summary>
        /// This class handles Logic of the Downbutton
        /// </summary>
        public class VRDownButton : MonoBehaviour, IColliderEventPressEnterHandler
            , IColliderEventPressExitHandler
        {

            [SerializeField]
            private ColliderButtonEventData.InputButton m_activeButton = ColliderButtonEventData.InputButton.Trigger;


            /// <summary>
            /// This Vector shows how much the Button will be displaced while pressing
            /// </summary>
            public Vector3 ButtonDownDisplacement;


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
            private bool getDownwards = false;

            /// <summary>
            /// The current 3D-Model 
            /// </summary>
            private VolumeRenderedObject volumeObject;
            /// <summary>
            /// The Base of the Console
            /// </summary>
            private GameObject consoleBase;
            /// <summary>
            /// Sidepanels without the Sliders
            /// </summary>
            private GameObject regulator1;
            /// <summary>
            /// Sidepanels without the Sliders
            /// </summary>
            private GameObject regulator2;
            /// <summary>
            /// Sidepanels without the Sliders
            /// </summary>
            private GameObject regulator3;
            /// <summary>
            /// 
            /// </summary>
            private GameObject sliceRenderer;


            /// <summary>
            /// Finding the GameObjects in the Scene
            /// </summary>
            /// <remarks>
            /// 
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
            void Start()
            {

                consoleBase = GameObject.Find("ConsoleBase");
                regulator1 = GameObject.Find("Regulator");
                regulator2 = GameObject.Find("Regulator (1)");
                regulator3 = GameObject.Find("Regulator (2)");
                sliceRenderer = GameObject.Find("EditSliceRenderer3");

            }




            /// <summary>
            /// Handles the buttonpress when the object is entered
            /// </summary>
            /// <remarks>
            /// Behaviour when the Button is pressed:
            /// <ul>
            /// 
            /// <li>The lower part of the Button will be displaced depending on the value of buttonDownDisplacement </li>
            /// <li>Finding the current VolumeObject</li>
            /// <li>Downward-Movement (getDownwards) will be enabled</li>
            /// </ul> 
            /// </remarks>
            /// <param name="eventData"></param>
            /// <returns>void</returns>
            public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
            {
                if (eventData.button == m_activeButton)
                {
                    volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();

                    ButtonObject.localPosition += ButtonDownDisplacement;
                    getDownwards = true;





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
                ButtonObject.localPosition -= ButtonDownDisplacement;
                getDownwards = false;
            }



            // Update is called once per frame

            /// <summary>
            /// Handles the ObjectDisplacement
            /// </summary>
            /// <remarks>
            /// Behaviour when Downward-Movement is enabled, the VolumeObject is not null and the localPosition of the consoleBase is greater than -1.3 (Bottomposition)
            /// <ul>
            /// <li>The ConsoleBase, the Regulators and the 3D-Model wll be displaced depending on objectDisplacement and Time.deltaTime every Frame</li>
            /// </ul> 
            /// </remarks>
            /// <param name="eventData"></param>
            /// <returns>void</returns>
            void Update()
            {
                if (getDownwards)
                {
                    if (volumeObject != null)
                    {

                        if (consoleBase.transform.localPosition.y > -1.3f)
                        {

                            consoleBase.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            regulator1.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            regulator2.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            regulator3.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            sliceRenderer.transform.localPosition += ObjectDisplacement * Time.deltaTime;
                            volumeObject.transform.localPosition += ObjectDisplacement * Time.deltaTime; ;
                            //Debug.Log("runter");

                        }



                       

                    }


                }
            }



        }
    }
}