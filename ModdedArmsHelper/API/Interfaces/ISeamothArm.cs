using UnityEngine;

namespace ModdedArmsHelper.API.Interfaces
{
    public interface ISeamothArm
    {        
        GameObject GetGameObject();               

        bool HasClaw();

        bool HasDrill();

        bool HasPropCannon();

        void SetSide(SeamothArm arm);        

        void SetRotation(SeamothArm arm, bool isDocked);

        bool OnUseDown(out float cooldownDuration);
        
        bool OnUseHeld(out float cooldownDuration);
        
        bool OnUseUp(out float cooldownDuration);
        
        bool OnAltDown();
        
        void Update(ref Quaternion aimDirection);
        
        void Reset();

        float GetEnergyCost();
    }
}