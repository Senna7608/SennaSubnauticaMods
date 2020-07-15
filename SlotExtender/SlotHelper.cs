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

        public static SlotPosLayout SessionSlotPosLayout = SEConfig.SLOT_LAYOUT == SlotLayout.Grid ? new SlotPosGrid() : (SlotPosLayout)new SlotPosCircle();

        public static Vector2[] SlotPos => SessionSlotPosLayout.SlotPos;
        public static Vector2[] ArmSlotPos => SessionSlotPosLayout.ArmSlotPos;
        public static Vector2 VehicleImgPos => SessionSlotPosLayout.VehicleImgPos;

        public static Dictionary<string, SlotData> ALLSLOTS = new Dictionary<string, SlotData>();

        public static List<SlotData> SessionSeamothSlots = new List<SlotData>()
        {
            new SlotData("SeamothModule1", SlotConfigID.Slot_1, SlotPos[0], SlotType.OriginalNormal),
            new SlotData("SeamothModule2", SlotConfigID.Slot_2, SlotPos[1], SlotType.OriginalNormal),
            new SlotData("SeamothModule3", SlotConfigID.Slot_3, SlotPos[2], SlotType.OriginalNormal),
            new SlotData("SeamothModule4", SlotConfigID.Slot_4, SlotPos[3], SlotType.OriginalNormal),
        };

        public static readonly List<SlotData> NewSeamothSlots = new List<SlotData>()
        {
            new SlotData("SeamothModule5", SlotConfigID.Slot_5, SlotPos[4], SlotType.CloneNormal),
            new SlotData("SeamothModule6", SlotConfigID.Slot_6, SlotPos[5], SlotType.CloneNormal),
            new SlotData("SeamothModule7", SlotConfigID.Slot_7, SlotPos[6], SlotType.CloneNormal),
            new SlotData("SeamothModule8", SlotConfigID.Slot_8, SlotPos[7], SlotType.CloneNormal),
            new SlotData("SeamothModule9", SlotConfigID.Slot_9, SlotPos[8], SlotType.CloneNormal),
            new SlotData("SeamothModule10", SlotConfigID.Slot_10, SlotPos[9], SlotType.CloneNormal),
            new SlotData("SeamothModule11", SlotConfigID.Slot_11, SlotPos[10], SlotType.CloneNormal),
            new SlotData("SeamothModule12", SlotConfigID.Slot_12, SlotPos[11], SlotType.CloneNormal),
            new SlotData("SeamothArmLeft", SlotConfigID.SeamothArmLeft, ArmSlotPos[0], SlotType.CloneArmLeft),
            new SlotData("SeamothArmRight", SlotConfigID.SeamothArmRight, ArmSlotPos[1], SlotType.CloneArmRight)
        };

        public static List<SlotData> SessionExosuitSlots = new List<SlotData>()
        {
            new SlotData("ExosuitArmLeft", SlotConfigID.ExosuitArmLeft, ArmSlotPos[0], SlotType.OriginalArmLeft),
            new SlotData("ExosuitArmRight", SlotConfigID.ExosuitArmRight, ArmSlotPos[1], SlotType.OriginalArmRight),
            new SlotData("ExosuitModule1", SlotConfigID.Slot_1, SlotPos[0], SlotType.OriginalNormal),
            new SlotData("ExosuitModule2", SlotConfigID.Slot_2, SlotPos[1], SlotType.OriginalNormal),
            new SlotData("ExosuitModule3", SlotConfigID.Slot_3, SlotPos[2], SlotType.OriginalNormal),
            new SlotData("ExosuitModule4", SlotConfigID.Slot_4, SlotPos[3], SlotType.OriginalNormal)
        };

        public static readonly List<SlotData> NewExosuitSlots = new List<SlotData>()
        {
            new SlotData("ExosuitModule5", SlotConfigID.Slot_5, SlotPos[4], SlotType.CloneNormal),
            new SlotData("ExosuitModule6", SlotConfigID.Slot_6, SlotPos[5], SlotType.CloneNormal),
            new SlotData("ExosuitModule7", SlotConfigID.Slot_7, SlotPos[6], SlotType.CloneNormal),
            new SlotData("ExosuitModule8", SlotConfigID.Slot_8, SlotPos[7], SlotType.CloneNormal),
            new SlotData("ExosuitModule9", SlotConfigID.Slot_9, SlotPos[8], SlotType.CloneNormal),
            new SlotData("ExosuitModule10", SlotConfigID.Slot_10, SlotPos[9], SlotType.CloneNormal),
            new SlotData("ExosuitModule11", SlotConfigID.Slot_11, SlotPos[10], SlotType.CloneNormal),
            new SlotData("ExosuitModule12", SlotConfigID.Slot_12, SlotPos[11], SlotType.CloneNormal)
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

        public static void InitSlotIDs()
        {
            SNLogger.Debug("SlotExtender", "Method call: SlotHelper.InitSlotIDs()");            

            for (int i = 0; i < SEConfig.EXTRASLOTS; i++)
            {
                SessionSeamothSlots.Add(NewSeamothSlots[i]);
                SessionExosuitSlots.Add(NewExosuitSlots[i]);
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
        }

        public static void ExpandSlotMapping()
        {
            SNLogger.Debug("SlotExtender", "Method call: SlotHelper.ExpandSlotMapping()");

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

            SNLogger.Log("SlotExtender", "Equipment slot mapping Patched!");
        }

        public static bool IsExtendedSeamothSlot(string slotName)
        {
            SNLogger.Debug("SlotExtender", $"Method call: SlotHelper.IsExtendedSeamothSlot({slotName})");

            foreach (string slot in SessionNewSeamothSlotIDs)
            {
                if (slotName.Equals(slot))
                    return true;
            }

            return false;
        }

        public static bool IsSeamothArmSlot(string slotName)
        {
            SNLogger.Debug("SlotExtender", $"Method call: SlotHelper.IsSeamothArmSlot({slotName})");

            return slotName.Equals("SeamothArmLeft") || slotName.Equals("SeamothArmRight") ? true : false;
        }

        public static void InitSessionAllSlots()
        {
            SNLogger.Debug("SlotExtender", $"Method call: SlotHelper.InitSessionAllSlots()");

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

        [Conditional("DEBUG")]
        private static void DebugAllSlots()
        {
            SNLogger.Debug("SlotExtender", "Listing Dictionary: ALLSLOTS...\n");

            foreach (KeyValuePair<string, SlotData> kvp in ALLSLOTS)
            {
                SNLogger.Log(
                    $"SlotID: {kvp.Value.SlotID}\n" +
                    $"InternalSlotID: {kvp.Value.SlotConfigIDName}\n" +
                    $"SlotPOS: {kvp.Value.SlotPos}\n" +
                    $"SlotType: {kvp.Value.SlotType}\n" +
                    $"KeyCodeName: {kvp.Value.KeyCodeName}\n");
            }
        }
    }
}
