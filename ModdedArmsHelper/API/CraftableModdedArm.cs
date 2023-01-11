using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System.Collections;
using SMLExpander;
using Common;
using UWE;
using ModdedArmsHelper.API.Interfaces;

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The abstract class to inherit when you want to add new Exosuit and/or Seamoth arm into the game.    
    /// </summary>
    public abstract class CraftableModdedArm : ModPrefab
    {
        private readonly Dictionary<ArmTemplate, TechType> ArmTypes = new Dictionary<ArmTemplate, TechType>()
        {
            { ArmTemplate.ClawArm, TechType.ExosuitDrillArmModule },
            { ArmTemplate.DrillArm, TechType.ExosuitDrillArmModule },
            { ArmTemplate.GrapplingArm, TechType.ExosuitGrapplingArmModule },
            { ArmTemplate.PropulsionArm, TechType.ExosuitPropulsionArmModule },
            { ArmTemplate.TorpedoArm, TechType.ExosuitTorpedoArmModule },
        };

#pragma warning disable CS1591  //XML documentation
        protected readonly string TechTypeName;                
        protected readonly string FriendlyName;
        protected readonly string Description;
        public readonly ArmType ArmType;
        protected readonly TechType PrefabForClone;
        protected readonly TechType RequiredForUnlock;
        protected readonly ModPrefab_Fragment _Fragment;
#pragma warning restore CS1591  //XML documentation
        private bool isEncyExists = false;
        private FMODAsset new_blueprint;
        /// <summary>
        /// The cloned arm gameobject.<br/>Initialized internally.        
        /// </summary>
        public GameObject PrefabClone { get; private set; }

        /// <summary>
		/// The arm clone template.<br/>Initialized internally.       
		/// </summary>
        public ArmTemplate ArmTemplate { get; private set; }

        /// <summary>
		/// Initializes a new instance of the <see cref="CraftableModdedArm"/> class, the basic class for any arm that can be crafted at a Vehicle Upgrade Console.        
		/// </summary>        
        /// <param name="techTypeName">The main internal identifier for this item. Your item's <see cref="TechType"/> will be created using this name.</param>
        /// <param name="friendlyName">The name displayed in-game for this item whether in the open world or in the inventory.</param>
        /// <param name="description">The description for this item; Typically seen in the PDA, inventory, or crafting screens.</param>
        /// <param name="armType">The <see cref="ArmType"/> parameter (Exosuit or Seamoth).</param>
        /// <param name="armTemplate">The base <see cref="ArmTemplate"/> for cloning.</param>
        /// <param name="requiredForUnlock">The required <see cref="TechType"/> that must first be scanned or picked up to unlock the blueprint for this item. If you use fragment set this one to TechType.None</param>
        /// <param name="fragment">The <see cref="SpawnableArmFragment"/> parameter. If you not used fragment set this one to null.</param>
        protected CraftableModdedArm(
            string techTypeName,                       
            string friendlyName,
            string description,
            ArmType armType,
            ArmTemplate armTemplate,            
            TechType requiredForUnlock,
            SpawnableArmFragment fragment
            )
            : base(techTypeName, $"{techTypeName}.prefab")
        {
            TechTypeName = techTypeName;                        
            FriendlyName = friendlyName;
            Description = description;
            ArmType = armType;
            PrefabForClone = ArmTypes[armTemplate];            
            RequiredForUnlock = requiredForUnlock;
            _Fragment = fragment;
            ArmTemplate = armTemplate;

            IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
        }
                
        private void OnQuitEvent()
        {
            SendArmRequest();
        }

        private void SendArmRequest()
        {
            RegisterArmRequest registerArmRequest = RegisterArm();

            ArmServices.main.RegisterArm(registerArmRequest.craftableModdedArm, registerArmRequest.armModdingRequest);
        }
        
        /// <summary>
        /// Call this from the MOD main class to execute the patch.        
        /// </summary>
        public void Patch()
        {
            TechType = TechTypeHandler.Main.AddTechType(TechTypeName, FriendlyName, Description, (Atlas.Sprite)null, false);

            CoroutineHost.StartCoroutine(InitAsync());
        }

        private IEnumerator InitAsync()
        {
            SNLogger.Debug($"{TechTypeName}: Async Patch started...");

            while (!uGUI.isInitialized)
            {                
                yield return null;
            }

            InternalPatch();
            SNLogger.Log($"{TechTypeName}: Async Patch completed.");
            yield break;
        }

        private void InternalPatch()
        {
            SNLogger.Debug($"{TechTypeName}: Internal patch started...");

            Atlas.Sprite sprite = GetItemSprite();            

            SetCustomLanguageText();

            PrefabHandler.Main.RegisterPrefab(this);
            CraftDataHandler.Main.SetTechData(TechType, GetRecipe());
            SpriteHandler.Main.RegisterSprite(TechType, sprite);
            CraftDataHandler.Main.SetItemSize(TechType, new Vector2int(1, 1));
            CraftDataHandler.Main.AddToGroup(TechGroup.VehicleUpgrades, TechCategory.VehicleUpgrades, TechType);
            CraftDataHandler.Main.SetEquipmentType(TechType, ArmType == ArmType.ExosuitArm ? EquipmentType.ExosuitArm : (EquipmentType)100);
            CraftDataHandler.Main.SetQuickSlotType(TechType, QuickSlotType.Selectable);
            CraftDataHandler.Main.SetBackgroundType(TechType, CraftData.BackgroundType.ExosuitArm);

            new_blueprint = ScriptableObject.CreateInstance<FMODAsset>();
            new_blueprint.name = "new_blueprint";
            new_blueprint.path = "event:/tools/scanner/new_blueprint";

            EncyData encyData = GetEncyclopediaData();

            if (encyData != null)
            {
                isEncyExists = true;

                PDAEncyclopedia.EntryData entryData = new PDAEncyclopedia.EntryData()
                {
                    key = ClassID,
                    path = EncyHelper.GetEncyPath(encyData.node),
                    nodes = EncyHelper.GetEncyNodes(encyData.node),                    
                    unlocked = false,
                    popup = null,
                    image = encyData.image,
                    audio = null                    
                };

                PDAEncyclopediaHandler.Main.AddCustomEntry(entryData);

                LanguageHandler.Main.SetLanguageLine($"Ency_{ClassID}", encyData.title);
                LanguageHandler.Main.SetLanguageLine($"EncyDesc_{ClassID}", encyData.description);
            }

            if (_Fragment != null)
            {
                PDAScanner.EntryData scannerEntryData = new PDAScanner.EntryData()
                {
                    key = _Fragment.TechType,
                    blueprint = TechType,
                    destroyAfterScan = _Fragment.DestroyAfterScan,
                    encyclopedia = isEncyExists ? ClassID : null,
                    isFragment = true,
                    locked = false,
                    scanTime = _Fragment.ScanTime,
                    totalFragments = _Fragment.TotalFragments                    
                };

                PDAHandler.Main.AddCustomScannerEntry(scannerEntryData);                

                KnownTechHandler.Main.SetAnalysisTechEntry(TechType, new TechType[1] { TechType }, new_blueprint);
            }
            else
            {
                KnownTechHandler.Main.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!", new_blueprint);
            }

            if (ArmType == ArmType.ExosuitArm)
            {
                CraftTreeHandler.Main.AddCraftingNode(CraftTree.Type.SeamothUpgrades, TechType, new string[] { "ExosuitModules" });
            }
            else
            {
                CraftTreeHandler.Main.AddCraftingNode(CraftTree.Type.SeamothUpgrades, TechType, new string[] { "SeamothModules" });
            }

            SendArmRequest();

            SNLogger.Log($"{TechTypeName}: Internal Patch completed.");
        }

        /// <summary>
        /// Initializes the cloned arm for the inventory item.
        /// </summary>
        /// <remarks>
        /// If you want to set the visual appearance of the arm in the open world or in the inventory,<br/>then you can implement it in this <see cref="ModifyGameObjectAsync"/> method.
        /// </remarks>
        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            SNLogger.Debug($"GetGameObjectAsync started for arm: [{TechTypeName}]...");

            CoroutineTask<GameObject> prefabRequest = CraftData.GetPrefabForTechTypeAsync(PrefabForClone);
            yield return prefabRequest;

            GameObject result = prefabRequest.GetResult();

            if (result == null)
            {                
                yield break;
            }
                        
            PrefabClone = Object.Instantiate(result);

            PrefabClone.name = TechTypeName;
            
            if (ArmTemplate == ArmTemplate.ClawArm)
            {
                TaskResult<bool> overrideResult = new TaskResult<bool>();
                CoroutineTask<bool> overrideRequest = new CoroutineTask<bool>(OverrideClawArmAsync(overrideResult), overrideResult);
                yield return overrideRequest;

                if (!overrideRequest.GetResult())
                {
                    SNLogger.Error($"OverrideClawArmAsync failed for TechType: [{TechTypeName}]");
                    yield break;
                }
            }

            PrefabClone.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            TaskResult<bool> modifyResult = new TaskResult<bool>();
            CoroutineTask<bool> modifyRequest = new CoroutineTask<bool>(ModifyGameObjectAsync(modifyResult), modifyResult);
            yield return modifyRequest;
            if (!modifyRequest.GetResult())
            {
                SNLogger.Error($"ModifyGameObjectAsync failed for TechType: [{TechTypeName}]");
                yield break;
            }

            SNLogger.Debug($"GetGameObjectAsync completed for TechType: [{TechTypeName}]");
            gameObject.Set(PrefabClone);
            yield break;
        }

        private IEnumerator OverrideClawArmAsync(IOut<bool> success)
        {
            SNLogger.Debug($"OverrideClawArmAsync started...");

            Main.armsGraphics.ArmsTemplateCache.TryGetValue(ArmTemplate.ClawArm, out GameObject armPrefab);

            SkinnedMeshRenderer smr = armPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            Mesh clawMesh = smr.sharedMesh;

            MeshFilter mf = PrefabClone.GetComponentInChildren<MeshFilter>();
            mf.sharedMesh = Object.Instantiate(clawMesh);            

            MeshRenderer mr = PrefabClone.GetComponentInChildren<MeshRenderer>();
            mr.materials = (Material[])smr.materials.Clone();

            Object.Destroy(PrefabClone.GetComponentInChildren<CapsuleCollider>());

            BoxCollider bc_1 = PrefabClone.FindChild("collider").AddComponent<BoxCollider>();

            bc_1.size = new Vector3(1.29f, 0.33f, 0.42f);
            bc_1.center = new Vector3(-0.53f, 0f, 0.04f);

            GameObject collider2 = new GameObject("collider2");
            collider2.transform.SetParent(PrefabClone.transform, false);
            collider2.transform.localPosition = new Vector3(-1.88f, 0.07f, 0.50f);
            collider2.transform.localRotation = Quaternion.Euler(0, 34, 0);

            BoxCollider bc_2 = collider2.AddComponent<BoxCollider>();
            bc_2.size = new Vector3(1.06f, 0.23f, 0.31f);
            bc_2.center = new Vector3(0, -0.08f, 0);

            SNLogger.Debug($"OverrideClawArmAsync completed.");
            success.Set(true);
            yield break;
        }  

        /// <summary>
		/// This provides the <see cref="TechData"/> instance used to designate how this arm is crafted.
		/// </summary>
        protected abstract TechData GetRecipe();

        /// <summary>
		/// If you want to set up an encyclopedia data, you can do it here.<br/>If not, simply return null.<br/>See also <see cref="EncyData"/> class.
		/// </summary>
        protected abstract EncyData GetEncyclopediaData();

        /// <summary>
		/// In this method, you must set up and return the <see cref="RegisterArmRequest"/> <see cref="CraftableModdedArm"/> and the <see cref="IArmModdingRequest"/> interface.
		/// </summary>
        protected abstract RegisterArmRequest RegisterArm();

        /// <summary>
		/// Use this if You want to add custom language lines.<br/>See also <see cref="LanguageHandler"/>.
		/// </summary>
        protected abstract void SetCustomLanguageText();

        /// <summary>
		/// Use this if You want modify the visual appearance of the arm in the open world.<br/>Use the <see cref="CraftableModdedArm.PrefabClone"/> parameter to access the cloned arm.
		/// </summary>
        protected abstract IEnumerator ModifyGameObjectAsync(IOut<bool> success);

        /// <summary>
        /// Use this to set up the sprite of the arm.
        /// </summary>
        protected abstract Atlas.Sprite GetItemSprite();
    }
}