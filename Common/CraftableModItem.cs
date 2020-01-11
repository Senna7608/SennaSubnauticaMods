using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using System;
using UnityEngine;

namespace Common
{
    internal abstract class CraftableModItem : ModPrefab
    {
        protected readonly string NameID;
        protected readonly string IconFilename;
        protected readonly TechType IconTechType;
        protected readonly string FriendlyName;
        protected readonly string Description;
        protected GameObject _GameObject { get; private set; }

        protected readonly TechType PrefabTemplate;
        protected readonly CraftTree.Type FabricatorType;
        protected readonly string[] FabricatorTab;
        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;
        protected readonly EquipmentType TypeForEquipment;
        protected readonly QuickSlotType TypeForQuickslot;
        protected readonly CraftData.BackgroundType BackgroundType;
        protected readonly Vector2int ItemSize;
        protected readonly string GameResourceFileName;        

        protected CraftableModItem(
            string nameID,
            string iconFileName,
            TechType iconTechType,
            string friendlyName,
            string description,            
            TechType template,
            CraftTree.Type fabricatorType,
            string[] fabricatorTab,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            EquipmentType equipmentType,
            QuickSlotType quickSlotType,
            CraftData.BackgroundType backgroundType,
            Vector2int itemSize,            
            string gamerResourceFileName
            )
            : base(nameID, $"{nameID}Prefab")
        {
            NameID = nameID;
            IconFilename = iconFileName;
            IconTechType = iconTechType;
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
            BackgroundType = backgroundType;
            ItemSize = itemSize;            
            GameResourceFileName = gamerResourceFileName;
        }

        public virtual void Patch()
        {
            Atlas.Sprite sprite = null;

            if (IconFilename != null)
            {
                string iconfilePath = $"./QMods/{IconFilename}/Assets/{IconFilename}.png";

                try
                {                    
                    sprite = ImageUtils.LoadSpriteFromFile(iconfilePath);
                }
                catch
                {
                    SNLogger.Log($"[{NameID}] ***ERROR! File [{iconfilePath}] not Found! ");
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
                    SNLogger.Log($"[{NameID}] ***ERROR! Resource TechType icon [{IconTechType.ToString()}] not Found! ");
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
                    SNLogger.Log($"[{NameID}] ***ERROR! Resource template icon [{PrefabTemplate.ToString()}] not Found! ");
                }
            }


            TechType = TechTypeHandler.AddTechType(NameID, FriendlyName, Description, sprite , false);
            SpriteHandler.RegisterSprite(TechType, sprite);            
            CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, FabricatorTab);
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
            CraftDataHandler.SetEquipmentType(TechType, TypeForEquipment);
            CraftDataHandler.SetQuickSlotType(TechType, TypeForQuickslot);
            CraftDataHandler.SetItemSize(TechType, ItemSize);
            CraftDataHandler.SetBackgroundType(TechType, BackgroundType);
            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");

            PrefabHandler.RegisterPrefab(this);            
        }

        protected abstract TechData GetRecipe();        

        public override GameObject GetGameObject()
        {
            if (GameResourceFileName == null)
            {
                try
                {
                    _GameObject = UnityEngine.Object.Instantiate(CraftData.GetPrefabForTechType(PrefabTemplate));
                }
                catch
                {
                    _GameObject = UnityEngine.Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                }
            }
            else
            {
                try
                {
                    _GameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(GameResourceFileName));
                }
                catch
                {
                    _GameObject = UnityEngine.Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                }
            }

            _GameObject.name = NameID;

            _GameObject.AddComponent<SelfDestruct>();

            return _GameObject;
        }
        
        public Atlas.Sprite GetResourceIcon(TechType techType)
        {
            return SpriteManager.Get(techType);
        }
    }
}
