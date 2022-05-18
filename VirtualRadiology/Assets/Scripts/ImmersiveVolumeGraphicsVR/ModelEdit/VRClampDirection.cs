using UnityEngine;

namespace ImmersiveVolumeGraphics {

    /// <summary>
    /// This namespace containts all components that can edit or interact with the 3D-Model 
    /// </summary>
    namespace ModelEdit
    {
        /// <summary>
        /// This class clamps the position of the Sliders that users cannot grab them out of the Object
        /// </summary>
        public class VRClampDirection : MonoBehaviour
        {
            /// <summary>
            /// Boolean to check if the clamping should happen in x-Direction
            /// </summary>
            public bool XDirection;
            /// <summary>
            /// Boolean to check if the clamping should happen in y-Direction
            /// </summary>
            public bool YDirection;
            /// <summary>
            /// Boolean to check if the clamping should happen in z-Direction
            /// </summary>
            public bool ZDirection;
            /// <summary>
            /// Offsetvalue in X-Direction from Startposition
            /// </summary>
            public float OffsetX;
            /// <summary>
            /// Offsetvalue in Y-Direction from Startposition
            /// </summary>
            public float OffsetY;
            /// <summary>
            /// Offsetvalue in Z-Direction from Startposition
            /// </summary>
            public float OffsetZ;
            /// <summary>
            /// Startposition
            /// </summary>
            public Vector3 StartPosition;

            /// <summary>
            ///  Sets the start position
            /// </summary>
            /// <remarks> 
            /// </remarks>
            /// <returns>void</returns>
            private void Start()
            {
                var position = transform.position;
                StartPosition = new Vector3(position.x, position.y, position.z);
                
                // ToDo: Calculate offset based on border bounding boxes to make it more exact
            }

            /// <summary>
            /// Applies clamping to a GameObject
            /// </summary>
            /// <remarks>
            /// <ul>
            /// <li>Checks in which direction the clamping should occur</li>
            /// <li>Applies clamping to the GameObject considering the offset value on every frame</li>
            /// </ul> 
            /// </remarks>
            /// <returns>void</returns>
            private void Update()
            {
                var localPosition = gameObject.transform.localPosition;
                
                if (XDirection)
                {
                    if (localPosition.x >= StartPosition.x + OffsetX)
                    {
                        localPosition = new Vector3(StartPosition.x + OffsetX, 
                                localPosition.y, 
                                localPosition.z);
                        
                    }

                    if (localPosition.x <= StartPosition.x - OffsetX)
                    {
                        localPosition = new Vector3(StartPosition.x - OffsetX, 
                            localPosition.y, localPosition.z);
                        
                    }
                }

                if (YDirection)
                {
                    if (localPosition.y >= StartPosition.y + OffsetY)
                    {
                        localPosition = new Vector3(localPosition.x, StartPosition.y + OffsetY, localPosition.z);
                    }

                    if (localPosition.y <= StartPosition.y - OffsetY)
                    {
                        localPosition = new Vector3(localPosition.x, StartPosition.y - OffsetY, localPosition.z);
                    }
                }

                if (ZDirection)
                {
                    if (localPosition.z >= StartPosition.z + OffsetZ)
                    {
                        localPosition = new Vector3(localPosition.x, localPosition.y, StartPosition.z + OffsetZ);
                    }

                    if (localPosition.z <= StartPosition.z - OffsetZ)
                    {
                        localPosition = new Vector3(localPosition.x, localPosition.y, StartPosition.z - OffsetZ);
                    }
                }

                gameObject.transform.localPosition = localPosition;
            }

        }

    }
}

