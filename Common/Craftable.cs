using System;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///Based on Craftable Class from PrimeSonic/Upgraded Vehicles mod
    ///Original code found on GitHub: https://github.com/PrimeSonic/PrimeSonicSubnauticaMods/blob/master/UpgradedVehicles/Craftables/Craftable.cs
    ///</summary>

    internal abstract class Craftable : ModPrefab
    {
        public readonly string NameID;
        public readonly string FriendlyName;
        public readonly string Description;
        public GameObject prefab { get; private set; }

        protected readonly TechType PrefabTemplate;
        protected readonly CraftTree.Type FabricatorType;
        protected readonly string FabricatorTab;
        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;
        protected readonly EquipmentType TypeForEquipment;
        protected readonly QuickSlotType TypeForQuickslot;
        protected readonly Type[] ComponentsToAdd;
        internal readonly string GameResourceFileName;        

        protected Craftable(
            string nameID,
            string friendlyName,
            string description,            
            TechType template,
            CraftTree.Type fabricatorType,
            string fabricatorTab,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            EquipmentType equipmentType,
            QuickSlotType quickSlotType,
            Type[] componentsToAdd,
            string gamerResourceFileName
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
            TypeForEquipment = equipmentType;
            TypeForQuickslot = quickSlotType;
            ComponentsToAdd = componentsToAdd;
            GameResourceFileName = gamerResourceFileName;
        }

        public virtual void Patch()
        {
            Atlas.Sprite sprite = ImageUtils.LoadSpriteFromFile($"./QMods/{NameID}/Assets/{NameID}.png");
            TechType = TechTypeHandler.AddTechType(NameID, FriendlyName, Description, sprite , false);
            SpriteHandler.RegisterSprite(TechType, sprite);            
            CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, FabricatorTab);
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
            CraftDataHandler.SetEquipmentType(TechType, TypeForEquipment);
            CraftDataHandler.SetQuickSlotType(TechType, TypeForQuickslot);
            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");

            PrefabHandler.RegisterPrefab(this);            
        }

        protected abstract TechData GetRecipe();

        public override GameObject GetGameObject()
        {
            if (GameResourceFileName == null)
                prefab = CraftData.GetPrefabForTechType(PrefabTemplate);
            else
                prefab = Resources.Load<GameObject>(GameResourceFileName);

            if (ComponentsToAdd != null)
            {
                foreach (Type component in ComponentsToAdd)
                {
                    prefab.AddIfNeedComponent(component);
                }
            }
            
            return prefab;
        }        
    }
}
