using Common;
using Harmony;
using System;
using System.Reflection;
using UnityEngine;

namespace SNTestMOD
{
    /*
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Start")]
    public class Seamoth_Start_Patch
    {        
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {            
                      
        }
    }
    
        
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    public class Exosuit_Start_Patch
    {        
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance)
        {            
                      
        }
    }
    */
    /*
    [HarmonyPatch(typeof(BaseNuclearReactor))]
    [HarmonyPatch("Start")]
    public class BaseNuclearReactor_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(BaseNuclearReactor __instance)
        {
            __instance.gameObject.AddIfNeedComponent<Radiation_NuclearReactor>();

            Main.PrintLOG($"Radiation_NuclearReactor component added to gameobject: {__instance.gameObject.name}");
        }
    }

        */
    
    [HarmonyPatch(typeof(ResourceTracker))]
    [HarmonyPatch("Register")]    
    public class ResourceTracker_Register_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ResourceTracker __instance)
        {
            TechType techType = (TechType)__instance.GetPrivateField("techType");                        

            if (techType == TechType.UraniniteCrystal)
            {
                __instance.gameObject.AddIfNeedComponent<Radiation_UraniniteCrystal>();

                Main.PrintLOG($"Radiation_UraniniteCrystal component added to gameobject: {__instance.gameObject.name}");
            }            
            
        }
    }


    /*
[HarmonyPatch(typeof(Drillable))]
[HarmonyPatch("GetDominantResourceType")]
public class Drillable_GetDominantResourceType_Patch
{
    [HarmonyPostfix]
    public static void Postfix(Drillable __instance, ref TechType __result)
    {
        var component = __instance.gameObject.GetComponent<Radiate_DrillableUranium>();

        if (__result == TechType.UraniniteCrystal && component == null)
        {               
            __instance.gameObject.AddIfNeedComponent<Radiate_DrillableUranium>();

            Main.PrintLOG($"Radiation_DrillableUranium component added to gameobject: {__instance.gameObject.name}");
        }


    }
}
*/
    /*
    [HarmonyPatch(typeof(LeakingRadiation))]
    [HarmonyPatch("Start")]
    public class LeakingRadiation_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(LeakingRadiation __instance)
        {
            __instance.gameObject.AddIfNeedComponent<ScanLeakMain>();
        }
    }
    */


    [HarmonyPatch(typeof(Builder))]
    [HarmonyPatch("CreateGhost")]
    public class Builder_CreateGhost_Patch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            try
            {
                GameObject prefab = (GameObject)typeof(Builder).GetField("prefab", BindingFlags.NonPublic | BindingFlags.Static).GetValue(typeof(Builder));
                
                if (prefab.name.Equals("BaseMoonpool"))
                {
                    if (prefab.TryGetComponent(out Constructable constructable))
                    {
                        constructable.forceUpright = false;
                        constructable.rotationEnabled = true;
                    }
                }

                //Debug.Log($"[SNTestMOD] prefab.name = {prefab.name}");

            }
            catch
            {
                Debug.Log("[SNTestMOD] Builder CreateGhost patch failed to get prefab gameobject!");
            }

        }        
    }


    [HarmonyPatch(typeof(Builder))]
    [HarmonyPatch("Update")]
    public class Builder_Update_Patch
    {

        [HarmonyPostfix]
        public static void Postfix()
        {
            try
            {
                GameObject prefab = (GameObject)typeof(Builder).GetField("prefab", BindingFlags.NonPublic | BindingFlags.Static).GetValue(typeof(Builder));

                if (prefab.name.Equals("BaseMoonpool"))
                {
                    
                        Debug.Log($"[SNTestMOD] canPlace = {Builder.canPlace}");
                   


                }               

            }
            catch
            {
                Debug.Log("[SNTestMOD] Builder Update patch failed to get prefab gameobject!");
            }

        }

    }

    }
