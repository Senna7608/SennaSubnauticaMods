using Common;
using HarmonyLib;
using ModdedArmsHelper;
using UnityEngine;

namespace ModdedArmsAPI.Patches
{
    [HarmonyPatch(typeof(uGUI_MainMenu))]
    [HarmonyPatch("Awake")]
    internal class uGUI_MainMenu_Awake_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(uGUI_MainMenu __instance)
        {
            if (Main.helperRoot != null)
            {
                Object.DestroyImmediate(Main.helperRoot);
            }

            Main.helperRoot = new GameObject("ModdedArmsHelper");

            Main.helperRoot.transform.SetParent(uGUI.main.transform, false);

            SNLogger.Debug("'ModdedArmsHelper' child gameObject created in 'uGUI.main'");

            Main.helperRoot.AddComponent<ArmRegistrationListener>();

            SNLogger.Debug("'RegistrationListener' attached to 'ModdedArmsHelper'");

            if (Main.graphicsRoot != null)
            {
                Object.DestroyImmediate(Main.graphicsRoot);
            }

            Main.graphicsRoot = new GameObject("GraphicsRoot");

            Main.graphicsRoot.transform.SetParent(Main.helperRoot.transform, false);

            Main.graphicsRoot.AddComponent<ArmsGraphics>();

            SNLogger.Debug("'GraphicsRoot' child gameObject created in 'ModdedArmsHelper'");
        }
    }
}
