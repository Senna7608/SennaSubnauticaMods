using UnityEngine;

namespace ModdedArmsHelper.API.Interfaces
{
    /// <summary>
    /// The modded Seamoth arm can be controlled via this interface. 
    /// </summary>    
    public interface ISeamothArm
    {
        /// <summary>
        /// Simply return the gameObject property.
        /// </summary>
        GameObject GetGameObject();

        /// <summary>
        /// Use this if the modded arm can grab or hold objects like claw and propulsion arm. Otherwise return null.
        /// </summary>
        GameObject GetInteractableRoot(GameObject target);

        /// <summary>
        /// If your arm has claw capabilities returns true. Otherwise returns false.
        /// </summary>
        bool HasClaw();

        /// <summary>
        /// If your arm has drill capabilities returns true. Otherwise returns false.
        /// </summary>
        bool HasDrill();

        /// <summary>
        /// If your arm has propulsion capabilities returns true. Otherwise returns false.
        /// </summary>
        bool HasPropCannon();

        /// <summary>
        /// The arm manager calls this method when you attached arm to any side.
        /// In this method you can set up arm position, rotation, etc... in normal state for side left or right.
        /// </summary>
        void SetSide(SeamothArm arm);

        /// <summary>
        /// The arm manager calls this method when Seamoth docked in base or Cyclops.
        /// In this method you can set up arm position, rotation, etc... in docked state for side left or right.
        /// </summary>
        void SetRotation(SeamothArm arm, bool isDocked);

        /// <summary>
        /// The arm manager calls this method when the left mouse button has pressed down. 
        /// </summary>
        bool OnUseDown(out float cooldownDuration);

        /// <summary>
        /// The arm manager calls this method when the left mouse button has held. 
        /// </summary>
        bool OnUseHeld(out float cooldownDuration);

        /// <summary>
        /// The arm manager calls this method when the left mouse button has released. 
        /// </summary>
        bool OnUseUp(out float cooldownDuration);

        /// <summary>
        /// The arm manager calls this method when the alternate key button has pressed. 
        /// </summary>
        bool OnAltDown();

        /// <summary>
        /// The arm manager calls this once per frame. 
        /// </summary>
        void Update(ref Quaternion aimDirection);

        /// <summary>
        /// The arm manager calls this once per frame. 
        /// </summary>
        void ResetArm();

        /// <summary>
        ///  The arm manager calls this method to query the arm power consumption.
        /// </summary>
        float GetEnergyCost();
    }
}