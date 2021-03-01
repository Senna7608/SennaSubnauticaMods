using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Common.Helpers.SMLHelpers;

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The abstract class to inherit when you want to add new Exosuit arm into the game.    
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
                
        protected readonly string TechTypeName;                
        protected readonly string FriendlyName;
        protected readonly string Description;
        public readonly ArmType ArmType;
        protected readonly TechType PrefabForClone;
        protected readonly TechType RequiredForUnlock;
        protected readonly ModPrefab_Fragment _Fragment;

        private bool isEncyExists = false;
        
        /// <summary>
		/// The cloned arm gameobject. Initialized internally.        
		/// </summary>
        public GameObject PrefabClone { get; private set; }
        /// <summary>
		/// The arm clone template. Initialized internally.       
		/// </summary>
        public ArmTemplate ArmTemplate { get; private set; }

        /// <summary>
		/// Initializes a new instance of the <see cref="T:ModdedArmsHelper.API.ModdedExosuitArm"/> class, the basic class for any arm that can be crafted at a Vehicle Upgrade Console.
        /// </summary>
		/// <param name="techTypeName">The main internal identifier for this item. Your item's <see cref="T:TechType"/> will be created using this name.</param>
        /// <param name="friendlyName">The name displayed in-game for this item whether in the open world or in the inventory.</param>
        /// <param name="description">The description for this item; Typically seen in the PDA, inventory, or crafting screens.</param>        
        /// <param name="armTemplate">The <see cref="T:ModdedArmsHelper.API.ArmTemplate"/> for cloning.</param>
        /// <param name="requiredForUnlock">The required <see cref="T:TechType"/> that must first be scanned or picked up to unlock the blueprint for this item.</param>
        protected CraftableModdedArm(
            string techTypeName,                       
            string friendlyName,
            string description,
            ArmType armType,
            ArmTemplate armTemplate,            
            TechType requiredForUnlock,
            SpawnableArmFragment fragment
            )
            : base(techTypeName, $"{techTypeName}.Prefab")
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
            RegisterArm();
        }

        public virtual void Patch()
        {
            Atlas.Sprite sprite = GetItemSprite();

            TechType = TechTypeHandler.Main.AddTechType(TechTypeName, FriendlyName, Description, sprite, false);

            SetCustomLanguageText();

            PrefabHandler.Main.RegisterPrefab(this);
            CraftDataHandler.Main.SetTechData(TechType, GetRecipe());
            SpriteHandler.Main.RegisterSprite(TechType, sprite);
            CraftDataHandler.Main.SetItemSize(TechType, new Vector2int(1, 1));
            CraftDataHandler.Main.AddToGroup(TechGroup.VehicleUpgrades, TechCategory.VehicleUpgrades, TechType);
            CraftDataHandler.Main.SetEquipmentType(TechType, ArmType == ArmType.ExosuitArm ? EquipmentType.ExosuitArm : (EquipmentType)100);
            CraftDataHandler.Main.SetQuickSlotType(TechType, QuickSlotType.Selectable);
            CraftDataHandler.Main.SetBackgroundType(TechType, CraftData.BackgroundType.ExosuitArm);

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

            if (RequiredForUnlock == TechType.None && _Fragment != null)
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

                KnownTechHandler.Main.SetAnalysisTechEntry(TechType, new TechType[1] { TechType });
            }
            else
            {
                KnownTechHandler.Main.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
            }

            if (ArmType == ArmType.ExosuitArm)
            {
                CraftTreeHandler.Main.AddCraftingNode(CraftTree.Type.SeamothUpgrades, TechType, new string[] { "ExosuitModules" });
            }
            else
            {
                CraftTreeHandler.Main.AddCraftingNode(CraftTree.Type.SeamothUpgrades, TechType, new string[] { "SeamothModules" });
            }

            RegisterArm();
        }

        
        /// <summary>
        /// Initializes the cloned arm for the inventory item.
        /// <remarks>
        /// If you want to set the visual appearance of the arm in the open world or in the inventory, you can implement 'ModifyGameObject' method!        
        public override GameObject GetGameObject()
        {            
            PrefabClone = Object.Instantiate(CraftData.GetPrefabForTechType(PrefabForClone));                               

            PrefabClone.name = TechTypeName;
            
            if (ArmTemplate == ArmTemplate.ClawArm)
            {
                OverrideClawArm();
            }

            PrefabClone.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            ModifyGameObject();

            return PrefabClone;
        }

        private void OverrideClawArm()
        {
            Main.graphics.ArmsTemplateCache.TryGetValue(ArmTemplate.ClawArm, out GameObject armPrefab);

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
        }  

        /// <summary>
		/// This provides the <see cref="T:SMLHelper.V2.Crafting.TechData" /> instance used to designate how this arm is crafted.
		/// </summary>
        protected abstract TechData GetRecipe();

        protected abstract EncyData GetEncyclopediaData();

        protected abstract void RegisterArm();

        protected abstract void SetCustomLanguageText();

        /// <summary>
		/// Use this if You want modify the visual appearance of the arm in the open world.
		/// </summary>
        protected abstract void ModifyGameObject();

        protected abstract Atlas.Sprite GetItemSprite();
    }
}
