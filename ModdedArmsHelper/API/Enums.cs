namespace ModdedArmsHelper.API
{
    public enum ArmType
    {
        ExosuitArm,
        SeamothArm
    }

    public enum ArmTemplate
    {
        ClawArm,
        DrillArm,
        GrapplingArm,
        PropulsionArm,
        TorpedoArm
    };

    public enum ArmBaseAbility
    {
        None,
        Claw,
        Drill,
        Propulsion,
        GrapplingHook,
        Torpedo
    }

    public enum SeamothArm
    {
        None,
        Left,
        Right
    }

    public enum TargetObjectType
    {
        None,
        Pickupable,
        Drillable
    }    
}
