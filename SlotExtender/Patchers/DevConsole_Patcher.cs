using Common;
using Harmony;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(DevConsole))]
    [HarmonyPatch("SetState")]
    internal class DevConsole_SetState_Patch
    {
        [HarmonyPrefix]
        internal static void Prefix(DevConsole __instance, bool value)
        {
            if (Main.ListenerInstance.IsNotNull())
            {
                Main.ListenerInstance.onConsoleInputFieldActive.Update(value);
            }
        }
    }
}
