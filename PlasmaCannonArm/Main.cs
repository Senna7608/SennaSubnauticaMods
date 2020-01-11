using System;
using UnityEngine;
using Harmony;
using System.Reflection;
using static Common.GameHelper;

namespace PlasmaCannonArm
{
    public static class Main
    {
        public static AssetBundle assetBundle;        
        public static Material cannon_material;
        public static Material plasma_Material;
        public static GameObject plasmaShootSFX;

        public static TechType techTypeID;

        public static void Load()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile("./QMods/PlasmaCannonArm/Assets/plasmasfx");

                plasmaShootSFX = assetBundle.LoadAsset<GameObject>("plasmaShootSFX");                               

                cannon_material = GetResourceMaterial("worldentities/doodads/precursor/precursorteleporter", "precursor_interior_teleporter_02_01", 0);
                
                var plasmaCannon = new PlasmaCannonArmPrefab();
                
                plasmaCannon.Patch();

                techTypeID = plasmaCannon.TechType;

                HarmonyInstance.Create("Subnautica.ExosuitPlasmaCannonArm.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static Material GetPlasmaMaterial()
        {
            GameObject powerTransmitter = UnityEngine.Object.Instantiate(CraftData.GetPrefabForTechType(TechType.PowerTransmitter));

            GameObject laserBeam = UnityEngine.Object.Instantiate(powerTransmitter.GetComponent<PowerFX>().vfxPrefab, null, false);

            UnityEngine.Object.Destroy(powerTransmitter);

            LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();

            Material plasma = UnityEngine.Object.Instantiate(lineRenderer.material) as Material;

            UnityEngine.Object.Destroy(laserBeam);

            return plasma;
        }        
    }    
    
}
