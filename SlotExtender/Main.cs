using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Common;

namespace SlotExtender
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                HarmonyInstance.Create("Subnautica.SlotExtender.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class SeaMoth_slotIDs_Patcher
    {
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.ExpandedSeamothSlotIDs;
            return false;
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class Exosuit_slotIDs_Patcher
    {
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.ExpandedExosuitSlotIDs;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class SeaMoth_Awake_Patch
    {       
        internal static void Postfix(SeaMoth __instance)
        {
            if (__instance.GetComponent<SlotExtender>() == null)
            {
                __instance.gameObject.AddComponent<SlotExtender>();

                Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Awake")]
    internal class Exosuit_Awake_Patch
    {
        internal static void Postfix(Exosuit __instance)
        {
            if (__instance.GetComponent<SlotExtender>() == null)
            {
                __instance.gameObject.AddComponent<SlotExtender>();

                Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
            }
        }
    }
          
    [HarmonyPatch(typeof(uGUI_Equipment))]
    [HarmonyPatch("Awake")]
    internal class uGUI_Equipment_Awake_Patch
    {
        internal static void Prefix(uGUI_Equipment __instance)
        {
            __instance.gameObject.AddComponent<Initialize_uGUI>();
        }

        internal static void Postfix(ref uGUI_Equipment __instance)
        {            
            var allSlots = (Dictionary<string, uGUI_EquipmentSlot>)__instance.GetPrivateField("allSlots", BindingFlags.NonPublic | BindingFlags.Instance);
            
            Initialize_uGUI.Instance.Add_uGUIslots(__instance, allSlots);
        }
    }
    
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("GetSlotType")]
    internal class Equipment_GetSlotType_Patcher
    {
        internal static void Prefix(string slot, ref EquipmentType __result)
        {
            SlotHelper.ExpandSlotMapping();            
        }
    }

    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("AllowedToAdd")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(Pickupable), typeof(bool) })]
    internal class Equipment_AllowedToAdd_Patch
    {
        internal static bool Prefix(Equipment __instance, string slot, Pickupable pickupable, bool verbose, ref bool __result)
        {
            if (pickupable.GetTechType() == TechType.VehicleStorageModule &&
                SlotExtender.IsExtendedSeamothSlot(slot))
            {
                // Do not allow storage modules in extended slots
                __result = false;
                return false;
            }

            return true;
        }
    }
}
