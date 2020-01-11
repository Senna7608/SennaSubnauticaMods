using Common;
using Harmony;
using UnityEngine;

namespace MAC.Fahrenheit
{
    [HarmonyPatch(typeof(uGUI_SeamothHUD))]
    [HarmonyPatch("Update")]
    public class uGUI_SeamothHUD_Update_Patch
    {
        private static Color textColor = new Color32(255, 220, 0, 255);

        [HarmonyPostfix]
        public static void Postfix(uGUI_SeamothHUD __instance)
        {
            Player main = Player.main;

            if (main != null && main.inSeamoth)
            {
                int celsius = (int)__instance.GetPrivateField("lastTemperature");
                int fahrenheit = Mathf.CeilToInt(celsius * 1.8f + 32);
                __instance.textTemperature.text = IntStringCache.GetStringForInt(fahrenheit);
                __instance.textTemperatureSuffix.color = textColor;
                __instance.textTemperatureSuffix.text = "\u00b0F";
            }
        }
    }

    [HarmonyPatch(typeof(uGUI_ExosuitHUD))]
    [HarmonyPatch("Update")]
    public class uGUI_ExosuitHUD_Update_Patch
    {
        private static Color textColor = new Color32(255, 220, 0, 255);

        [HarmonyPostfix]
        public static void Postfix(uGUI_ExosuitHUD __instance)
        {
            Player main = Player.main;

            if (main != null && main.inExosuit)
            {
                int celsius = (int)__instance.GetPrivateField("lastTemperature");
                int fahrenheit = Mathf.CeilToInt(celsius * 1.8f + 32);
                __instance.textTemperature.text = IntStringCache.GetStringForInt(fahrenheit);
                __instance.textTemperatureSuffix.color = textColor;
                __instance.textTemperatureSuffix.text = "\u00b0F";
            }
        }
    }

    [HarmonyPatch(typeof(ThermalPlant))]
    [HarmonyPatch("UpdateUI")]
    public class ThermalPlant_UpdateUI_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ThermalPlant __instance)
        {            
            int fahrenheit = Mathf.CeilToInt(__instance.temperature * 1.8f + 32);
            __instance.temperatureText.text = IntStringCache.GetStringForInt(fahrenheit);                       
        }
    }        
}

