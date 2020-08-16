using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Common.Helpers;
using static Common.Helpers.GraphicsHelper;
using QModManager.API.ModLoading;

namespace PlasmaCannonArm
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static ObjectHelper objectHelper = new ObjectHelper();

        public static AssetBundle assetBundle;        
        public static Material cannon_material;
        public static Material plasma_Material;
        public static GameObject plasmaShootSFX;

        [QModPatch]
        public static void Load()
        {
            try
            {
                new PlasmaCannonArmPrefab().Patch();

                assetBundle = AssetBundle.LoadFromFile($"{modFolder}/Assets/plasmasfx");

                plasmaShootSFX = assetBundle.LoadAsset<GameObject>("plasmaShootSFX");                               

                cannon_material = GetResourceMaterial("worldentities/doodads/precursor/precursorteleporter", "precursor_interior_teleporter_02_01", 0);                                

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.ExosuitPlasmaCannonArm.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static Material GetPlasmaMaterial()
        {
            GameObject powerTransmitter = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter);            

            GameObject laserBeam = UnityEngine.Object.Instantiate(powerTransmitter.GetComponent<PowerFX>().vfxPrefab, null, false);            

            LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();

            Material plasma = UnityEngine.Object.Instantiate(lineRenderer.material) as Material;

            UnityEngine.Object.Destroy(powerTransmitter);

            UnityEngine.Object.Destroy(laserBeam);

            return plasma;
        }        
    }    
    
}
