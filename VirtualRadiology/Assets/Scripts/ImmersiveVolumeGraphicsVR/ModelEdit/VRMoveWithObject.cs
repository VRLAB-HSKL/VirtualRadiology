﻿using UnityEngine;

namespace ImmersiveVolumeGraphics
{
    namespace ModelEdit
    {
        /// <summary>
        /// Move an GameObject in relation to another GameObject , similar to VRRotateWithObject
        /// </summary>
        /// <seealso>
        /// <ul>
        /// <li>VRRotateWithObject</li>
        /// </ul>
        /// </seealso>
        public class VRMoveWithObject : MonoBehaviour
        {
            /// <summary>
            /// First GameObject
            /// </summary>
            public GameObject Object1;
            
            /// <summary>
            /// Second GameObject
            /// </summary>
            public GameObject Object2;
            
            /// <summary>
            /// Name of the first GameObject
            /// </summary>
            public string ObjectName1 = "";
            
            /// <summary>
            /// Name of the second GameObject
            /// </summary>
            public string ObjectName2 = "";
            
            /// <summary>
            /// Displacement value
            /// </summary>
            public float Origin = 0;
            
            /// <summary>
            /// Check if the movement is in x-Direction
            /// </summary>
            public bool XDirection;
            
            /// <summary>
            /// Check if the movement is in y-Direction
            /// </summary>
            public bool YDirection;
            
            /// <summary>
            /// Check if the movement is in z-Direction
            /// </summary>
            public bool ZDirection;

            /// <summary>
            /// Find both Objects in the Scene
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <returns>void</returns>
            private void Start()
            {
                Object1 = GameObject.Find(ObjectName1);
                Object2 = GameObject.Find(ObjectName2);
            }

            /// <summary>
            /// Change the localPosition of the first GameObject in relation of the second GameObject
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Check whether the first Object exists or not</li>
            /// <li>Check in which direction the translation happens</li>
            /// <li>Set the first Object´s localPosition to the position of the second Object (in the correct direction) </li>
            /// <li>Find  the Objects when the first Object doesnt exist (updating Objectreference)  </li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            private void Update()
            {
                if (Object1 != null)
                {
                    if (ZDirection)
                    {
                        Object1.transform.localPosition = 
                            new Vector3(0f, -(Object2.transform.position.z - Origin), 0f);
                    }

                    if (XDirection)
                    {
                        Object1.transform.localPosition = 
                            new Vector3((Object2.transform.position.x - Origin), 0f, 0f);
                    }

                    if (YDirection)
                    {
                        Object1.transform.localPosition = 
                            new Vector3(0, 0, -(Object2.transform.position.z - Origin));
                    }
                }
                else
                {
                    Object1 = GameObject.Find(ObjectName1);
                    Object2 = GameObject.Find(ObjectName2);
                }
            }

            /// <summary>
            /// Initialize the Variables and Objects
            /// </summary>
            /// <remarks>
            /// This Method is used in the ImportRAWModel-Script to initialize this class
            /// <ul>
            /// <li>Hand over the Object names</li>
            /// <li>Set the Direction-Booleans</li>
            /// </ul> 
            /// </remarks>
            /// <param name="name1"></param>
            /// <param name="name2"></param>
            /// <param name="dir"></param>
            /// <returns>void</returns>
            public void InitObj(string name1, string name2, string dir)
            {
                // ToDo: Maybe use enum for direction values instead of raw strings
                
                ObjectName1 = name1;
                ObjectName2 = name2;

                if (dir.Equals("x"))
                {
                    XDirection = true;
                }

                if (dir.Equals("y"))
                {
                    YDirection = true;
                }

                if (dir.Equals("z"))
                {
                    ZDirection = true;
                }
            }


        }
    }
}
