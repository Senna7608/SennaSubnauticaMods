using System.Collections.Generic;
using UnityEngine;
using Common;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using Common.Helpers;
using System.Collections;
using UWE;

namespace ModdedArmsHelper
{
    [DisallowMultipleComponent]
    internal sealed class ArmsGraphics : MonoBehaviour
    {
        public GameObject exosuitResource;
        private GameObject InactiveCache;
        private GameObject Templates;

        public GameObject HookPrefab { get; private set; }        

        internal readonly Dictionary<TechType, GameObject> ModdedArmPrefabs = new Dictionary<TechType, GameObject>();

        internal readonly Dictionary<ArmTemplate, GameObject> ArmsTemplateCache = new Dictionary<ArmTemplate, GameObject>();
        
        internal enum REQUESTRESULT
        {
            UNDER_PROCESS,
            COMPLETED,
            FAILED
        }

        internal void Awake()
        {
            InactiveCache = ArmServices.main.objectHelper.CreateGameObject("InactiveCache", transform, false);
            Templates = ArmServices.main.objectHelper.CreateGameObject("Templates", InactiveCache.transform, false);

            CoroutineHost.StartCoroutine(LoadExosuitResourceAsync());

            CoroutineHost.StartCoroutine(InitializeArmsGraphics());            
        }        

        internal IEnumerator RegisterArm()
        {
            foreach (KeyValuePair<CraftableModdedArm, IArmModdingRequest> kvp in ArmServices.main.waitForRegistration)
            {
                TaskResult<REQUESTRESULT> taskResult = new TaskResult<REQUESTRESULT>();
                CoroutineTask<REQUESTRESULT> registerRequest = new CoroutineTask<REQUESTRESULT>(RegisterNewArmAsync(kvp.Key, kvp.Value, taskResult), taskResult);
                yield return registerRequest;

                switch(taskResult.Get())
                {
                    case REQUESTRESULT.UNDER_PROCESS: yield return null;
                        break;
                    case REQUESTRESULT.FAILED: continue;
                }               
                                
            }

            SNLogger.Log("Processing of modded arms applied for registration has completed.");
            yield break;
        }

        internal IEnumerator RegisterNewArmAsync(CraftableModdedArm armPrefab, IArmModdingRequest armModdingRequest, IOut<REQUESTRESULT> taskResult)
        {
            SNLogger.Log($"Processing data for new [{armPrefab.ArmType}], TechType: {armPrefab.TechType}, Request: {armModdingRequest}");

            if (ModdedArmPrefabs.ContainsKey(armPrefab.TechType))
            {
                yield break;
            }

            string techName = TechTypeExtensions.AsString(armPrefab.TechType);

            GameObject clonedArm = Instantiate(ArmsTemplateCache[armPrefab.ArmTemplate], InactiveCache.transform);
            
            clonedArm.name = techName;            
            clonedArm.SetActive(false);
            LowerArmHelper graphicsHelper =  clonedArm.AddComponent<LowerArmHelper>();

            ArmTag armTag = clonedArm.AddComponent<ArmTag>();
            armTag.armType = armPrefab.ArmType;
            armTag.armTemplate = armPrefab.ArmTemplate;
            armTag.techType = armPrefab.TechType;
                        
            TaskResult<bool> success = new TaskResult<bool>();
            CoroutineTask<bool> setupRequest = new CoroutineTask<bool>(armModdingRequest.SetUpArmAsync(clonedArm, graphicsHelper, success), success) ;
            yield return setupRequest;

            if (!success.Get())
            {
                SNLogger.Error($"SetUpArmAsync failed for TechType: [{techName}]!");
                taskResult.Set(REQUESTRESULT.FAILED);
                yield break;
            }

            if (armPrefab.ArmType == ArmType.ExosuitArm)
            {
                armModdingRequest.GetExosuitArmHandler(clonedArm);
            }
            else
            {
                armModdingRequest.GetSeamothArmHandler(clonedArm);
            }

            ModdedArmPrefabs.Add(armPrefab.TechType, clonedArm);            

            SNLogger.Log($"Processing complete for new [{armPrefab.ArmType}], TechType ID: [{(int)armPrefab.TechType}], TechType: [{techName}]");
            taskResult.Set(REQUESTRESULT.COMPLETED);
            yield break;
        }

        internal ISeamothArm SpawnArm(TechType techType, Transform parent)
        {
            SNLogger.Debug($"SpawnArm: techType: {techType}, Transform: {parent.name}");

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

        private IEnumerator LoadExosuitResourceAsync()
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Tools/Exosuit.prefab");

            yield return request;

            if (!request.TryGetPrefab(out GameObject prefab))
            {
                SNLogger.Error("API message: Cannot load [Exosuit] prefab!");
                yield break;
            }

            exosuitResource = UWE.Utils.InstantiateDeactivated(prefab, transform, Vector3.zero, Quaternion.identity);

            exosuitResource.GetComponent<Exosuit>().enabled = false;
            exosuitResource.GetComponent<Rigidbody>().isKinematic = true;
            exosuitResource.GetComponent<WorldForces>().enabled = false;
            UWE.Utils.ZeroTransform(exosuitResource.transform);

            yield break;
        }

        private IEnumerator InitializeArmsGraphics()
        {
            while (exosuitResource == null)
            {
                yield return null;
            }

            Exosuit exosuit = exosuitResource.GetComponent<Exosuit>();

            GameObject clone, model;
            Transform ArmRig, elbow;

            foreach (Exosuit.ExosuitArmPrefab armPrefab in exosuit.armPrefabs)
            {
                clone = UWE.Utils.InstantiateDeactivated(armPrefab.prefab, Templates.transform, Vector3.zero, Quaternion.Euler(0, 0, 0));

                ArmRig = clone.transform.Find("exosuit_01_armRight/ArmRig");
                elbow = ArmRig.Find("clavicle/shoulder/bicepPivot/elbow/");

                ArmServices.main.objectHelper.CreateGameObject("ModdedLowerArmContainer", elbow, Vector3.zero, Vector3.zero, Vector3.one);

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
            
            SNLogger.Log($"Cache initialization completed. GameObject: {name}, path: {transform.GetPath()}, ID: {GetInstanceID()}");

            Main.armsGraphics = this;            
            SNLogger.Log("Processing of modded arms applied for registration has started...");
            CoroutineHost.StartCoroutine(RegisterArm());
            //DestroyImmediate(exosuitResource);
            yield break;
        }
    }
}
