using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Reflection;
using System.IO;
using SeamothArms.ArmPrefabs;
using Common.Helpers;
using SeamothArms.ArmControls;

namespace SeamothArms
{
    public class SeamothArms_Graphics
    {
        public GameObject GraphicsRoot { get; private set; }
        public ObjectHelper objectHelper { get; private set; }

        public Exosuit ExosuitResource { get; private set; }
        
        //public TorpedoType[] TorpedoTypes { get; private set; }            

        private readonly Dictionary<ArmTemplate, GameObject> ArmsCache = new Dictionary<ArmTemplate, GameObject>();        

        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public SeamothArms_Graphics()
        {
            GraphicsRoot = new GameObject("SeamothArms_Graphics");

            GraphicsRoot.AddComponent<Indestructible>();

            objectHelper = new ObjectHelper();            

            ExosuitResource = Resources.Load<GameObject>("worldentities/tools/exosuit").GetComponent<Exosuit>();

            if (ExosuitResource == null)
            {
                SNLogger.Error("SeamothArms", "Failed to load resource: [Exosuit]");
            }
            else
            {
                SNLogger.Log("SeamothArms", "Resource loaded: [Exosuit]");
            }            

            InitializeArmsGraphics();

            SNLogger.Log("SeamothArms", $"Graphics cache initialization complete. GameObject: {GraphicsRoot.name}, ID: {GraphicsRoot.GetInstanceID()}");

            Main.isGraphicsReady = true;
        }        

        public ISeamothArm SpawnArm(TechType techType, Transform parent)
        {
            GameObject arm = Object.Instantiate(GetSeamothArmPrefab(techType));
            arm.transform.parent = parent.transform;
            arm.transform.localRotation = Quaternion.identity;
            arm.transform.localPosition = Vector3.zero;
            arm.SetActive(true);
            return arm.GetComponent<ISeamothArm>();
        }

        private GameObject GetSeamothArmPrefab(TechType techType)
        {
            if (techType == SeamothDrillArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.DrillArm];
            }
            else if (techType == SeamothClawArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.ClawArm];
            }
            else if (techType == SeamothGrapplingArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.GrapplingArm];
            }
            else if (techType == SeamothPropulsionArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.PropulsionArm];
            }
            else if (techType == SeamothTorpedoArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.TorpedoArm];
            }

            return null;
        }
               
        private void InitializeArmsGraphics()
        {
            //TorpedoTypes = (TorpedoType[])ExosuitResource.torpedoTypes.Clone();

            for (int i = 0; i < ExosuitResource.armPrefabs.Length; i++)
            {
                GameObject prefab = objectHelper.GetPrefabClone(ExosuitResource.armPrefabs[i].prefab, GraphicsRoot.transform, false);

                prefab.FindChild("exosuit_01_armRight").name = "seamoth_armRight";                

                switch (ExosuitResource.armPrefabs[i].techType)
                {
                    case TechType.ExosuitDrillArmModule:

                        prefab.name = "SeamothDrillArm";

                        Object.DestroyImmediate(prefab.GetComponent<ExosuitDrillArm>());

                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_arm_torpedoLauncher_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_propulsion_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingHook"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingBase"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "propulsion"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "object"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "torpedoLauncher"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "wrist"));

                        GameObject drillGeo = objectHelper.FindDeepChild(prefab, "exosuit_drill_geo");
                        drillGeo.name = "ArmModel";                        

                        prefab.AddComponent<SeamothDrillArmControl>();
                        ArmsCache.Add(ArmTemplate.DrillArm, prefab);
                        break;

                    case TechType.ExosuitClawArmModule:
                        prefab.name = "SeamothClawArm";
                        Object.DestroyImmediate(prefab.GetComponent<ExosuitClawArm>());
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_arm_torpedoLauncher_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_drill_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_propulsion_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingHook"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingBase"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "propulsion"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "object"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "torpedoLauncher"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "drill"));

                        GameObject handGeo = objectHelper.FindDeepChild(prefab, "exosuit_hand_geo");
                        handGeo.name = "ArmModel";                        

                        prefab.AddComponent<SeamothClawArmControl>();
                        ArmsCache.Add(ArmTemplate.ClawArm, prefab);
                        break;

                    case TechType.ExosuitGrapplingArmModule:

                        prefab.name = "SeamothGrapplingArm";

                        GameObject grapplingBase = objectHelper.FindDeepChild(prefab, "grapplingBase");
                        GameObject hook = objectHelper.FindDeepChild(prefab, "hook");

                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_arm_torpedoLauncher_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_drill_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_propulsion_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "drill"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "object"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "propulsion"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "torpedoLauncher"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "wrist"));

                        GameObject grapplingGeo = objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_geo");
                        grapplingGeo.name = "ArmModel";

                        prefab.AddComponent<SeamothGrapplingArmControl>();

                        ArmsCache.Add(ArmTemplate.GrapplingArm, prefab);
                        break;

                    case TechType.ExosuitPropulsionArmModule:

                        prefab.name = "SeamothPropulsionArm";

                        Object.DestroyImmediate(prefab.GetComponent<ExosuitPropulsionArm>());

                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_arm_torpedoLauncher_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_drill_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingHook"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingBase"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "wrist"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "object"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "torpedoLauncher"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "drill"));

                        GameObject propulsionGeo = objectHelper.FindDeepChild(prefab, "exosuit_propulsion_geo");
                        propulsionGeo.name = "ArmModel";

                        prefab.AddComponent<SeamothPropulsionArmControl>();

                        ArmsCache.Add(ArmTemplate.PropulsionArm, prefab);
                        break;

                    case TechType.ExosuitTorpedoArmModule:

                        prefab.name = "SeamothTorpedoArm";

                        Object.DestroyImmediate(prefab.GetComponent<ExosuitTorpedoArm>());

                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_propulsion_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_drill_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_grapplingHook_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "exosuit_hand_geo"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingHook"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "grapplingBase"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "wrist"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "object"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "propulsion"));
                        Object.DestroyImmediate(objectHelper.FindDeepChild(prefab, "drill"));

                        GameObject torpedoLauncherGeo = objectHelper.FindDeepChild(prefab, "exosuit_arm_torpedoLauncher_geo");
                        torpedoLauncherGeo.name = "ArmModel";

                        prefab.AddComponent<SeamothTorpedoArmControl>();

                        ArmsCache.Add(ArmTemplate.TorpedoArm, prefab);
                        break;
                }
            }

            SNLogger.Log("SeamothArms", "Arms graphics cache initialized.");
        }
    }
}
