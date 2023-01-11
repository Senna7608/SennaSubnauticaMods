using System.Collections.Generic;
using SlotExtender.Configuration;
using Common;
using UnityEngine;
using System.Diagnostics;

namespace SlotExtender
{
    public static class SlotHelper
    {
        public static string[] SessionSeamothSlotIDs { get; private set; }
        public static string[] SessionExosuitSlotIDs { get; private set; }
        public static string[] SessionCyclopsSlotIDs { get; private set; }
        public static string[] NewChipSlotIDs { get; private set; }

        public static SlotPosLayout SessionSlotPosLayout = SEConfig.SLOT_LAYOUT == SlotLayout.Grid ? new SlotPosGrid() : (SlotPosLayout)new SlotPosCircle();

        public static Vector2[] SlotPos => SessionSlotPosLayout.SlotPos;
        public static Vector2[] ArmSlotPos => SessionSlotPosLayout.ArmSlotPos;
        public static Vector2 VehicleImgPos => SessionSlotPosLayout.VehicleImgPos;

        public static Vector2[] ChipSlotPos => new Vector2[2]
        {
            new Vector2(-136.5f, 184), // chip slot 3 - left
            new Vector2(136.5f, 184)  // chip slot 4 - right
        };

        public static Dictionary<string, SlotData> ALLSLOTS = new Dictionary<string, SlotData>();

        public static readonly Dictionary<SlotName, string> slotStringCache = new Dictionary<SlotName, string>()
        {
            { SlotName.Chip1,  "Chip1"  },
            { SlotName.Chip2,  "Chip2"  },
            { SlotName.Chip3,  "Chip3"  },
            { SlotName.Chip4,  "Chip4"  },

            { SlotName.ExosuitModule1,  "ExosuitModule1"  },
            { SlotName.ExosuitModule2,  "ExosuitModule2"  },
            { SlotName.ExosuitModule3,  "ExosuitModule3"  },
            { SlotName.ExosuitModule4,  "ExosuitModule4"  },
            { SlotName.ExosuitModule5,  "ExosuitModule5"  },
            { SlotName.ExosuitModule6,  "ExosuitModule6"  },
            { SlotName.ExosuitModule7,  "ExosuitModule7"  },
            { SlotName.ExosuitModule8,  "ExosuitModule8"  },
            { SlotName.ExosuitModule9,  "ExosuitModule9"  },
            { SlotName.ExosuitModule10, "ExosuitModule10" },
            { SlotName.ExosuitModule11, "ExosuitModule11" },
            { SlotName.ExosuitModule12, "ExosuitModule12" },
            { SlotName.ExosuitArmLeft,  "ExosuitArmLeft"  },
            { SlotName.ExosuitArmRight, "ExosuitArmRight" },

            { SlotName.SeamothModule1,  "SeamothModule1"  },
            { SlotName.SeamothModule2,  "SeamothModule2"  },
            { SlotName.SeamothModule3,  "SeamothModule3"  },
            { SlotName.SeamothModule4,  "SeamothModule4"  },
            { SlotName.SeamothModule5,  "SeamothModule5"  },
            { SlotName.SeamothModule6,  "SeamothModule6"  },
            { SlotName.SeamothModule7,  "SeamothModule7"  },
            { SlotName.SeamothModule8,  "SeamothModule8"  },
            { SlotName.SeamothModule9,  "SeamothModule9"  },
            { SlotName.SeamothModule10, "SeamothModule10" },
            { SlotName.SeamothModule11, "SeamothModule11" },
            { SlotName.SeamothModule12, "SeamothModule12" },
            { SlotName.SeamothArmLeft,  "SeamothArmLeft"  },
            { SlotName.SeamothArmRight, "SeamothArmRight" },

            { SlotName.CyclopsModule1,  "CyclopsModule1"  },
            { SlotName.CyclopsModule2,  "CyclopsModule2"  },
            { SlotName.CyclopsModule3,  "CyclopsModule3"  },
            { SlotName.CyclopsModule4,  "CyclopsModule4"  },
            { SlotName.CyclopsModule5,  "CyclopsModule5"  },
            { SlotName.CyclopsModule6,  "CyclopsModule6"  },
            { SlotName.CyclopsModule7,  "CyclopsModule7"  },
            { SlotName.CyclopsModule8,  "CyclopsModule8"  },
            { SlotName.CyclopsModule9,  "CyclopsModule9"  },
            { SlotName.CyclopsModule10, "CyclopsModule10" },
            { SlotName.CyclopsModule11, "CyclopsModule11" },
            { SlotName.CyclopsModule12, "CyclopsModule12" },
            { SlotName.CyclopsModule13, "CyclopsModule13" },
            { SlotName.CyclopsModule14, "CyclopsModule14" },
        };

