using Common;
using HarmonyLib;

namespace SeamothArms.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    public class Seamoth_Awake_Patch
    {        
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {            
            __instance.gameObject.EnsureComponent<SeamothArmManager>();

            SNLogger.Log("SeamothArms", $"Seamoth Arm Manager added in Awake -> Postfix Patch, ID: {__instance.GetInstanceID()}");
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
                SNLogger.Warn("SeamothArms", "Seamoth Arm Manager is not ready!");
            }
                       
            /*
            SeamothArmManager control = null;

            try
            {
                control = __instance.gameObject.GetComponent<SeamothArmManager>().Instance;                
            }
            catch
            {
                SNLogger.Warn("SeamothArms", "Seamoth Arm Manager is not ready!");
                return;
            }
            
            if (control != null && control.onDockedChanged != null)
                control.onDockedChanged.Trigger(docked);
                
    */
        }
    }
    

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftDown")]
    public class Vehicle_SlotLeftDown_Patch
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
                    SNLogger.Warn("SeamothArms", "Seamoth Arm Manager is not ready!");
                }

                /*
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>();

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmDown();
                    return false;
                }
                */
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftHeld")]
    public class Vehicle_SlotLeftHeld_Patch
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
                    SNLogger.Warn("SeamothArms", "Seamoth Arm Manager is not ready!");
                }

                /*
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>();

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmHeld();
                    return false;
                }
                */
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("SlotLeftUp")]
    public class Vehicle_SlotLeftUp_Patch
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
                    SNLogger.Warn("SeamothArms", "Seamoth Arm Manager is not ready!");
                }

                /*
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>();

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmUp();
                    return false;
                }
                */
            }

            return true;
        }
    }
}
