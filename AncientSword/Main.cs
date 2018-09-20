using Harmony;
using System;
using System.Reflection;
using UnityEngine;

namespace AncientSword
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                var ancientSword = new AncientSwordPrefab();
                ancientSword.Patch();

                HarmonyInstance.Create("Subnautica.AncientSword.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
