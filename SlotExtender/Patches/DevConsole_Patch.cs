using HarmonyLib;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(DevConsole), "SetState")]    
    internal class DevConsole_SetState_Patch
    {
        [HarmonyPrefix]
        internal static void Prefix(DevConsole __instance, bool value)
        {
            if (Main.ListenerInstance != null)
            {
                Main.ListenerInstance.onConsoleInputFieldActive.Update(value);
            }
        }
    }
}
