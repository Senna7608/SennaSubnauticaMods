using System.Collections;
using UnityEngine;

namespace ModdedArmsHelper.API.Interfaces
{
    /// <summary>
    /// The modded arm can be handled via this interface. 
    /// </summary>
    /// <remarks>
    /// Must be specified in the <see cref="CraftableModdedArm.RegisterArm"/> method.
    /// </remarks>
    public interface IArmModdingRequest
    {
        /// <summary>
        /// Implement this method and return interface when your modded arm is <see cref="Exosuit"/> arm. If not, return null.
        /// </summary>
        IExosuitArm GetExosuitArmHandler(GameObject clonedArm);

        /// <summary>
        /// Implement this method and return interface when your modded arm is <see cref="SeaMoth"/> arm. If not, return null.
        /// </summary>
        ISeamothArm GetSeamothArmHandler(GameObject clonedArm);

        /// <summary>
        /// Implement this async method for the set up your equipped modded arm model. (Not the open world or in the inventory!).
        /// </summary>
        IEnumerator SetUpArmAsync(GameObject clonedArm, LowerArmHelper graphicsHelper, IOut<bool> success);        
    }
}
