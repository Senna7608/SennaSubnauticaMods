using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace RepairModule
{
    public static class Main
    {        
        public static void Load()
        {
            try
            {
                var repairModule = new RepairModulePrefab();
                repairModule.Patch();

                HarmonyInstance.Create("Subnautica.RepairModule.mod").PatchAll(Assembly.GetExecutingAssembly());                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }       
}
