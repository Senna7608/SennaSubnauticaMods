using UnityEngine;

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The basic elements of the modded arm.<br/>Initialized internally.
    /// </summary>
    [DisallowMultipleComponent]
    public class ArmTag : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="ArmType"/> of the modded arm.
        /// </summary>
        public ArmType armType { get; internal set; }

        /// <summary>
        /// The <see cref="ArmTemplate"/> of the modded arm.
        /// </summary>
        public ArmTemplate armTemplate { get; internal set; }

        /// <summary>
        /// The <see cref="TechType"/> of the modded arm.
        /// </summary>
        public TechType techType { get; internal set; }
    }
}
