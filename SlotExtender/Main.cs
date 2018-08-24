using Harmony;
using System;
using System.Reflection;
using UnityEngine;

namespace SlotExtender
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                HarmonyInstance.Create("Subnautica.SlotExtender.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }    

    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class SeaMoth_Awake_Patch
    {
        internal static void Postfix(SeaMoth __instance)
        {           
            if (__instance.GetComponent<SlotExtender>() == null)
            {
                __instance.gameObject.AddComponent<SlotExtender>();
                Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");                
            }            
        }
    }
    
}
