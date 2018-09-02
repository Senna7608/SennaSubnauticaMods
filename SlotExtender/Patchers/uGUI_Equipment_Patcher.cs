using System.Collections.Generic;
using System.Reflection;
using Common;
using Harmony;

namespace SlotExtender.Patchers
{
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
}
