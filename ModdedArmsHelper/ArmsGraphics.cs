using System.Collections.Generic;
using UnityEngine;
using Common;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using Common.Helpers;

namespace ModdedArmsHelper
{
    [DisallowMultipleComponent]
    internal sealed class ArmsGraphics : MonoBehaviour
    {
        private GameObject InactiveCache;
        private GameObject Templates;

        public GameObject HookPrefab { get; private set; }        

        internal readonly Dictionary<TechType, GameObject> ModdedArmPrefabs = new Dictionary<TechType, GameObject>();

        internal readonly Dictionary<ArmTemplate, GameObject> ArmsTemplateCache = new Dictionary<ArmTemplate, GameObject>();
        
        internal void Awake()
        {
            InactiveCache = ArmServices.main.objectHelper.CreateGameObject("InactiveCache", transform, false);
            Templates = ArmServices.main.objectHelper.CreateGameObject("Templates", InactiveCache.transform, false);

            InitArmsGraphics();            

            Main.graphics = this;           
        }        

        internal void RegisterNewArm(CraftableModdedArm armPrefab, IArmModdingRequest armModdingRequest)
        {
            if(ModdedArmPrefabs.ContainsKey(armPrefab.TechType))
            {
                return;
            }

            string techName = TechTypeExtensions.AsString(armPrefab.TechType);

            GameObject clonedArm = Instantiate(ArmsTemplateCache[armPrefab.ArmTemplate], InactiveCache.transform);
            
            clonedArm.name = techName;            
            clonedArm.SetActive(false);
            SetupHelper graphicsHelper =  clonedArm.AddComponent<SetupHelper>();

            ArmTag armTag = clonedArm.AddComponent<ArmTag>();
            armTag.armType = armPrefab.ArmType;
            armTag.armTemplate = armPrefab.ArmTemplate;
            armTag.techType = armPrefab.TechType;

            armModdingRequest.SetUpArm(clonedArm, graphicsHelper);
            
            if (armPrefab.ArmType == ArmType.ExosuitArm)
            {
                armModdingRequest.GetExosuitArmHandler(clonedArm);
            }
            else
            {
                armModdingRequest.GetSeamothArmHandler(clonedArm);
            }

            ModdedArmPrefabs.Add(armPrefab.TechType, clonedArm);            

            SNLogger.Log($"New {armPrefab.ArmType} registered: TechType ID: [{(int)armPrefab.TechType}], TechType name: [{techName}]");            
        }

        internal ISeamothArm SpawnArm(TechType techType, Transform parent)
        {
            GameObject arm = Instantiate(GetSeamothArmPrefab(techType), parent.transform);
                       
            arm.transform.localRotation = Quaternion.identity;
            arm.transform.localPosition = Vector3.zero;
            arm.SetActive(true);

            return arm.GetComponent<ISeamothArm>();
        }        

        private GameObject GetSeamothArmPrefab(TechType techType)
        {
            if (ModdedArmPrefabs.TryGetValue(techType, out GameObject armPrefab))
            {
                return armPrefab;
            }

            return null;
        }

