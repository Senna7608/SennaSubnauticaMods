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

            void _setSlotPos(GameObject slot, Vector2 pos)
            {
                slot.GetComponent<uGUI_EquipmentSlot>().rectTransform.anchoredPosition = pos;
            }

            void _processBaseSlot(SlotData slotData)
            {
                _setSlotPos(Equipment.FindChild(slotData.SlotID), slotData.SlotPOS);
            }

            void _processNewSlot(SlotData slotData, GameObject prefab)
            {
                GameObject temp_slot = Object.Instantiate(prefab, Equipment.transform, false);
                temp_slot.name = slotData.SlotID;

                _setSlotPos(temp_slot, slotData.SlotPOS);
                temp_slot.GetComponent<uGUI_EquipmentSlot>().slot = slotData.SlotID;
            }

            GameObject SeamothModule = Equipment.FindChild("SeamothModule2");
            GameObject ExosuitModule = Equipment.FindChild("ExosuitModule2");
            GameObject ExosuitArmLeft = Equipment.FindChild("ExosuitArmLeft");
            GameObject ExosuitArmRight = Equipment.FindChild("ExosuitArmRight");

            // processing exosuit slots
            SlotHelper.BaseExosuitSlotsData.ForEach(slotData => _processBaseSlot(slotData));
            SlotHelper.NewExosuitSlotsData.ForEach(slotData => _processNewSlot(slotData, ExosuitModule));

            _setSlotPos(ExosuitArmLeft, SlotHelper.NewSeamothArmSlotsData[0].SlotPOS);
            _setSlotPos(ExosuitArmRight, SlotHelper.NewSeamothArmSlotsData[1].SlotPOS);

            Equipment.transform.Find("ExosuitModule1/Exosuit").localPosition = SlotHelper.vehicleImgPos;

            // processing seamoth slots
            SlotHelper.BaseSeamothSlotsData.ForEach(slotData => _processBaseSlot(slotData));
            SlotHelper.NewSeamothSlotsData.ForEach(slotData => _processNewSlot(slotData, SeamothModule));

            _processNewSlot(SlotHelper.NewSeamothArmSlotsData[0], ExosuitArmLeft);
            _processNewSlot(SlotHelper.NewSeamothArmSlotsData[1], ExosuitArmRight);

            Equipment.transform.Find("SeamothModule1/Seamoth").localPosition = SlotHelper.vehicleImgPos;

            SNLogger.Log("SlotExtender", "uGUI_Equipment Slots Patched!");
        }

        [HarmonyPostfix]
        public static void Postfix(ref uGUI_Equipment __instance)
        {
            __instance.gameObject.AddComponent<uGUI_SlotTextHandler>();
        }
    }
}