        public static List<SlotData> NewChipSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.Chip3], SlotConfigID.Chip, ChipSlotPos[0], SlotType.CloneChip),
            new SlotData(slotStringCache[SlotName.Chip4], SlotConfigID.Chip, ChipSlotPos[1], SlotType.CloneChip),
        };

        public static List<SlotData> SessionSeamothSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.SeamothModule1], SlotConfigID.Slot_1, SlotPos[0], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule2], SlotConfigID.Slot_2, SlotPos[1], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule3], SlotConfigID.Slot_3, SlotPos[2], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule4], SlotConfigID.Slot_4, SlotPos[3], SlotType.OriginalNormal),
        };

        public static readonly List<SlotData> NewSeamothSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.SeamothModule5], SlotConfigID.Slot_5, SlotPos[4], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule6], SlotConfigID.Slot_6, SlotPos[5], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule7], SlotConfigID.Slot_7, SlotPos[6], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule8], SlotConfigID.Slot_8, SlotPos[7], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule9], SlotConfigID.Slot_9, SlotPos[8], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule10], SlotConfigID.Slot_10, SlotPos[9], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule11], SlotConfigID.Slot_11, SlotPos[10], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothModule12], SlotConfigID.Slot_12, SlotPos[11], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.SeamothArmLeft], SlotConfigID.SeamothArmLeft, ArmSlotPos[0], SlotType.CloneArmLeft),
            new SlotData(slotStringCache[SlotName.SeamothArmRight], SlotConfigID.SeamothArmRight, ArmSlotPos[1], SlotType.CloneArmRight)
        };

        public static List<SlotData> SessionExosuitSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.ExosuitArmLeft], SlotConfigID.ExosuitArmLeft, ArmSlotPos[0], SlotType.OriginalArmLeft),
            new SlotData(slotStringCache[SlotName.ExosuitArmRight], SlotConfigID.ExosuitArmRight, ArmSlotPos[1], SlotType.OriginalArmRight),
            new SlotData(slotStringCache[SlotName.ExosuitModule1], SlotConfigID.Slot_1, SlotPos[0], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule2], SlotConfigID.Slot_2, SlotPos[1], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule3], SlotConfigID.Slot_3, SlotPos[2], SlotType.OriginalNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule4], SlotConfigID.Slot_4, SlotPos[3], SlotType.OriginalNormal)
        };

        public static readonly List<SlotData> NewExosuitSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.ExosuitModule5], SlotConfigID.Slot_5, SlotPos[4], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule6], SlotConfigID.Slot_6, SlotPos[5], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule7], SlotConfigID.Slot_7, SlotPos[6], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule8], SlotConfigID.Slot_8, SlotPos[7], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule9], SlotConfigID.Slot_9, SlotPos[8], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule10], SlotConfigID.Slot_10, SlotPos[9], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule11], SlotConfigID.Slot_11, SlotPos[10], SlotType.CloneNormal),
            new SlotData(slotStringCache[SlotName.ExosuitModule12], SlotConfigID.Slot_12, SlotPos[11], SlotType.CloneNormal)
        };

        public static List<SlotData> SessionCyclopsSlots = new List<SlotData>()
        {
            new SlotData(slotStringCache[SlotName.CyclopsModule1], SlotConfigID.Slot_1, SlotPos[0], SlotType.OriginalNormal, "Module1"),
            new SlotData(slotStringCache[SlotName.CyclopsModule2], SlotConfigID.Slot_2, SlotPos[1], SlotType.OriginalNormal, "Module2"),
            new SlotData(slotStringCache[SlotName.CyclopsModule3], SlotConfigID.Slot_3, SlotPos[2], SlotType.OriginalNormal, "Module3"),
            new SlotData(slotStringCache[SlotName.CyclopsModule4], SlotConfigID.Slot_4, SlotPos[3], SlotType.OriginalNormal, "Module4"),
            new SlotData(slotStringCache[SlotName.CyclopsModule5], SlotConfigID.Slot_5, SlotPos[4], SlotType.OriginalNormal, "Module5"),
            new SlotData(slotStringCache[SlotName.CyclopsModule6], SlotConfigID.Slot_6, SlotPos[5], SlotType.OriginalNormal, "Module6"),
        };

        public static readonly List<SlotData> NewCyclopsSlots = new List<SlotData>()
        {            
            new SlotData(slotStringCache[SlotName.CyclopsModule7], SlotConfigID.Slot_7, SlotPos[6], SlotType.CloneNormal, "Module7"),
            new SlotData(slotStringCache[SlotName.CyclopsModule8], SlotConfigID.Slot_8, SlotPos[7], SlotType.CloneNormal, "Module8"),
            new SlotData(slotStringCache[SlotName.CyclopsModule9], SlotConfigID.Slot_9, ArmSlotPos[0], SlotType.CloneNormal, "Module9"),
            new SlotData(slotStringCache[SlotName.CyclopsModule10], SlotConfigID.Slot_10, ArmSlotPos[1], SlotType.CloneNormal, "Module10"),
            new SlotData(slotStringCache[SlotName.CyclopsModule11], SlotConfigID.Slot_11, SlotPos[8], SlotType.CloneNormal, "Module11"),
            new SlotData(slotStringCache[SlotName.CyclopsModule12], SlotConfigID.Slot_12, SlotPos[9], SlotType.CloneNormal, "Module12"),
            new SlotData(slotStringCache[SlotName.CyclopsModule13], SlotConfigID.SeamothArmLeft, SlotPos[10], SlotType.CloneNormal, "Module13"),
            new SlotData(slotStringCache[SlotName.CyclopsModule14], SlotConfigID.SeamothArmRight, SlotPos[11], SlotType.CloneNormal, "Module14")
        };

        public static IEnumerable<string> SessionNewSeamothSlotIDs
        {
            get
            {
                foreach (SlotData slotData in SessionSeamothSlots)
                {
                    if (slotData.SlotType != SlotType.OriginalNormal)
                    {
                        yield return slotData.SlotID;
                    }
                }
            }
        }

        public static IEnumerable<string> SessionNewExosuitSlotIDs
        {
            get
            {
                foreach (SlotData slotData in SessionExosuitSlots)
                {
                    if (slotData.SlotType == SlotType.CloneNormal)
                    {
                        yield return slotData.SlotID;
                    }
                }
            }
        }

        public static IEnumerable<string> SessionNewCyclopsSlotIDs
        {
            get
            {
                foreach (SlotData slotData in SessionCyclopsSlots)
                {
                    if (slotData.SlotType == SlotType.CloneNormal)
                    {
                        yield return slotData.uGui_SlotName;
                    }
                }
            }
        }

        public static void InitSlotIDs()
        {
            for (int i = 0; i < SEConfig.EXTRASLOTS; i++)
            {
                SessionSeamothSlots.Add(NewSeamothSlots[i]);
                SessionExosuitSlots.Add(NewExosuitSlots[i]);
                SessionCyclopsSlots.Add(NewCyclopsSlots[i]);
            }            

            if (SEConfig.isSeamothArmsExists)
            {
                foreach (SlotData slotData in NewSeamothSlots)
                {
                    SlotType slotType = slotData.SlotType;

                    if (slotType == SlotType.CloneArmLeft || slotType == SlotType.CloneArmRight)
                    {
                        SessionSeamothSlots.Add(slotData);
                    }
                }
            }

            NewChipSlotIDs = new string[NewChipSlots.Count];

            for (int i = 0; i < NewChipSlotIDs.Length; i++)
            {
                NewChipSlotIDs[i] = NewChipSlots[i].SlotID;
            }
            
            SessionSeamothSlotIDs = new string[SessionSeamothSlots.Count];

            for (int i = 0; i < SessionSeamothSlotIDs.Length; i++)
            {
                SessionSeamothSlotIDs[i] = SessionSeamothSlots[i].SlotID;
            }

            SessionExosuitSlotIDs = new string[SessionExosuitSlots.Count];

            for (int i = 0; i < SessionExosuitSlotIDs.Length; i++)
            {
                SessionExosuitSlotIDs[i] = SessionExosuitSlots[i].SlotID;
            }

            SessionCyclopsSlotIDs = new string[SessionCyclopsSlots.Count];

            for (int i = 0; i < SessionCyclopsSlotIDs.Length; i++)
            {
                SessionCyclopsSlotIDs[i] = SessionCyclopsSlots[i].SlotID;
            }
        }

        public static void ExpandSlotMapping()
        {
            foreach (SlotData slotData in NewChipSlots)
            {
                Equipment.slotMapping.Add(slotData.SlotID, EquipmentType.Chip);
            }

            foreach (SlotData slotData in SessionSeamothSlots)
            {
                switch (slotData.SlotType)
                {
                    case SlotType.CloneNormal:
                        Equipment.slotMapping.Add(slotData.SlotID, EquipmentType.SeamothModule);
                        break;

                    case SlotType.CloneArmLeft:
                    case SlotType.CloneArmRight:
                        Equipment.slotMapping.Add(slotData.SlotID, (EquipmentType)ModdedEquipmentType.SeamothArm);
                        break;
                }
            }

            foreach (SlotData slotData in SessionExosuitSlots)
            {
                if (slotData.SlotType == SlotType.CloneNormal)
                {
                    Equipment.slotMapping.Add(slotData.SlotID, EquipmentType.ExosuitModule);
                }
            }

            foreach (SlotData slotData in SessionCyclopsSlots)
            {
                if (slotData.SlotType == SlotType.CloneNormal)
                {
                    Equipment.slotMapping.Add(slotData.uGui_SlotName, EquipmentType.CyclopsModule);
                }
            }

            SNLogger.Log("Equipment slot mapping Patched!");

            DebugEquipmentSlotMapping();
        }

        public static bool IsExtendedSeamothSlot(string slotName)
        {             
            foreach (string slot in SessionNewSeamothSlotIDs)
            {
                if (slotName.Equals(slot))
                    return true;
            }           

            return false;
        }

        public static bool IsSeamothArmSlot(string slotName)
        {
            return slotName.Equals("SeamothArmLeft") || slotName.Equals("SeamothArmRight") ? true : false;
        }

        public static void InitSessionAllSlots()
        {
            ALLSLOTS.Clear();

            foreach (SlotData slotData in SessionSeamothSlots)
            {
                SEConfig.SLOTKEYBINDINGS.TryGetValue(slotData.SlotConfigID, out string result);
                slotData.KeyCodeName = result;
                ALLSLOTS.Add(slotData.SlotID, slotData);
            }

            foreach (SlotData slotData in SessionExosuitSlots)
            {
                SEConfig.SLOTKEYBINDINGS.TryGetValue(slotData.SlotConfigID, out string result);
                slotData.KeyCodeName = result;
                ALLSLOTS.Add(slotData.SlotID, slotData);
            }

            DebugAllSlots();
        }
        

        public static int GetSeamothSlotInt(SlotConfigID slotID)
        {
            for (int i = 0; i < SessionSeamothSlots.Count; i++)
            {
                if (SessionSeamothSlots[i].SlotConfigID == slotID)
                {
                    return i;
                }
            }

            return -1;
        }


        public static void ALLSLOTS_Update()
        {
            foreach (KeyValuePair<string, SlotData> kvp in ALLSLOTS)
            {
                SEConfig.SLOTKEYBINDINGS.TryGetValue(kvp.Value.SlotConfigID, out string result);
                kvp.Value.KeyCodeName = result;                
            }
        }

        [Conditional("TRACE")]
        private static void DebugAllSlots()
        {
            SNLogger.Debug("Listing Dictionary: ALLSLOTS...");

            foreach (KeyValuePair<string, SlotData> kvp in ALLSLOTS)
            {
                SNLogger.Debug(
                    $"SlotID: {kvp.Value.SlotID}" +
                    $", InternalSlotID: {kvp.Value.SlotConfigIDName}" +
                    $", SlotPOS: {kvp.Value.SlotPos}" +
                    $", SlotType: {kvp.Value.SlotType}" +
                    $", KeyCodeName: {kvp.Value.KeyCodeName}");
            }
        }

        [Conditional("TRACE")]
        private static void DebugEquipmentSlotMapping()
        {
            SNLogger.Debug("Listing Equipment SlotMapping...");

            foreach (KeyValuePair<string, EquipmentType> kvp in Equipment.slotMapping)
            {
                SNLogger.Debug($"Slot: {kvp.Key}, EquipmentType: {kvp.Value}");
            }
        }
    }
}
