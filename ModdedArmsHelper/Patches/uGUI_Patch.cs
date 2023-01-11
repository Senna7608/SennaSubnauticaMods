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

            if (Main.cacheRoot != null)
            {
                Object.DestroyImmediate(Main.cacheRoot);
            }

            Main.cacheRoot = new GameObject("CacheRoot");

            Main.cacheRoot.transform.SetParent(Main.helperRoot.transform, false);

            Main.cacheRoot.AddComponent<ArmsGraphics>();

            SNLogger.Debug("'CacheRoot' child gameObject created in 'ModdedArmsHelper'");            
        }
    }
}
