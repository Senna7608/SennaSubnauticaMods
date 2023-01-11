using Common;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UWE;

namespace SMLExpander
{
    internal abstract class ModPrefab_Craftable : ModPrefab
    {
        protected readonly string TechTypeName;
        protected readonly string FriendlyName;
        protected readonly string Description;
        public GameObject GameObjectClone { get; set; } = null;
        public Atlas.Sprite ItemSprite { get; private set; }

        protected readonly TechType PrefabTemplate;
        protected readonly string GameResourceFileName;

        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;
        protected readonly EquipmentType TypeForEquipment;
        protected readonly QuickSlotType TypeForQuickslot;
        protected readonly CraftData.BackgroundType BackgroundType;
        protected readonly Vector2int ItemSize;
        protected readonly ModPrefab_Fragment _Fragment;
        
        private bool isEncyExists = false;

        protected ModPrefab_Craftable(
            string techTypeName,
            string friendlyName,
            string description,
            TechType template,
            string gamerResourceFileName,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            EquipmentType equipmentType,
            QuickSlotType quickSlotType,
            CraftData.BackgroundType backgroundType,
            Vector2int itemSize,
            ModPrefab_Fragment fragment
            )
            : base(techTypeName, $"{techTypeName}.prefab")
        {
            TechTypeName = techTypeName;
            FriendlyName = friendlyName;
            Description = description;
            PrefabTemplate = template;
            GameResourceFileName = gamerResourceFileName;
            RequiredForUnlock = requiredAnalysis;
            GroupForPDA = groupForPDA;
            CategoryForPDA = categoryForPDA;
            TypeForEquipment = equipmentType;
            TypeForQuickslot = quickSlotType;
            BackgroundType = backgroundType;
            ItemSize = itemSize;
            _Fragment = fragment;

            //IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
        }
        
        /*
        private void OnQuitEvent()
        {
            Patch();
        }        
        */

        public void Patch()
        {
            TechType = TechTypeHandler.Main.AddTechType(TechTypeName, FriendlyName, Description, (Atlas.Sprite)null, false);

            PrePatch();

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

            ItemSprite = GetItemSprite();
            SpriteHandler.Main.RegisterSprite(TechType, ItemSprite);

            PrefabHandler.Main.RegisterPrefab(this);
            CraftDataHandler.Main.SetTechData(TechType, GetRecipe());
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
                    kind = PDAEncyclopedia.EntryData.Kind.Encyclopedia,
                    unlocked = false,
                    popup = _Fragment != null ? _Fragment.UnlockSprite : GetUnitySprite(ItemSprite),
                    image = encyData.image,
                    sound = null,
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

                KnownTechHandler.Main.SetAnalysisTechEntry(TechType, new TechType[1] { TechType }, _Fragment.UnlockSprite);
            }
            else
            {
                KnownTechHandler.Main.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
            }

            TabNode NewTabNode = GetTabNodeData();

            if (NewTabNode != null)
            {
                CraftTreeHandler.Main.AddTabNode(NewTabNode.craftTree, NewTabNode.uniqueName, NewTabNode.displayName, NewTabNode.sprite);
            }

            foreach (CraftTreeType craftTreeType in GetCraftTreeTypesData().TreeTypes)
            {
                CraftTreeHandler.Main.AddCraftingNode(craftTreeType.TreeType, TechType, craftTreeType.StepsToTab);
            }

            SNLogger.Log($"{TechTypeName}: Internal Patch completed.");
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            SNLogger.Debug($"GetGameObjectAsync started for TechType: [{TechTypeName}]...");

            if (GameObjectClone != null)
            {
                gameObject.Set(GameObjectClone);
                yield break;
            }

            if (PrefabTemplate != TechType.None)
            {
                CoroutineTask<GameObject> getPrefabrequest = CraftData.GetPrefabForTechTypeAsync(PrefabTemplate);
                yield return getPrefabrequest;

                GameObject result = getPrefabrequest.GetResult();

                if (result == null)
                {
                    SNLogger.Error($"{TechTypeName}: Cannot instantiate prefab from CraftData!");
                    yield break;
                }

                GameObjectClone = Object.Instantiate(result);
            }
            else if (GameResourceFileName != null)
            {
                AsyncOperationHandle<GameObject> loadRequest = AddressablesUtility.LoadAsync<GameObject>(GameResourceFileName);

                yield return loadRequest;

                if (loadRequest.Status == AsyncOperationStatus.Failed)
                {
                    SNLogger.Error($"GameObject cannot be loaded from this location: [{GameResourceFileName}]");
                    yield break;
                }

                GameObject loadPrefab = loadRequest.Result;

                GameObjectClone = UWE.Utils.InstantiateDeactivated(loadPrefab, null, Vector3.zero, Quaternion.identity);
            }

            TaskResult<bool> modifyResult = new TaskResult<bool>();
            CoroutineTask<bool> modifyrequest = new CoroutineTask<bool>(ModifyGameObjectAsync(modifyResult), modifyResult);
            yield return modifyrequest;

            if (!modifyrequest.GetResult())
            {
                SNLogger.Error($"ModifyGameObjectAsync failed for TechType: [{TechTypeName}]");
                yield break;
            }

            gameObject.Set(GameObjectClone);

            SNLogger.Debug($"GetGameObjectAsync completed for TechType: [{TechTypeName}]");

            yield break;
        }

        protected abstract TechData GetRecipe();

        protected abstract EncyData GetEncyclopediaData();

        protected abstract CrafTreeTypesData GetCraftTreeTypesData();

        protected abstract TabNode GetTabNodeData();        

        protected abstract IEnumerator ModifyGameObjectAsync(IOut<bool> success);        

        protected abstract void PrePatch();       

        protected abstract Atlas.Sprite GetItemSprite();

        public Sprite GetUnitySprite(Atlas.Sprite sprite)
        {
            return Sprite.Create(sprite.texture,
                                 new Rect(0.0f, 0.0f, sprite.texture.width, sprite.texture.height),
                                 new Vector2(sprite.texture.width * 0.5f, sprite.texture.height * 0.5f));
        }
        
    }
}
