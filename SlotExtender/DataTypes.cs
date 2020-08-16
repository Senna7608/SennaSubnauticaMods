using Common.Helpers;
using UnityEngine;

namespace SlotExtender
{
    public enum ModdedEquipmentType
    {
        SeamothArm = 100
    };

    public enum SlotLayout
    {
        Grid,
        Circle
    };

    public enum SlotType
    {
        OriginalNormal,
        OriginalArmLeft,
        OriginalArmRight,
        CloneNormal,
        CloneArmLeft,
        CloneArmRight
    };

    public enum SlotConfigID
    {
        ExosuitArmLeft = -1,
        ExosuitArmRight,        
        Slot_1,
        Slot_2,
        Slot_3,
        Slot_4,
        Slot_5,
        Slot_6,
        Slot_7,
        Slot_8,
        Slot_9,
        Slot_10,
        Slot_11,
        Slot_12,
        SeamothArmLeft,
        SeamothArmRight        
    };

    public class SlotData
    {
        public string SlotID;
        public SlotConfigID SlotConfigID;        
        public Vector2 SlotPos;
        public SlotType SlotType;        

        public string KeyCodeName { get; set; }        
        public string SlotConfigIDName => SlotConfigID.ToString();
        public KeyCode KeyCode => InputHelper.GetInputNameAsKeyCode(KeyCodeName);

        public SlotData(string slotID, SlotConfigID internalSlotID, Vector2 slotPOS, SlotType slotType)
        {
            SlotID = slotID;
            SlotConfigID = internalSlotID;            
            SlotPos = slotPOS;
            SlotType = slotType;            
        }   
    }

    public abstract class SlotPosLayout
    {
        public abstract Vector2 VehicleImgPos { get; }
        public abstract Vector2[] SlotPos { get; }
        public abstract Vector2[] ArmSlotPos { get; }

        public const float Unit = 200f;
        public const float RowStep = Unit * 2.2f / 3;        
    }
}
