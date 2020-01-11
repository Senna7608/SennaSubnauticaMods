using UnityEngine;

namespace SeamothArms
{
    public interface ISeamothArm
    {        
        GameObject GetGameObject();               

        bool HasClaw();

        bool HasDrill();        

        void SetSide(Arm arm);        

        void SetRotation(Arm arm, bool isDocked);

        bool OnUseDown(out float cooldownDuration);
        
        bool OnUseHeld(out float cooldownDuration);
        
        bool OnUseUp(out float cooldownDuration);
        
        bool OnAltDown();
        
        void Update(ref Quaternion aimDirection);
        
        void Reset();

        float GetEnergyCost();
    }
}