        private void InitArmsGraphics()
        {
            Exosuit exosuitResource = Resources.Load<GameObject>("worldentities/tools/exosuit").GetComponent<Exosuit>();
            
            GameObject clone, model;
            Transform ArmRig, elbow;

            foreach (Exosuit.ExosuitArmPrefab armPrefab in exosuitResource.armPrefabs)
            {
                clone = UWE.Utils.InstantiateDeactivated(armPrefab.prefab, Templates.transform, Vector3.zero, Quaternion.Euler(0, 0, 0));

                ArmRig = clone.transform.Find("exosuit_01_armRight/ArmRig");
                elbow = ArmRig.Find("clavicle/shoulder/bicepPivot/elbow/");                

                switch (armPrefab.techType)
                {
                    case TechType.ExosuitClawArmModule:
                        clone.name = "ClawArmTemplate";
                        DestroyImmediate(clone.GetComponent<ExosuitClawArm>());

                        DestroyImmediate(ArmRig.Find("exosuit_arm_torpedoLauncher_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_drill_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_hand_geo").gameObject);                        
                        DestroyImmediate(ArmRig.Find("exosuit_propulsion_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("grapplingHook").gameObject);
                        DestroyImmediate(elbow.Find("drill").gameObject);
                        DestroyImmediate(elbow.Find("grapplingBase").gameObject);
                        DestroyImmediate(elbow.Find("object").gameObject);
                        DestroyImmediate(elbow.Find("propulsion").gameObject);
                        DestroyImmediate(elbow.Find("torpedoLauncher").gameObject);

                        model = ArmRig.Find("exosuit_hand_geo").gameObject;
                        model.name = "ArmModel";

                        ArmsTemplateCache.Add(ArmTemplate.ClawArm, clone);
                        break;

                    case TechType.ExosuitDrillArmModule:
                        clone.name = "DrillArmTemplate";
                        DestroyImmediate(clone.GetComponent<ExosuitDrillArm>());

                        DestroyImmediate(ArmRig.Find("exosuit_arm_torpedoLauncher_geo").gameObject);                        
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_hand_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_hand_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_propulsion_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("grapplingHook").gameObject);                        
                        DestroyImmediate(elbow.Find("grapplingBase").gameObject);
                        DestroyImmediate(elbow.Find("object").gameObject);
                        DestroyImmediate(elbow.Find("propulsion").gameObject);
                        DestroyImmediate(elbow.Find("torpedoLauncher").gameObject);
                        DestroyImmediate(elbow.Find("wrist").gameObject);

                        model = ArmRig.Find("exosuit_drill_geo").gameObject;
                        model.name = "ArmModel";

                        ArmsTemplateCache.Add(ArmTemplate.DrillArm, clone);
                        break;

                    case TechType.ExosuitGrapplingArmModule:
                        clone.name = "GrapplingArmTemplate";

                        ExosuitGrapplingArm component = clone.GetComponent<ExosuitGrapplingArm>();

                        HookPrefab = Instantiate(component.hookPrefab, Templates.transform);
                        HookPrefab.name = "Hook";

                        DestroyImmediate(component);
                        
                        DestroyImmediate(elbow.Find("drill").gameObject);                        
                        DestroyImmediate(elbow.Find("object").gameObject);
                        DestroyImmediate(elbow.Find("propulsion").gameObject);
                        DestroyImmediate(elbow.Find("torpedoLauncher").gameObject);
                        DestroyImmediate(elbow.Find("wrist").gameObject);

                        model = ArmRig.Find("exosuit_grapplingHook_geo").gameObject;
                        model.name = "ArmModel";

                        ArmsTemplateCache.Add(ArmTemplate.GrapplingArm, clone);
                        break;

                    case TechType.ExosuitPropulsionArmModule:
                        clone.name = "PropulsionArmTemplate";
                        DestroyImmediate(clone.GetComponent<ExosuitPropulsionArm>());

                        DestroyImmediate(ArmRig.Find("exosuit_arm_torpedoLauncher_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_drill_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_hand_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_hand_geo").gameObject);                        
                        DestroyImmediate(ArmRig.Find("grapplingHook").gameObject);
                        DestroyImmediate(elbow.Find("drill").gameObject);
                        DestroyImmediate(elbow.Find("grapplingBase").gameObject);
                        DestroyImmediate(elbow.Find("object").gameObject);                        
                        DestroyImmediate(elbow.Find("torpedoLauncher").gameObject);
                        DestroyImmediate(elbow.Find("wrist").gameObject);

                        model = ArmRig.Find("exosuit_propulsion_geo").gameObject;
                        model.name = "ArmModel";

                        ArmsTemplateCache.Add(ArmTemplate.PropulsionArm, clone);
                        break;

                    case TechType.ExosuitTorpedoArmModule:
                        clone.name = "TorpedoArmTemplate";
                        DestroyImmediate(clone.GetComponent<ExosuitTorpedoArm>());
                        
                        DestroyImmediate(ArmRig.Find("exosuit_drill_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_grapplingHook_hand_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_hand_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("exosuit_propulsion_geo").gameObject);
                        DestroyImmediate(ArmRig.Find("grapplingHook").gameObject);
                        DestroyImmediate(elbow.Find("drill").gameObject);
                        DestroyImmediate(elbow.Find("grapplingBase").gameObject);
                        DestroyImmediate(elbow.Find("object").gameObject);
                        DestroyImmediate(elbow.Find("propulsion").gameObject);                        
                        DestroyImmediate(elbow.Find("wrist").gameObject);

                        model = ArmRig.Find("exosuit_arm_torpedoLauncher_geo").gameObject;
                        model.name = "ArmModel";

                        ArmsTemplateCache.Add(ArmTemplate.TorpedoArm, clone);
                        break;
                }                
            }

            
            SNLogger.Log($"Graphics cache initialization complete. GameObject: {name}, path: {transform.GetPath()}, ID: {GetInstanceID()}");
        }
    }
}
