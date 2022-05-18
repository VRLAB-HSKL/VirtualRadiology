using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ImmersiveVolumeGraphics {
    namespace ModelEdit
    {
        /// <summary>
        /// Rotate an GameObject in relation to another GameObject, this script is needed for the RotatableTable
        /// </summary>
        /// 


        /// <seealso>
        /// <ul>
        /// <li>Sources:</li>
        /// <li> [1] https://www.reddit.com/r/Unity3D/comments/duf38l/rotate_two_objects_simultaneously/f7518gl/</li>
        /// </ul>
        /// </seealso>
        public class VRRotateWithObject : MonoBehaviour
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
            /// <param name="void"></param>
            /// <returns>void</returns>
            void Start()
            {
                //
                Object1 = GameObject.Find(ObjectName1);
                Object2 = GameObject.Find(ObjectName2);

            }

            // Update is called once per frame

            /// <summary>
            /// Change the rotation  of the second GameObject in relation of the first GameObject
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Check whether the first Object exists or not</li>
            /// <li>Check in which direction the rotation happens</li>
            /// <li>Calculate the rotation vector</li>
            /// <li>Set the second Object´s rotation to the rotation of the first Object (in the correct direction) </li>
            /// <li>Find  the Objects when the first Object doesnt exist (updating Objectreference)  </li>
            /// </ul> 
            /// </remarks>
            /// <param name="void"></param>
            /// <returns>void</returns>

            void Update()
            {
                // obj.transform.position = this.transform.position;
                if (Object1 != null && Object2 != null)
                {


                    if (ZDirection)
                    {


                        Vector3 rotation = new Vector3(0.0f, 0.0f, Object1.transform.eulerAngles.z);
                        Object2.transform.rotation = Quaternion.Euler(rotation);

                    }

                    if (XDirection)
                    {

                        Vector3 rotation = new Vector3(Object1.transform.eulerAngles.y, 0.0f, 0.0f);
                        Object2.transform.rotation = Quaternion.Euler(rotation);

                    }

                    if (YDirection)
                    {

                        Vector3 rotation = new Vector3(Object2.transform.eulerAngles.x, Object1.transform.eulerAngles.y * 2, Object2.transform.eulerAngles.z);
                        Object2.transform.rotation = Quaternion.Euler(rotation);


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
            /// <li>Hand over the Objectnames</li>
            /// <li>Set the Direction-Booleans</li>
            /// </ul> 
            /// </remarks>
            /// <param name="name1"></param>
            /// <param name="name2"></param>
            /// <param name="dir"></param>
            /// <returns>void</returns>
            public void initObj(string name1, string name2, string dir)
            {
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
