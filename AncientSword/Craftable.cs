using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace AncientSword
{
    ///<summary>
    ///Craftable Class from PrimeSonic/Upgraded Vehicles mod
    ///Original code found on GitHub: https://github.com/PrimeSonic/PrimeSonicSubnauticaMods/blob/master/UpgradedVehicles/Craftables/Craftable.cs
    ///</summary>

    internal abstract class Craftable : ModPrefab
    {
        public readonly string NameID;
        public readonly string FriendlyName;
        public readonly string Description;        
        protected readonly TechType PrefabTemplate;

        protected readonly CraftTree.Type FabricatorType;
        protected readonly string[] FabricatorTab;

        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;

        protected Craftable(
            string nameID,
            string friendlyName,
            string description,            
            TechType template,
            CraftTree.Type fabricatorType,
            string[] fabricatorTab,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA)
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
            TechType = TechTypeHandler.AddTechType(NameID, FriendlyName, Description, true);
            Atlas.Sprite swordIcon = null;
            swordIcon = ImageUtils.LoadSpriteFromFile($"./QMods/AncientSword/Assets/{NameID}.png");
            SpriteHandler.RegisterSprite(TechType, swordIcon);

            PrefabHandler.RegisterPrefab(this);

            CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, FabricatorTab);
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.SetEquipmentType(TechType, EquipmentType.Hand);
            CraftDataHandler.SetItemSize(TechType, new Vector2int(2, 2));
            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
        }

        protected abstract TechData GetRecipe();

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(PrefabTemplate);
            prefab.AddComponent<AncientSword>();
            //return Object.Instantiate(AncientSwordPrefab.knife);
            return Object.Instantiate(prefab);
        }        
    }
}
