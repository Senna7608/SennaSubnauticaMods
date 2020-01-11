using Common;
using Harmony;

namespace SeamothArms.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    public class Seamoth_Awake_Patch
    {        
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {            
            __instance.gameObject.AddIfNeedComponent<SeamothArmManager>();

            SNLogger.Log($"[SeamothArms] Seamoth Arm Manager added in Awake -> Postfix Patch, ID: {__instance.GetInstanceID()}");
        }
    }
    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnDockedChanged")]
    internal class SeaMoth_OnDockedChanged_Patch
    {        
        [HarmonyPostfix]
        internal static void Postfix(SeaMoth __instance, bool docked, Vehicle.DockType dockType)
        {
            SeamothArmManager control = null;

            try
            {
                control = __instance.gameObject.GetComponent<SeamothArmManager>().Instance;                
            }
            catch
            {
                SNLogger.Log("[SeamothArms] OnDockedChanged Patch Warning! Seamoth Arm Manager is not ready!");
                return;
            }
            
            if (control != null && control.onDockedChanged != null)
                control.onDockedChanged.Trigger(docked);                        
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
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>().Instance;

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmDown();
                    return false;
                }                
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
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>().Instance;

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmHeld();
                    return false;
                }                
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
                SeamothArmManager control = __instance.gameObject.GetComponent<SeamothArmManager>().Instance;

                if (control.IsArmSlotSelected)
                {
                    control.SlotArmUp();
                    return false;
                }                
            }

            return true;
        }
    }
}
