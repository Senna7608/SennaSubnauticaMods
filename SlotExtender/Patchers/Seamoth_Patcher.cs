using Harmony;
using Common;
using UnityEngine;
using System.Reflection;

namespace SlotExtender.Patchers
{    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class Seamoth_slotIDs_Patch
    {
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.ExpandedSeamothSlotIDs;
            return false;
        }
    }    

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class SeaMoth_Awake_Patch
    {        
        internal static void Postfix(SeaMoth __instance)
        {
            __instance.gameObject.AddOrGetComponent<SlotExtender>();
            Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
        }
    }
}
