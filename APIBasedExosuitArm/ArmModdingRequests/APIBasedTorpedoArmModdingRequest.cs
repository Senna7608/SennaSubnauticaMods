using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using APIBasedExosuitArms.Handlers;

namespace APIBasedExosuitArms.ArmModdingRequest
{
    public class APIBasedTorpedoArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<APIBasedTorpedoArmHandler>();
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
