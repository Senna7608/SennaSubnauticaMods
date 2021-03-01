using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using SeamothArms.InternalArmHandlers;

namespace SeamothArms.ArmHandlerRequesters
{
    public class SeamothTorpedoArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return null;
        }

        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<InternalTorpedoArmHandler>();
        }

        public void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper)
        {
        }        
    }
}