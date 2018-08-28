using UnityEngine;

namespace SlotExtender
{
    internal static class SlotHelper
    {
        private static bool SlotMappingExpanded = false;

        internal static readonly string[] ExpandedSeamothSlotIDs = new string[8]
        {
            "SeamothModule1",
            "SeamothModule2",
            "SeamothModule3",
            "SeamothModule4",
            "SeamothModule5",
            "SeamothModule6",
            "SeamothModule7",
            "SeamothModule8"
        };

        internal static readonly string[] ExpandedExosuitSlotIDs = new string[10]
        {
            "ExosuitArmLeft",
            "ExosuitArmRight",
            "ExosuitModule1",
            "ExosuitModule2",
            "ExosuitModule3",
            "ExosuitModule4",
            "ExosuitModule5",
            "ExosuitModule6",
            "ExosuitModule7",
            "ExosuitModule8"
        };
        
        internal static void ExpandSlotMapping()
        {
            if (!SlotMappingExpanded)
            {
                Equipment.slotMapping.Add("SeamothModule5", EquipmentType.SeamothModule);
                Equipment.slotMapping.Add("SeamothModule6", EquipmentType.SeamothModule);
                Equipment.slotMapping.Add("SeamothModule7", EquipmentType.SeamothModule);
                Equipment.slotMapping.Add("SeamothModule8", EquipmentType.SeamothModule);

                Equipment.slotMapping.Add("ExosuitModule5", EquipmentType.ExosuitModule);
                Equipment.slotMapping.Add("ExosuitModule6", EquipmentType.ExosuitModule);
                Equipment.slotMapping.Add("ExosuitModule7", EquipmentType.ExosuitModule);
                Equipment.slotMapping.Add("ExosuitModule8", EquipmentType.ExosuitModule);               
                
                Debug.Log("[SlotExtender] Equipment.slotMapping Patched!");
                SlotMappingExpanded = true;
            }
        }
    }
}
