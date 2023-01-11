using UnityEngine;
using ModdedArmsHelper.API.Interfaces;

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The helper class to setting up the modded arm.<br/>The <see cref="IArmModdingRequest"/> interface called this when creating the new arm prefab.
    /// </summary>
    public class LowerArmHelper : MonoBehaviour
    {
        /// <summary>
        /// This method hide the vanilla arm lower arm mesh in the SkinnedMeshRenderer.
        /// </summary>
        private void DisableLowerArmMesh()
        {
            SkinnedMeshRenderer sMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true);

            Mesh armMesh = Instantiate(sMeshRenderer.sharedMesh);

            sMeshRenderer.sharedMesh = armMesh;           
            
            sMeshRenderer.sharedMesh.subMeshCount = 1;
        }

        /// <summary>
        /// Call this method to insert a new lower arm gameobject.<br/>The gameobject is inserted into ../elbow/ModdedLowerArmContainer
        /// </summary>
        public GameObject AttachNewLowerArm(GameObject newLowerArm, bool instantiate)
        {
            DisableLowerArmMesh();

            GameObject ModdedLowerArmContainer = ArmServices.main.objectHelper.FindDeepChild(transform, "ModdedLowerArmContainer");

            if (instantiate)
            {
                GameObject clone = Instantiate(newLowerArm);
                clone.transform.SetParent(ModdedLowerArmContainer.transform, false);
                return clone;
            }
            
            newLowerArm.transform.SetParent(ModdedLowerArmContainer.transform, false);
            return newLowerArm;
        }
    }
}
