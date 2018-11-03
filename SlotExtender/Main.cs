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
            
            Config.Config.InitConfig();
        }        
    }    
}
