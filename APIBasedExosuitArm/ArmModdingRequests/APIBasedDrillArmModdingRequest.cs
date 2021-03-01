using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using APIBasedExosuitArms.Handlers;

namespace APIBasedExosuitArms.ArmModdingRequest
{
    public class APIBasedDrillArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<APIBasedDrillArmHandler>();
        }
       
        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return null;
        }

        public void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper)
        {
        }
    }    
}
