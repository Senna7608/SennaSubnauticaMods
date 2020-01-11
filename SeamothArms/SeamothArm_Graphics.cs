using System.Collections.Generic;
using UnityEngine;
using Common;
using SeamothArms.ArmControls;
using SeamothArms.ArmPrefabs;

namespace SeamothArms
{
    public partial class SeamothArmManager
    {
        private readonly Dictionary<ArmTemplate, GameObject> ArmsCache = new Dictionary<ArmTemplate, GameObject>();

        private readonly List<GameObject> GeoCache = new List<GameObject>();        

        private ISeamothArm SpawnArm(TechType techType, Transform parent)
        {
            GameObject arm = Instantiate(GetSeamomthArmPrefab(techType));
            arm.transform.parent = parent.transform;
            arm.transform.localRotation = Quaternion.identity;
            arm.transform.localPosition = Vector3.zero;
            arm.SetActive(true);
            return arm.GetComponent<ISeamothArm>();
        }

        private GameObject GetSeamomthArmPrefab(TechType techType)
        {
            if (techType == SeamothDrillArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.DrillArm];
            }
            else if (techType == SeamothClawArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.ClawArm];
            }
            else if(techType == SeamothGrapplingArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.GrapplingArm];
            }
            else if(techType == SeamothPropulsionArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.PropulsionArm];
            }
            else if (techType == SeamothTorpedoArmPrefab.TechTypeID)
            {
                return ArmsCache[ArmTemplate.TorpedoArm];
            }

            return null;
        }

        
        private void InitializeArmsCache()
        {
            var exosuit = Resources.Load<GameObject>("worldentities/tools/exosuit").GetComponent<Exosuit>();

            for (int i = 0; i < exosuit.armPrefabs.Length; i++)
            {
                GameObject prefab = exosuit.armPrefabs[i].prefab.GetPrefabClone(SeamothArmPrefabs.transform, false);
                
                prefab.FindChild("exosuit_01_armRight").name = "seamoth_armRight";

                /*
                GameObject bicepPivot = prefab.FindChildInMaxDepth("bicepPivot");
                GameObject bicepPivot_collider_container = GameHelper.CreateGameObject("collider", bicepPivot.transform);
                CapsuleCollider bicepPivot_collider = bicepPivot_collider_container.AddComponent<CapsuleCollider>();
                bicepPivot_collider.center = new Vector3(0.34f, 0f, 0f);
                bicepPivot_collider.radius = 0.16f;
                bicepPivot_collider.height = 1.15f;
                bicepPivot_collider.direction = 0;                
                */

                switch (exosuit.armPrefabs[i].techType)
                {
                    case TechType.ExosuitDrillArmModule:

                        prefab.name = "SeamothDrillArmPrefab";

                        DestroyImmediate(prefab.GetComponent<ExosuitDrillArm>());

                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_arm_torpedoLauncher_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_propulsion_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingHook"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingBase"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("propulsion"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("object"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("torpedoLauncher"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("wrist"));
                        
                        GameObject drillGeo = prefab.FindChildInMaxDepth("exosuit_drill_geo");
                        drillGeo.name = "seamoth_drill_geo";
                        GeoCache.Add(drillGeo);
                        
                        /*
                        GameObject drill = prefab.FindChildInMaxDepth("drill");
                        GameObject drill_collider_container = GameHelper.CreateGameObject("collider", drill.transform);                        
                        CapsuleCollider drill_collider = drill_collider_container.AddComponent<CapsuleCollider>();
                        drill_collider.direction = 0;
                        drill_collider.center = new Vector3(-0.18f, 0f, 0f);
                        drill_collider.radius = 0.20f;
                        drill_collider.height = 1.92f;
                        */

                        prefab.AddComponent<SeamothDrillArmControl>();
                        ArmsCache.Add(ArmTemplate.DrillArm, prefab);
                        break;

                    case TechType.ExosuitClawArmModule:
                        prefab.name = "SeamothClawArmPrefab";
                        DestroyImmediate(prefab.GetComponent<ExosuitClawArm>());
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_arm_torpedoLauncher_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_drill_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_propulsion_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingHook"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingBase"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("propulsion"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("object"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("torpedoLauncher"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("drill"));

                        GameObject handGeo = prefab.FindChildInMaxDepth("exosuit_hand_geo");
                        handGeo.name = "seamoth_hand_geo";
                        GeoCache.Add(handGeo);                        

                        /*
                        GameObject wrist = prefab.FindChildInMaxDepth("wrist");
                        GameObject wrist_collider_container = GameHelper.CreateGameObject("collider", wrist.transform);                        
                        CapsuleCollider wrist_collider = wrist_collider_container.AddComponent<CapsuleCollider>();
                        wrist_collider.direction = 0;
                        wrist_collider.center = new Vector3(0.68f, 0f, 0f);
                        wrist_collider.radius = 0.27f;
                        wrist_collider.height = 1.49f;
                        */

                        prefab.AddComponent<SeamothClawArmControl>();
                        ArmsCache.Add(ArmTemplate.ClawArm, prefab);
                        break;

                    case TechType.ExosuitGrapplingArmModule:

                        prefab.name = "SeamothGrapplingArmPrefab";

                        GameObject grapplingBase = prefab.FindChildInMaxDepth("grapplingBase");
                        GameObject hook = prefab.FindChildInMaxDepth("hook");

                        //ExosuitGrapplingArm component = prefab.GetComponent<ExosuitGrapplingArm>();

                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_arm_torpedoLauncher_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_drill_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_propulsion_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("drill"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("object"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("propulsion"));                                                
                        DestroyImmediate(prefab.FindChildInMaxDepth("torpedoLauncher"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("wrist"));

                        GameObject grapplingGeo = prefab.FindChildInMaxDepth("exosuit_grapplingHook_geo");
                        grapplingGeo.name = "semoth_grapplingHook_geo";
                        GeoCache.Add(grapplingGeo);                        
                        
                        /*
                        GameObject elbow = prefab.FindChildInMaxDepth("elbow");
                        GameObject elbow_collider_container = GameHelper.CreateGameObject("collider", elbow.transform);                        
                        CapsuleCollider elbow_collider = elbow_collider_container.AddComponent<CapsuleCollider>();
                        elbow_collider.direction = 0;
                        elbow_collider.center = new Vector3(-0.22f, 0f, 0f);
                        elbow_collider.radius = 0.25f;
                        elbow_collider.height = 0.95f; 
                        */

                        prefab.AddComponent<SeamothGrapplingArmControl>();                        

                        ArmsCache.Add(ArmTemplate.GrapplingArm, prefab);
                        break;

                    case TechType.ExosuitPropulsionArmModule:

                        prefab.name = "SeamothPropulsionArmPrefab";    
                        
                        DestroyImmediate(prefab.GetComponent<ExosuitPropulsionArm>());

                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_arm_torpedoLauncher_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_drill_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingHook"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingBase"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("wrist"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("object"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("torpedoLauncher"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("drill"));

                        GameObject propulsionGeo = prefab.FindChildInMaxDepth("exosuit_propulsion_geo");
                        propulsionGeo.name = "seamoth_propulsion_geo";
                        GeoCache.Add(propulsionGeo);                        

                        prefab.AddComponent<SeamothPropulsionArmControl>();

                        ArmsCache.Add(ArmTemplate.PropulsionArm, prefab);
                        break;

                    case TechType.ExosuitTorpedoArmModule:

                        prefab.name = "SeamothTorpedoArmPrefab";

                        DestroyImmediate(prefab.GetComponent<ExosuitTorpedoArm>());  
                        
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_propulsion_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_drill_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_grapplingHook_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("exosuit_hand_geo"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingHook"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("grapplingBase"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("wrist"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("object"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("propulsion"));
                        DestroyImmediate(prefab.FindChildInMaxDepth("drill"));

                        GameObject torpedoLauncherGeo = prefab.FindChildInMaxDepth("exosuit_arm_torpedoLauncher_geo");
                        torpedoLauncherGeo.name = "seamoth_arm_torpedoLauncher_geo";

                        GeoCache.Add(torpedoLauncherGeo);                        

                        prefab.AddComponent<SeamothTorpedoArmControl>();                        

                        ArmsCache.Add(ArmTemplate.TorpedoArm, prefab);
                        break;
                }
            }
        }
    }
}
