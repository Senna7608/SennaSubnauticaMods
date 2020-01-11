using System;
using System.Collections.Generic;
using SlotExtender.Configuration;
using Common;

namespace SlotExtender
{
    public enum NewEquipmentType
    {
        SeamothArm = 100
    };

    internal static class SlotHelper
    {
        private static bool SlotMappingExpanded = false;

        internal static string[] SessionSeamothSlotIDs { get; private set; }
        internal static string[] SessionExosuitSlotIDs { get; private set; }

        internal static readonly string[] ExpandedSeamothSlotIDs = new string[14]
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
            "SeamothModule10",
            "SeamothModule11",
            "SeamothModule12",
            "SeamothArmLeft",
            "SeamothArmRight"
        };

        internal static readonly string[] ExpandedExosuitSlotIDs = new string[14]
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
            "ExosuitModule8",
            "ExosuitModule9",
            "ExosuitModule10",
            "ExosuitModule11",
            "ExosuitModule12"
        };
        
        internal static IEnumerable<string> NewSeamothSlotIDs
        {
            get
            {
                for (int i = 4; i < SEConfig.MAXSLOTS + 2; i++)
                {
                    if (i == SEConfig.MAXSLOTS)
                    {
                        yield return ExpandedSeamothSlotIDs[12];
                    }
                    else if (i == SEConfig.MAXSLOTS + 1)
                    {
                        yield return ExpandedSeamothSlotIDs[13];
                    }
                    else
                    {
                        yield return ExpandedSeamothSlotIDs[i];
                    }
                }
            }
        }        

        internal static IEnumerable<string> NewExosuitSlotIDs
        {
            get
            {
                for (int i = 6; i < SEConfig.MAXSLOTS + 2; i++)
                {
                    yield return ExpandedExosuitSlotIDs[i];
                }
            }
        }
        
        internal static void InitSlotIDs()
        {
            SessionSeamothSlotIDs = (string[])Array.CreateInstance(typeof(string), SEConfig.MAXSLOTS + 2);
            SessionExosuitSlotIDs = (string[])Array.CreateInstance(typeof(string), SEConfig.MAXSLOTS + 2);

            for (int i = 0; i < SEConfig.MAXSLOTS + 2; i++)
            {
                if (i == SEConfig.MAXSLOTS)
                {
                    SessionSeamothSlotIDs[i] = ExpandedSeamothSlotIDs[12];
                }
                else if (i == SEConfig.MAXSLOTS + 1)
                {
                    SessionSeamothSlotIDs[i] = ExpandedSeamothSlotIDs[13];
                }
                else
                {
                    SessionSeamothSlotIDs[i] = ExpandedSeamothSlotIDs[i];
                }
            }

            for (int i = 0; i < SEConfig.MAXSLOTS + 2; i++)
            {                
                SessionExosuitSlotIDs[i] = ExpandedExosuitSlotIDs[i];
            }
        }

        internal static void ExpandSlotMapping()
        {
            if (!SlotMappingExpanded)
            {
                foreach (string slotID in NewSeamothSlotIDs)
                {
                    if (slotID.Equals("SeamothArmLeft"))
                    {
                        Equipment.slotMapping.Add(slotID, (EquipmentType) NewEquipmentType.SeamothArm);
                    }
                    else if (slotID.Equals("SeamothArmRight"))
                    {
                        Equipment.slotMapping.Add(slotID, (EquipmentType) NewEquipmentType.SeamothArm);
                    }
                    else
                    {
                        Equipment.slotMapping.Add(slotID, EquipmentType.SeamothModule);
                    }
                }

                foreach (string slotID in NewExosuitSlotIDs)
                {
                    Equipment.slotMapping.Add(slotID, EquipmentType.ExosuitModule);                    
                }

                SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] Equipment slot mapping Patched!");
                SlotMappingExpanded = true;
            }
        }

        internal static bool IsExtendedSeamothSlot(string slotName)
        {
            foreach (string slot in NewSeamothSlotIDs)
            { 
                if (slotName.Equals(slot))
                    return true;
            }

            return false;
        }

        internal static bool IsSeamothArmSlot(string slotName)
        {            
            if (slotName.Equals("SeamothArmLeft") || slotName.Equals("SeamothArmRight"))
                return true;            

            return false;
        }        
    }
}
