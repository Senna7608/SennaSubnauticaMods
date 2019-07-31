using System;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace SeamothEnergyShield
{
    public static class Main
    {
        public static void Load()
        {
            try
            {                
                var seamothShield = new SeamothShieldPrefab();

                seamothShield.Patch();                

                HarmonyInstance.Create("Subnautica.SeamothEnergyShield.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }             
    }    
}
