using UnityEngine;

namespace UnityVolumeRendering
{
    [ExecuteInEditMode]
    public class SlicingPlane : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial.SetMatrix("_parentInverseMat", transform.parent.worldToLocalMatrix);
            meshRenderer.sharedMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one)); // TODO: allow changing scale
        }
    }
}
