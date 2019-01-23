using System;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;
using Common.DebugHelper;
using Common;
using SMLHelper.V2.MonoBehaviours;

namespace CannonArm
{
    ///<summary>
    ///Based on Craftable Class from PrimeSonic/Upgraded Vehicles mod
    ///Original code found on GitHub: https://github.com/PrimeSonic/PrimeSonicSubnauticaMods/blob/master/UpgradedVehicles/Craftables/Craftable.cs
    ///</summary>

    internal abstract class CraftArm : ModPrefab
    {
        public readonly string NameID;
        public readonly string FriendlyName;
        public readonly string Description;
        protected readonly TechType PrefabTemplate;
        protected readonly CraftTree.Type FabricatorType;
        protected readonly string FabricatorTab;
        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;
        
        protected CraftArm(
            string nameID,
            string friendlyName,
            string description,            
            TechType template,
            CraftTree.Type fabricatorType,
            string fabricatorTab,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA
            )
            : base(nameID, $"{nameID}Prefab")
        {
            NameID = nameID;
            FriendlyName = friendlyName;
            Description = description;            
            PrefabTemplate = template;
            FabricatorType = fabricatorType;
            FabricatorTab = fabricatorTab;
            RequiredForUnlock = requiredAnalysis;
            GroupForPDA = groupForPDA;
            CategoryForPDA = categoryForPDA;            
        }

        public virtual void Patch()
        {
            TechType = TechTypeHandler.AddTechType(NameID, FriendlyName, Description, ImageUtils.LoadSpriteFromFile($"./QMods/{NameID}/Assets/{NameID}.png"), false);
            CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, FabricatorTab);
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
            PrefabHandler.RegisterPrefab(this);
            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");            
        }

        protected abstract TechData GetRecipe();

        public override GameObject GetGameObject()
        {
            //GameObject prefab = CraftData.GetPrefabForTechType(PrefabTemplate);
            //var gameObject = UnityEngine.Object.Instantiate(prefab);

            GameObject prefab = Main.assetBundle.LoadAsset<GameObject>("ExosuitCannonArm");

            GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
            
            gameObject.name = "ExosuitCannonArm";
            gameObject.AddOrGetComponent<PrefabIdentifier>().ClassId = ClassID;
            gameObject.AddOrGetComponent<TechTag>().type = TechType;
            gameObject.AddOrGetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
            gameObject.AddOrGetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
            gameObject.AddOrGetComponent<Pickupable>().isPickupable = true;

            Fixer fixer = gameObject.AddOrGetComponent<Fixer>();
            fixer.ClassId = ClassID;
            fixer.techType = TechType;

            SkyApplier skyApplier = gameObject.AddOrGetComponent<SkyApplier>();
            skyApplier.renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            skyApplier.anchorSky = Skies.Auto;

            WorldForces worldForces = gameObject.AddOrGetComponent<WorldForces>();
            Rigidbody rigidbody = gameObject.AddOrGetComponent<Rigidbody>();

            worldForces.underwaterGravity = 0;
            worldForces.useRigidbody = rigidbody;

            VFXFabricating vfxFabricating = gameObject.AddOrGetComponent<VFXFabricating>();
            vfxFabricating.localMinY = -3f;
            vfxFabricating.localMaxY = 3f;
            vfxFabricating.posOffset = new Vector3(0f, 0, 0f);
            vfxFabricating.eulerOffset = new Vector3(0f, 90f, -90f);
            vfxFabricating.scaleFactor = 0.5f;

            ExosuitCannonArm component = gameObject.AddOrGetComponent<ExosuitCannonArm>();
                                     
            DebugHelper.DebugGameObject(gameObject);
                
            return UnityEngine.Object.Instantiate(gameObject);
        }        
    }
}
