using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Common.Helpers.SMLHelpers
{
    public abstract class ModPrefab_Craftable : ModPrefab
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
        protected readonly ModPrefab_Fragment _Fragment;

        private bool isEncyExists = false;

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
        protected ModPrefab_Craftable(
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
            string gamerResourceFileName,
            ModPrefab_Fragment fragment
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
            _Fragment = fragment;

            //IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
        }

        private void OnQuitEvent()
        {
            Patch();
        }

        public virtual void Patch()
        {
            Atlas.Sprite atlasSprite = null;            

            if (IconFilePath != null)
            {
                try
                {
                    atlasSprite = ImageUtils.LoadSpriteFromFile(IconFilePath);
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
                    atlasSprite = GetResourceIcon(IconTechType);
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
                    atlasSprite = GetResourceIcon(PrefabTemplate);
                }
                catch
                {
                    SNLogger.Error(NameID, $"Resource template icon [{PrefabTemplate.ToString()}] not Found!");
                }
            }

            TechType = TechTypeHandler.Main.AddTechType(NameID, FriendlyName, Description, atlasSprite, false);

            PrefabHandler.Main.RegisterPrefab(this);
            CraftDataHandler.Main.SetTechData(TechType, GetRecipe());
            SpriteHandler.Main.RegisterSprite(TechType, atlasSprite);
            CraftDataHandler.Main.SetItemSize(TechType, ItemSize);
            CraftDataHandler.Main.AddToGroup(GroupForPDA, CategoryForPDA, TechType);
            CraftDataHandler.Main.SetEquipmentType(TechType, TypeForEquipment);
            CraftDataHandler.Main.SetQuickSlotType(TechType, TypeForQuickslot);
            CraftDataHandler.Main.SetBackgroundType(TechType, BackgroundType);

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
                    popup = GetUnitySprite(atlasSprite),
                    image = encyData.image,
                    audio = null,                   
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

                KnownTechHandler.Main.SetAnalysisTechEntry(TechType, new TechType[1] { TechType }, GetUnitySprite(_Fragment.UnlockSprite));
            }
            else
            {
                KnownTechHandler.Main.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
            }

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
        }

        protected abstract TechData GetRecipe();

        protected abstract EncyData GetEncyclopediaData();

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
                    SNLogger.Warn(NameID, "Cannot instantiate prefab from TechType!");
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
                    SNLogger.Warn(NameID, "Cannot instantiate prefab from resource!");
                }
            }

            _GameObject.name = NameID;

            return _GameObject;
        }

        public Atlas.Sprite GetResourceIcon(TechType techType)
        {
            return SpriteManager.Get(techType);
        }

        public Sprite GetUnitySprite(Atlas.Sprite atlasSprite)
        {
            return  Sprite.Create(atlasSprite.texture, new Rect(0.0f, 0.0f, atlasSprite.texture.width, atlasSprite.texture.height), new Vector2(atlasSprite.texture.width * 0.5f, atlasSprite.texture.height * 0.5f));
        }
    }
}
