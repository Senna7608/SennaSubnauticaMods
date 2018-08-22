using Harmony;
using System;
using System.Reflection;
using UnityEngine;

namespace SlotExtender
{
    class Main
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
       

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class SeaMoth_Awake_Patch
    {
        internal static void Postfix(SeaMoth __instance)
        {            
            if (__instance.GetComponent<SlotExtender>() == null)
            {  
                __instance.gameObject.AddComponent<SlotExtender>();                
            }            
        }
    }

    // Working only with one Seamoth! if two or more: call every upgradesInput in the hierarchy but the upper one active only
    /*
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Update")]
    public class SeaMoth_Update_Patch
    {
        public static void Postfix(SeaMoth __instance)
        {
            if (Player.main.inSeamoth)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    __instance.upgradesInput.OpenFromExternal();                    

                }               
            }

        }
    }
    */
}
