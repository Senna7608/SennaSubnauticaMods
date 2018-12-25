using System.Collections.Generic;
using Common;
using Harmony;
using System.Reflection;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(uGUI_Equipment))]
    [HarmonyPatch("Awake")]
    public class uGUI_Equipment_Awake_Patch
    {            
        public static void Prefix(uGUI_Equipment __instance)
        {
            var component = __instance.gameObject.AddOrGetComponent<Initialize_uGUI>();            
        }
        
        public static void Postfix(ref uGUI_Equipment __instance)
        {
            var allSlots = (Dictionary<string, uGUI_EquipmentSlot>)__instance.GetPrivateField("allSlots", BindingFlags.SetField);

            Initialize_uGUI.Instance.Add_uGUIslots(__instance, allSlots);            
        }
    }
}
