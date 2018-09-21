using System.Collections.Generic;
using UnityEngine;

namespace SlotExtender
{
    internal static class SlotHelper
    {
        private static bool SlotMappingExpanded = false;

        internal static readonly string[] ExpandedSeamothSlotIDs = new string[9]
        {
            "SeamothModule1",
            "SeamothModule2",
            "SeamothModule3",
            "SeamothModule4",
            // New slots start here
            "SeamothModule5",
            "SeamothModule6",
            "SeamothModule7",
            "SeamothModule8",
            "SeamothModule9",
        };

        internal static readonly string[] ExpandedExosuitSlotIDs = new string[10]
        {
            "ExosuitArmLeft",
            "ExosuitArmRight",
            "ExosuitModule1",
            "ExosuitModule2",
            "ExosuitModule3",
            "ExosuitModule4",
            // New slots start here
            "ExosuitModule5",
            "ExosuitModule6",
            "ExosuitModule7",
            "ExosuitModule8"
        };

        internal static IEnumerable<string> NewSeamothSlotIDs
        {
            get
            {
                for (int i = 4; i < ExpandedSeamothSlotIDs.Length; i++)
                {
                    yield return ExpandedSeamothSlotIDs[i];
                }
            }
        }

        internal static IEnumerable<string> NewExosuitSlotIDs
        {
            get
            {
                for (int i = 6; i < ExpandedExosuitSlotIDs.Length; i++)
                {
                    yield return ExpandedExosuitSlotIDs[i];
                }
            }
        }

        internal static void ExpandSlotMapping()
        {
            if (!SlotMappingExpanded)
            {
                foreach (string slotID in NewSeamothSlotIDs)
                    Equipment.slotMapping.Add(slotID, EquipmentType.SeamothModule);

                foreach (string slotID in NewExosuitSlotIDs)
                    Equipment.slotMapping.Add(slotID, EquipmentType.ExosuitModule);

                Debug.Log("[SlotExtender] Equipment.slotMapping Patched!");
                SlotMappingExpanded = true;
            }
        }
    }
}
