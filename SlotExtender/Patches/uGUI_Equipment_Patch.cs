using UnityEngine;
using HarmonyLib;
using Common;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(uGUI_Equipment), "Awake")]    
    public static class uGUI_Equipment_Awake_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(uGUI_Equipment __instance)
        {
            if (Main.uGUI_PrefixComplete)
                return;

            Transform transform = __instance.gameObject.transform;

            void _setSlotPos(GameObject slot, Vector2 pos)
            {
                slot.GetComponent<uGUI_EquipmentSlot>().rectTransform.anchoredPosition = pos;
            }

            void _processSlot(SlotData slotData, GameObject normal, GameObject ArmLeft, GameObject ArmRight)
            {
                switch (slotData.SlotType)
                {
                    case SlotType.CloneChip:
                        _processCloneSlot(slotData, normal);
                        break;
                    case SlotType.OriginalNormal:
                    case SlotType.OriginalArmLeft:
                    case SlotType.OriginalArmRight:
                        _processOriginalSlot(slotData);
                        break;                   

                    case SlotType.CloneNormal:
                        _processCloneSlot(slotData, normal);
                        break;

                    case SlotType.CloneArmLeft:
                        _processCloneSlot(slotData, ArmLeft);
                        break;

                    case SlotType.CloneArmRight:
                        _processCloneSlot(slotData, ArmRight);
                        break;
                }
            }

            void _processOriginalSlot(SlotData slotData)
            {
                GameObject originalSlot = transform.Find(slotData.SlotID).gameObject;

                _setSlotPos(originalSlot, slotData.SlotPos);                
            }

            void _processCloneSlot(SlotData slotData, GameObject prefab)
            {
                GameObject temp_slot = Object.Instantiate(prefab, transform, false);

                temp_slot.name = slotData.SlotID;

                _setSlotPos(temp_slot, slotData.SlotPos);

                temp_slot.GetComponent<uGUI_EquipmentSlot>().slot = slotData.SlotID;           
            }

            // initializing GameObject variables for cloning
            GameObject NormalModuleSlot = transform.Find("SeamothModule2").gameObject;
            GameObject ArmLeftSlot = transform.Find("ExosuitArmLeft").gameObject;
            GameObject ArmRightSlot = transform.Find("ExosuitArmRight").gameObject;
            GameObject ChipSlot = transform.Find("Chip1").gameObject;

            // processing player chip slots            
            SlotHelper.NewChipSlots.ForEach(slotData => _processSlot(slotData, ChipSlot, null, null));

            // processing seamoth slots            
            SlotHelper.SessionSeamothSlots.ForEach(slotData => _processSlot(slotData, NormalModuleSlot, ArmLeftSlot, ArmRightSlot));

            // repositioning Seamoth background picture
            transform.Find("SeamothModule1/Seamoth").localPosition = SlotHelper.VehicleImgPos;

            // processing exosuit slots
            SlotHelper.SessionExosuitSlots.ForEach(slotData => _processSlot(slotData, NormalModuleSlot, ArmLeftSlot, ArmRightSlot));

            // repositioning Exosuit background picture
            transform.Find("ExosuitModule1/Exosuit").localPosition = SlotHelper.VehicleImgPos;            

            Main.uGUI_PrefixComplete = true;

            SNLogger.Log("uGUI_Equipment Slots Patched!");
        }

        
        [HarmonyPostfix]
        public static void Postfix(ref uGUI_Equipment __instance)
        {
            if (Main.uGUI_PostfixComplete)
                return;

            __instance.gameObject.EnsureComponent<uGUI_SlotTextHandler>();

            Main.uGUI_PostfixComplete = true;
        }
        
    }
}
