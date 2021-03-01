using UnityEngine;

namespace ModdedArmsHelper.API
{
    public class SetupHelper : MonoBehaviour
    {
        public void DisableLowerArmMesh()
        {
            SkinnedMeshRenderer sMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true);

            Mesh armMesh = Instantiate(sMeshRenderer.sharedMesh) as Mesh;

            sMeshRenderer.sharedMesh = armMesh;           
            
            sMeshRenderer.sharedMesh.subMeshCount = 1;
        }

        public void AttachLowerArm(GameObject newLowerArm)
        {

        }

    }
}
