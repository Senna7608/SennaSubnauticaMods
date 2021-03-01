using HarmonyLib;
using Common;

namespace ModdedArmsHelper.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class Seamoth_Awake_Patch
    {        
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {            
            __instance.gameObject.EnsureComponent<SeamothArmManager>();

            SNLogger.Log($"Seamoth Arm Manager added in Awake -> Postfix Patch, ID: {__instance.GetInstanceID()}");
        }
    }
    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnDockedChanged")]
    internal class SeaMoth_OnDockedChanged_Patch
    {        
        [HarmonyPostfix]
        internal static void Postfix(SeaMoth __instance, bool docked, Vehicle.DockType dockType)
        {
            if (__instance.gameObject.TryGetComponent(out SeamothArmManager manager))
            {
                manager.onDockedChanged?.Trigger(docked);
            }
            else
            {
                SNLogger.Warn("Seamoth Arm Manager is not ready!");
            }            
        }
    }
    

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftDown")]
    internal class Vehicle_SlotLeftDown_Patch
    {        
        [HarmonyPrefix]
        public static bool Prefix(Vehicle __instance)
        {
            if (__instance.GetType() == typeof(SeaMoth))
            {
                if (__instance.gameObject.TryGetComponent(out SeamothArmManager manager))
                {
                    if (manager.IsArmSlotSelected)
                    {
                        manager.SlotArmDown();
                        return false;
                    }
                }
                else
                {
                    SNLogger.Warn("Seamoth Arm Manager is not ready!");
                }                
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftHeld")]
    internal class Vehicle_SlotLeftHeld_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Vehicle __instance)
        {
            if (__instance.GetType() == typeof(SeaMoth))
            {
                if (__instance.gameObject.TryGetComponent(out SeamothArmManager manager))
                {
                    if (manager.IsArmSlotSelected)
                    {
                        manager.SlotArmHeld();
                        return false;
                    }
                }
                else
                {
                    SNLogger.Warn("Seamoth Arm Manager is not ready!");
                }               
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftUp")]
    internal class Vehicle_SlotLeftUp_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Vehicle __instance)
        {
            if (__instance.GetType() == typeof(SeaMoth))
            {
                if (__instance.gameObject.TryGetComponent(out SeamothArmManager manager))
                {
                    if (manager.IsArmSlotSelected)
                    {
                        manager.SlotArmUp();
                        return false;
                    }
                }
                else
                {
                    SNLogger.Warn("Seamoth Arm Manager is not ready!");
                }                
            }

            return true;
        }
    }
}
