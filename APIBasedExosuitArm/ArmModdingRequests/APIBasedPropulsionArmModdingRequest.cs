using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using APIBasedExosuitArms.Handlers;

namespace APIBasedExosuitArms.ArmModdingRequest
{
    public class APIBasedPropulsionArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<APIBasedPropulsionArmHandler>();
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
