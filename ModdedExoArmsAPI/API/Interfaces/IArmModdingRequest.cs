using UnityEngine;

namespace ModdedArmsHelper.API.Interfaces
{
    public interface IArmModdingRequest
    {
        IExosuitArm GetExosuitArmHandler(GameObject clonedArm);
        ISeamothArm GetSeamothArmHandler(GameObject clonedArm);
        void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper);
    }
}
