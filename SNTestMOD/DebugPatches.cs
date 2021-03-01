using Common;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SNTestMOD
{
    /*
    [HarmonyPatch(typeof(LootDistributionData))]
    [HarmonyPatch("Initialize")]
    public class LootDistributionData_Initialize_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(LootDistributionData __instance)
        {
            foreach (KeyValuePair<string, LootDistributionData.SrcData> source in __instance.srcDistribution)
            {                
                SNLogger.Log($"\nprefabPath: {source.Value.prefabPath}");

                foreach (LootDistributionData.BiomeData biomeData in source.Value.distribution)
                {
                    SNLogger.Log($"biome: {biomeData.biome}, count: {biomeData.count}, probability: {biomeData.probability}");                    
                }
            }
        }
    }
    */
    
    /*
    [HarmonyPatch(typeof(PDAScanner))]
    [HarmonyPatch("Initialize")]        
    public class PDAScanner_Initialize_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            FieldInfo fieldInfo = AccessTools.Field(typeof(PDAScanner), "mapping");

            Dictionary<TechType, PDAScanner.EntryData> mapping = (Dictionary<TechType, PDAScanner.EntryData>)fieldInfo.GetValue(typeof(PDAScanner));

            foreach (KeyValuePair<TechType, PDAScanner.EntryData> entry in mapping)
            {
                SNLogger.Log("\nPDAScanner:");
                SNLogger.Log($"TechType: {entry.Key.ToString()}");

                SNLogger.Log($"blueprint: {entry.Value.blueprint}");
                SNLogger.Log($"destroyAfterScan: {entry.Value.destroyAfterScan}");
                SNLogger.Log($"encyclopedia: {entry.Value.encyclopedia}");
                SNLogger.Log($"isFragment: {entry.Value.isFragment}");
                SNLogger.Log($"locked: {entry.Value.locked}");
                SNLogger.Log($"scanTime: {entry.Value.scanTime}");
                SNLogger.Log($"totalFragments: {entry.Value.totalFragments}");                
            }
        }
    }    
    

    [HarmonyPatch(typeof(SpriteManager))]
    [HarmonyPatch("Get")]
    [HarmonyPatch(new Type[] { typeof(TechType) })]
    public class SpriteManager_Get_Patch
    {
        static bool isPatched = false;

        [HarmonyPostfix]
        public static void Postfix(TechType techType)
        {
            if (isPatched)
                return;            

            FieldInfo fieldInfo = AccessTools.Field(typeof(SpriteManager), "groups");

            Dictionary<SpriteManager.Group, Dictionary<string, Atlas.Sprite>> atlases = (Dictionary<SpriteManager.Group, Dictionary<string, Atlas.Sprite>>)fieldInfo.GetValue(typeof(SpriteManager));

            foreach (KeyValuePair<SpriteManager.Group, Dictionary<string, Atlas.Sprite>> atlas in atlases)
            {                
                SNLogger.Log($"atlas: {atlas.Key}");

                foreach (KeyValuePair<string, Atlas.Sprite> sprite in atlas.Value)
                {
                    SNLogger.Log($"sprite: {sprite.Key}");
                }
            }

            isPatched = true;
        }
    }
    
    
    [HarmonyPatch(typeof(PDAEncyclopedia))]
    [HarmonyPatch("Initialize")]
    public class PDAEncyclopedia_Initialize_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            FieldInfo fieldInfo = AccessTools.Field(typeof(PDAEncyclopedia), "mapping");

            Dictionary<string, PDAEncyclopedia.EntryData> entries = (Dictionary<string, PDAEncyclopedia.EntryData>)fieldInfo.GetValue(typeof(PDAEncyclopedia));

            foreach (KeyValuePair<string, PDAEncyclopedia.EntryData> entry in entries)
            {
                SNLogger.Log("\nPDAEncyclopedia:");
                SNLogger.Log($"key: {entry.Value.key}");
                SNLogger.Log($"path: {entry.Value.path}");

                foreach (string node in entry.Value.nodes)
                {
                    SNLogger.Log($"node: {node}");
                }
                
                SNLogger.Log($"unlocked: {entry.Value.unlocked}");
                SNLogger.Log($"popup: {entry.Value.popup}");
                SNLogger.Log($"image: {entry.Value.image}");
                SNLogger.Log($"sound: {entry.Value.sound}");
                SNLogger.Log($"audio: {entry.Value.audio}");                
            }
        }
    }    


    /*
    [HarmonyPatch(typeof(KnownTech))]
    [HarmonyPatch("Initialize")]
    public class KnownTech_Initialize_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            FieldInfo fieldInfo = AccessTools.Field(typeof(KnownTech), "analysisTech");

            List<KnownTech.AnalysisTech> analysisTech = (List<KnownTech.AnalysisTech>)fieldInfo.GetValue(typeof(KnownTech));

            foreach (KnownTech.AnalysisTech entry in analysisTech)
            {
                SNLogger.Log("\nKnownTech:");
                SNLogger.Log($"techType: {entry.techType.ToString()}");
                SNLogger.Log($"unlockMessage: {entry.unlockMessage}");

                try
                {
                    SNLogger.Log($"unlockSound: {entry.unlockSound.path}");
                }
                catch
                {
                    SNLogger.Log("unlockSound: null");
                    continue;
                }


                try
                {
                    SNLogger.Log($"unlockPopup: {entry.unlockPopup.name}");
                }
                catch
                {
                    SNLogger.Log("unlockPopup: null");
                    continue;
                }
            }
        }
    }
    */
}
