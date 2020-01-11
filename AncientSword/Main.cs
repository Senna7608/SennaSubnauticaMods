using System;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace AncientSword
{
    public static class Main
    {
        public static void Load()
        {
            try
            { 
                var ancientSword = new SwordPrefab();

                ancientSword.Patch();

                HarmonyInstance.Create("Subnautica.AncientSword.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        [HarmonyPatch(typeof(PDAScanner))]
        [HarmonyPatch("Unlock")]
        public static class PDAScannerUnlockPatch
        {
            public static bool Prefix(PDAScanner.EntryData entryData)
            {
                if (entryData.key == TechType.PrecursorPrisonArtifact8)
                {
                    if (!KnownTech.Contains(SwordPrefab.TechTypeID))
                    {
                        KnownTech.Add(SwordPrefab.TechTypeID);
                        ErrorMessage.AddMessage("Added blueprint for Ancient Sword fabrication to database");
                    }
                }
                return true;
            }
        }        
    }
}
