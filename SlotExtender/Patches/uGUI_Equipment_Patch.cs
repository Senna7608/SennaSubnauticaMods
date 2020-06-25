using System.Collections.Generic;
using Common;
using Harmony;
using System.Reflection;
using UnityEngine;

namespace SlotExtender.Patches
{
    /*
    [HarmonyPatch(typeof(uGUI_Equipment))]
    [HarmonyPatch("Awake")]
    internal class uGUI_Equipment_Awake_Patch
    {
        [HarmonyPrefix]
        internal static void Prefix(uGUI_Equipment __instance)
        {
           __instance.gameObject.AddIfNeedComponent<Initialize_uGUI>();            
        }

        [HarmonyPostfix]
        internal static void Postfix(ref uGUI_Equipment __instance)
        {
            var allSlots = (Dictionary<string, uGUI_EquipmentSlot>)__instance.GetPrivateField("allSlots", BindingFlags.SetField);

            Initialize_uGUI.Instance.Add_uGUIslots(__instance, allSlots);            
        }
    }
    */

    [HarmonyPatch(typeof(uGUI_Equipment))]
    [HarmonyPatch("Awake")]
    public class uGUI_Equipment_Awake_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(uGUI_Equipment __instance)
        {
            GameObject Equipment = __instance.gameObject;

            GameObject SeamothModule1 = Equipment.FindChild("SeamothModule1");
            GameObject SeamothModule2 = Equipment.FindChild("SeamothModule2");
            GameObject ExosuitModule2 = Equipment.FindChild("ExosuitModule2");
            GameObject ExosuitArmLeft = Equipment.FindChild("ExosuitArmLeft");
            GameObject ExosuitArmRight = Equipment.FindChild("ExosuitArmRight");

            foreach (SlotData baseExoSlotData in SlotHelper.BaseExosuitSlotsData)
            {
                GameObject temp_slot = Equipment.FindChild(baseExoSlotData.SlotID);
                uGUI_EquipmentSlot temp_slot_uGUI = temp_slot.GetComponent<uGUI_EquipmentSlot>();
                temp_slot_uGUI.rectTransform.anchoredPosition = baseExoSlotData.SlotPOS;
            }

            foreach (SlotData newExoSlotData in SlotHelper.NewExosuitSlotsData)
            {
                GameObject temp_slot = Object.Instantiate(ExosuitModule2, Equipment.transform, false);
                temp_slot.name = newExoSlotData.SlotID;
                uGUI_EquipmentSlot temp_slot_uGUI = temp_slot.GetComponent<uGUI_EquipmentSlot>();
                temp_slot_uGUI.slot = newExoSlotData.SlotID;
                temp_slot_uGUI.rectTransform.anchoredPosition = newExoSlotData.SlotPOS;
            }

            uGUI_EquipmentSlot exosuitArmLeft = ExosuitArmLeft.GetComponent<uGUI_EquipmentSlot>();
            exosuitArmLeft.rectTransform.anchoredPosition = SlotHelper.NewSeamothArmSlotsData[0].SlotPOS;

            uGUI_EquipmentSlot exosuitArmRight = ExosuitArmRight.GetComponent<uGUI_EquipmentSlot>();
            exosuitArmRight.rectTransform.anchoredPosition = SlotHelper.NewSeamothArmSlotsData[1].SlotPOS;

            SeamothModule1.FindChild("Seamoth").transform.localPosition = SlotHelper.slotPos[11];

            foreach (SlotData baseSeamothSlotData in SlotHelper.BaseSeamothSlotsData)
            {
                GameObject temp_slot = Equipment.FindChild(baseSeamothSlotData.SlotID);
                uGUI_EquipmentSlot temp_slot_uGUI = temp_slot.GetComponent<uGUI_EquipmentSlot>();
                temp_slot_uGUI.rectTransform.anchoredPosition = baseSeamothSlotData.SlotPOS;
            }

            foreach (SlotData newSeamothSlotData in SlotHelper.NewSeamothSlotsData)
            {
                GameObject temp_slot = Object.Instantiate(SeamothModule2, Equipment.transform, false);
                temp_slot.name = newSeamothSlotData.SlotID;
                uGUI_EquipmentSlot temp_slot_uGUI = temp_slot.GetComponent<uGUI_EquipmentSlot>();
                temp_slot_uGUI.slot = newSeamothSlotData.SlotID;
                temp_slot_uGUI.rectTransform.anchoredPosition = newSeamothSlotData.SlotPOS;
            }

            foreach (SlotData armSlotData in SlotHelper.NewSeamothArmSlotsData)
            {
                GameObject temp_slot = Object.Instantiate(ExosuitArmLeft, Equipment.transform, false);
                temp_slot.name = armSlotData.SlotID;
                uGUI_EquipmentSlot temp_slot_uGUI = temp_slot.GetComponent<uGUI_EquipmentSlot>();
                temp_slot_uGUI.slot = armSlotData.SlotID;
                temp_slot_uGUI.rectTransform.anchoredPosition = armSlotData.SlotPOS;
            }

            SNLogger.Log("SlotExtender", "uGUI_Equipment Slots Patched!");
        }

        [HarmonyPostfix]
        public static void Postfix(ref uGUI_Equipment __instance)
        {
            __instance.gameObject.AddComponent<uGUI_SlotTextHandler>();
        }
    }
}
