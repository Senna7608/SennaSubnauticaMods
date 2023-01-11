using UnityEngine;
using ModdedArmsHelper.API;
using SeamothArms.InternalArmHandlers;
using ModdedArmsHelper.API.Interfaces;
using System.Collections;

namespace SeamothArms.ArmHandlerRequesters
{
    public class SeamothDrillArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return null;
        }        

        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<InternalDrillArmHandler>();
        }

        public IEnumerator SetUpArmAsync(GameObject clonedArm, LowerArmHelper graphicsHelper, IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}
