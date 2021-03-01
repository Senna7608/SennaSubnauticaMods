using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace RotateMoonPool
{
    [HarmonyPatch(typeof(Builder))]
    [HarmonyPatch("CreateGhost")]
    public class Builder_CreateGhost_Patch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            GameObject prefab = (GameObject)typeof(Builder).GetField("prefab", BindingFlags.NonPublic | BindingFlags.Static).GetValue(typeof(Builder));

            if (prefab.name.Equals("BaseMoonpool"))
            {
                if (prefab.TryGetComponent(out Constructable constructable))
                {
                    constructable.rotationEnabled = true;
                }
            }

        }
    }
}
