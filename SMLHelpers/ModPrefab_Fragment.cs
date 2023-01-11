using Common;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UWE;
#pragma warning disable CS1591 //XML documentation


namespace SMLExpander
{
    public abstract class ModPrefab_Fragment : ModPrefab
    {
        protected readonly string TechTypeName;
        public readonly string VirtualPrefabFilename;
        protected readonly string FriendlyName;
        protected readonly TechType FragmentTemplate;
        protected readonly string PrefabFilePath;
        protected readonly EntitySlot.Type SlotType;
        protected readonly bool PrefabZUp;
        protected readonly LargeWorldEntity.CellLevel CellLevel;
        protected readonly Vector3 LocalScale;
        public readonly float ScanTime;
        public readonly int TotalFragments;
        public readonly bool DestroyAfterScan;

        public GameObject GameObjectClone { get; set; } = null;
        public Sprite UnlockSprite { get; private set; } = null;        

        protected ModPrefab_Fragment(
            string techTypeName,
            string friendlyName,
            TechType template,
            string prefabFilePath,
            EntitySlot.Type slotType,
            bool prefabZUp,
            LargeWorldEntity.CellLevel cellLevel,
            Vector3 localScale,
            float scanTime = 2,
            int totalFragments = 2,
            bool destroyAfterScan = true
            )
            : base(techTypeName, $"{techTypeName}.prefab")
        {
            TechTypeName = techTypeName;
            FriendlyName = friendlyName;
            FragmentTemplate = template;
            PrefabFilePath = prefabFilePath;
            SlotType = slotType;
            PrefabZUp = prefabZUp;
            CellLevel = cellLevel;
            LocalScale = localScale;
            ScanTime = scanTime;
            TotalFragments = totalFragments;
            DestroyAfterScan = destroyAfterScan;
            VirtualPrefabFilename = $"{techTypeName}.prefab";
        }

        public void Patch()
        {
            TechType = TechTypeHandler.Main.AddTechType(TechTypeName, FriendlyName, string.Empty, (Sprite)null, false);

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

            UnlockSprite = GetUnlockSprite();

            SpriteHandler.Main.RegisterSprite(TechType, UnlockSprite);

            PrefabHandler.Main.RegisterPrefab(this);

            LootDistributionData.SrcData srcData = new LootDistributionData.SrcData()
            {
                prefabPath = VirtualPrefabFilename,
                distribution = GetBiomeDatas()
            };

            LootDistributionHandler.Main.AddLootDistributionData(ClassID, srcData);

            WorldEntityInfo EntityInfo = new WorldEntityInfo()
            {
                classId = ClassID,
                techType = TechType,
                slotType = SlotType,
                prefabZUp = PrefabZUp,
                cellLevel = CellLevel,
                localScale = LocalScale
            };

            WorldEntityDatabaseHandler.Main.AddCustomInfo(ClassID, EntityInfo);            

            SNLogger.Log($"{TechTypeName}: Internal Patch completed.");
        }        

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            SNLogger.Debug($"GetGameObjectAsync started for fragment: [{TechTypeName}]...");

            if (GameObjectClone != null)
            {
                gameObject.Set(GameObjectClone);
                yield break;
            }

            GameObject result;

            if (!string.IsNullOrEmpty(PrefabFilePath))
            {
                SNLogger.Debug($"Prefab filename request (async) started for fragment: [{PrefabFilePath}]...");

                IPrefabRequest prefabRequest = PrefabDatabase.GetPrefabForFilenameAsync(PrefabFilePath);
                yield return prefabRequest;

                if (prefabRequest.TryGetPrefab(out result))
                {
                    GameObjectClone = UWE.Utils.InstantiateDeactivated(result);
                }
                else
                {
                    SNLogger.Error($"Cannot find prefab in PrefabDatabase at path: [{PrefabFilePath}]");
                    yield break;
                }
            }
            else if (FragmentTemplate != TechType.None)
            {
                SNLogger.Debug($"Prefab template request (async) started for fragment: [{FragmentTemplate}]...");

                CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(FragmentTemplate);
                yield return request;

                result = request.GetResult();

                if (result == null)
                {
                    SNLogger.Error($"Cannot instantiate prefab from TechType: [{FragmentTemplate}]");
                    yield break;
                }

                GameObjectClone = UWE.Utils.InstantiateDeactivated(result);
            }

            GameObjectClone.name = TechTypeName;

            PrefabIdentifier prefabIdentifier = GameObjectClone.GetComponent<PrefabIdentifier>();
            prefabIdentifier.ClassId = TechTypeName;

            TechTag techTag = GameObjectClone.EnsureComponent<TechTag>();
            techTag.type = TechType;

            ResourceTracker resourceTracker = GameObjectClone.GetComponent<ResourceTracker>();
            resourceTracker.overrideTechType = TechType.Fragment;

            TaskResult<bool> modifyResult = new TaskResult<bool>();
            CoroutineTask<bool> modifyrequest = new CoroutineTask<bool>(ModifyGameObjectAsync(modifyResult), modifyResult);
            yield return modifyrequest;

            if (!modifyrequest.GetResult())
            {
                SNLogger.Error($"ModifyGameObjectAsync failed for fragment: [{TechTypeName}]");
                yield break;
            }

            AddFragmentTracker(GameObjectClone);

            SNLogger.Debug($"GetGameObjectAsync completed for fragment: [{TechTypeName}]");

            gameObject.Set(GameObjectClone);
            yield break;
        }

        [Conditional("DEBUG")]
        private static void AddFragmentTracker(GameObject gameObject)
        {
            gameObject.AddComponent<FragmentTracker>();
        }
        
        protected abstract IEnumerator ModifyGameObjectAsync(IOut<bool> success);

        protected abstract List<LootDistributionData.BiomeData> GetBiomeDatas();

        protected abstract Sprite GetUnlockSprite();        

        public Sprite GetUnitySprite(Atlas.Sprite atlasSprite)
        {
            return Sprite.Create(atlasSprite.texture,
                                 new Rect(0.0f, 0.0f, atlasSprite.texture.width, atlasSprite.texture.height),
                                 new Vector2(atlasSprite.texture.width * 0.5f, atlasSprite.texture.height * 0.5f));
        }
    }   
}