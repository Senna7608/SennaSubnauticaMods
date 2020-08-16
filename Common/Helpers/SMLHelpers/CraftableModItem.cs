using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Common.Helpers.SMLHelpers
{
    internal abstract class CraftableModItem : ModPrefab
    {
        protected readonly string NameID;
        protected readonly string IconFilePath;
        protected readonly TechType IconTechType;
        protected readonly string FriendlyName;
        protected readonly string Description;
        public GameObject _GameObject { get; private set; }

        protected readonly TechType PrefabTemplate;
        protected readonly TabNode NewTabNode;
        protected readonly CraftTree.Type[] FabricatorTypes;
        protected readonly string[][] FabricatorTabs;
        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;
        protected readonly EquipmentType TypeForEquipment;
        protected readonly QuickSlotType TypeForQuickslot;
        protected readonly CraftData.BackgroundType BackgroundType;
        protected readonly Vector2int ItemSize;
        protected readonly string GameResourceFileName;

        public class TabNode
        {
            public readonly CraftTree.Type craftTree;
            public readonly string uniqueName;
            public readonly string displayName;
            public readonly Sprite sprite;

            public TabNode(CraftTree.Type craftTree, string uniqueName, string displayName, Sprite sprite)
            {
                this.craftTree = craftTree;
                this.uniqueName = uniqueName;
                this.displayName = displayName;
                this.sprite = sprite;
            }
        }

        protected CraftableModItem(
            string nameID,
            string iconFilePath,
            TechType iconTechType,
            string friendlyName,
            string description,
            TechType template,
            TabNode newTabNode,
            CraftTree.Type[] fabricatorTypes,
            string[][] fabricatorTabs,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            EquipmentType equipmentType,
            QuickSlotType quickSlotType,
            CraftData.BackgroundType backgroundType,
            Vector2int itemSize,
            string gamerResourceFileName
            )
            : base(nameID, $"{nameID}:Prefab")
        {
            NameID = nameID;
            IconFilePath = iconFilePath;
            IconTechType = iconTechType;
            FriendlyName = friendlyName;
            Description = description;
            PrefabTemplate = template;
            NewTabNode = newTabNode;
            FabricatorTypes = fabricatorTypes;
            FabricatorTabs = fabricatorTabs;
            RequiredForUnlock = requiredAnalysis;
            GroupForPDA = groupForPDA;
            CategoryForPDA = categoryForPDA;
            TypeForEquipment = equipmentType;
            TypeForQuickslot = quickSlotType;
            BackgroundType = backgroundType;
            ItemSize = itemSize;
            GameResourceFileName = gamerResourceFileName;

            //IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
        }

        private void OnQuitEvent()
        {
            Patch();
        }

        public virtual void Patch()
        {
            PrefabHandler.Main.RegisterPrefab(this);

            Atlas.Sprite sprite = null;

            if (IconFilePath != null)
            {
                try
                {
                    sprite = ImageUtils.LoadSpriteFromFile(IconFilePath);
                }
                catch
                {
                    SNLogger.Error(NameID, $"File [{IconFilePath}] not Found!");
                }
            }
            else if (IconTechType != TechType.None)
            {
                try
                {
                    sprite = GetResourceIcon(IconTechType);
                }
                catch
                {
                    SNLogger.Error(NameID, $"Resource TechType icon [{IconTechType.ToString()}] not Found!");
                }
            }
            else
            {
                try
                {
                    sprite = GetResourceIcon(PrefabTemplate);
                }
                catch
                {
                    SNLogger.Error(NameID, $"Resource template icon [{PrefabTemplate.ToString()}] not Found!");
                }
            }

            TechType = TechTypeHandler.Main.AddTechType(NameID, FriendlyName, Description, sprite, false);

            SpriteHandler.Main.RegisterSprite(TechType, sprite);

            if (NewTabNode != null)
            {
                CraftTreeHandler.Main.AddTabNode(NewTabNode.craftTree, NewTabNode.uniqueName, NewTabNode.displayName, NewTabNode.sprite);
            }

            for (int i = 0; i < FabricatorTypes.Length; i++)
            {
                if (FabricatorTabs == null)
                {                    
                    CraftTreeHandler.Main.AddCraftingNode(FabricatorTypes[i], TechType);
                }
                else
                {
                    CraftTreeHandler.Main.AddCraftingNode(FabricatorTypes[i], TechType, FabricatorTabs[i]);
                }
            }

            CraftDataHandler.Main.SetTechData(TechType, GetRecipe());
            CraftDataHandler.Main.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
            CraftDataHandler.Main.SetEquipmentType(TechType, TypeForEquipment);
            CraftDataHandler.Main.SetQuickSlotType(TechType, TypeForQuickslot);
            CraftDataHandler.Main.SetItemSize(TechType, ItemSize);
            CraftDataHandler.Main.SetBackgroundType(TechType, BackgroundType);
            KnownTechHandler.Main.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
        }

        protected abstract TechData GetRecipe();

        public override GameObject GetGameObject()
        {
            if (GameResourceFileName == null)
            {
                try
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(PrefabTemplate));
                }
                catch
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                    SNLogger.Warn(NameID, "Cannot instantiate prefab from TechType! Replacing this item to Titanium!");
                }
            }
            else
            {
                try
                {
                    _GameObject = Object.Instantiate(Resources.Load<GameObject>(GameResourceFileName));
                }
                catch
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                    SNLogger.Warn(NameID, "Cannot instantiate prefab from resource! Replacing this item to Titanium!");
                }
            }

            _GameObject.name = NameID;

            return _GameObject;
        }

        public Atlas.Sprite GetResourceIcon(TechType techType)
        {
            return SpriteManager.Get(techType);
        }
    }
}
