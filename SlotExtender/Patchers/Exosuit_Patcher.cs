﻿using Harmony;
using Common;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class Exosuit_slotIDs_Patch
    {
        [HarmonyPrefix]
        internal static bool Prefix(ref string[] __result)
        {
            //__result = SlotHelper.ExpandedExosuitSlotIDs;
            __result = SlotHelper.SessionExosuitSlotIDs;
            return false;
        }
    }    

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Awake")]
    internal class Exosuit_Awake_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(Exosuit __instance)
        {            
            __instance.gameObject.AddIfNeedComponent<SlotExtender>();
            Logger.Log($"Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");           
        }
    }
}
