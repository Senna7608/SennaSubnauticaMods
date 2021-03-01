using UnityEngine;
using SeamothArms.InternalArmHandlers;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;

namespace SeamothArms.ArmHandlerRequesters
{
    public class SeamothClawArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return null;
        }        

        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<InternalClawArmHandler>();
        }

        public void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper)
        {            
        }
    }
}
