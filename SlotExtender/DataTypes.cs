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
        CloneArmRight,
        Chip,
        CloneChip
    };

    public enum SlotConfigID
    {
        Chip = -2,
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

    public enum SlotName
    {
        Chip1,
        Chip2,
        Chip3,
        Chip4,        

        ExosuitModule1,
        ExosuitModule2,
        ExosuitModule3,
        ExosuitModule4,
        ExosuitModule5,
        ExosuitModule6,
        ExosuitModule7,
        ExosuitModule8,
        ExosuitModule9,
        ExosuitModule10,
        ExosuitModule11,
        ExosuitModule12,
        ExosuitArmLeft,
        ExosuitArmRight,

        SeamothModule1,
        SeamothModule2,
        SeamothModule3,
        SeamothModule4,
        SeamothModule5,
        SeamothModule6,
        SeamothModule7,
        SeamothModule8,
        SeamothModule9,
        SeamothModule10,
        SeamothModule11,
        SeamothModule12,
        SeamothArmLeft,
        SeamothArmRight,

        CyclopsModule1,
        CyclopsModule2,
        CyclopsModule3,
        CyclopsModule4,
        CyclopsModule5,
        CyclopsModule6,
        CyclopsModule7,
        CyclopsModule8,
        CyclopsModule9,
        CyclopsModule10,
        CyclopsModule11,
        CyclopsModule12,
        CyclopsModule13,
        CyclopsModule14,
    };

    public class SlotData
    {
        public string SlotID;
        public SlotConfigID SlotConfigID;        
        public Vector2 SlotPos;
        public SlotType SlotType;

        public string uGui_SlotName { get; set; }
        public string KeyCodeName { get; set; }        
        public string SlotConfigIDName => SlotConfigID.ToString();
        public KeyCode KeyCode => InputHelper.GetInputNameAsKeyCode(KeyCodeName);

        public SlotData(string slotID, SlotConfigID internalSlotID, Vector2 slotPOS, SlotType slotType, string uGUI_SlotName = "")
        {
            SlotID = slotID;
            SlotConfigID = internalSlotID;            
            SlotPos = slotPOS;
            SlotType = slotType;
            uGui_SlotName = uGUI_SlotName != "" ? uGUI_SlotName : slotID;            
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
