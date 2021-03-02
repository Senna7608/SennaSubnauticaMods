using UnityEngine;

namespace ModdedArmsHelper.API.Interfaces
{
    /// <summary>
    /// The modded arm can be registered via this interface. 
    /// </summary>
    /// <remarks>
    /// Must be specified in the <see cref="CraftableModdedArm.RegisterArm"/> method.
    /// </remarks>
    public interface IArmModdingRequest
    {
        /// <summary>
        /// Implement this method when your modded arm is <see cref="Exosuit"/> arm. If not, return null.
        /// </summary>
        IExosuitArm GetExosuitArmHandler(GameObject clonedArm);

        /// <summary>
        /// Implement this method when your modded arm is <see cref="SeaMoth"/> arm. If not, return null.
        /// </summary>
        ISeamothArm GetSeamothArmHandler(GameObject clonedArm);

        /// <summary>
        /// Implement this method for the set up your modded arm.
        /// </summary>
        void SetUpArm(GameObject clonedArm, SetupHelper graphicsHelper);
    }
}
