using UnityEngine;
using ModdedArmsHelper.API;
using SeamothArms.InternalArmHandlers;
using ModdedArmsHelper.API.Interfaces;

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

        public void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper)
        {            
        }
    }
}
