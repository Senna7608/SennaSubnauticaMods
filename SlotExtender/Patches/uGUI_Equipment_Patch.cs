using Common;
using Harmony;
using UnityEngine;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(uGUI_Equipment), "Awake")]    
    public class uGUI_Equipment_Awake_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(uGUI_Equipment __instance)
        {
            GameObject Equipment = __instance.gameObject;

            void _setSlotPos(GameObject slot, Vector2 pos)
            {
                slot.GetComponent<uGUI_EquipmentSlot>().rectTransform.anchoredPosition = pos;
            }

            void _processSlot(SlotData slotData, GameObject normal, GameObject ArmLeft, GameObject ArmRight)
            {
                switch (slotData.SlotType)
                {
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
                GameObject originalSlot = Equipment.FindChild(slotData.SlotID);                

                _setSlotPos(originalSlot, slotData.SlotPos);                
            }

            void _processCloneSlot(SlotData slotData, GameObject prefab)
            {
                GameObject temp_slot = Object.Instantiate(prefab, Equipment.transform, false);

                temp_slot.name = slotData.SlotID;

                _setSlotPos(temp_slot, slotData.SlotPos);

                temp_slot.GetComponent<uGUI_EquipmentSlot>().slot = slotData.SlotID;           
            }

            // initializing GameObject variables for cloning
            GameObject NormalModuleSlot = Equipment.FindChild("SeamothModule2");            
            GameObject ArmLeftSlot = Equipment.FindChild("ExosuitArmLeft");
            GameObject ArmRightSlot = Equipment.FindChild("ExosuitArmRight");
            
            // repositioning Exosuit background picture
            Equipment.transform.Find("ExosuitModule1/Exosuit").localPosition = SlotHelper.VehicleImgPos;

            // processing exosuit slots
            SlotHelper.SessionExosuitSlots.ForEach(slotData => _processSlot(slotData, NormalModuleSlot, ArmLeftSlot, ArmRightSlot));

            // processing seamoth slots            
            SlotHelper.SessionSeamothSlots.ForEach(slotData => _processSlot(slotData, NormalModuleSlot, ArmLeftSlot, ArmRightSlot));            
                        
            // repositioning Seamoth background picture
            Equipment.transform.Find("SeamothModule1/Seamoth").localPosition = SlotHelper.VehicleImgPos;
            
            SNLogger.Log("SlotExtender", "uGUI_Equipment Slots Patched!");
        }

        
        [HarmonyPostfix]
        public static void Postfix(ref uGUI_Equipment __instance)
        {
            __instance.gameObject.AddComponent<uGUI_SlotTextHandler>();
        }
        
    }
}
