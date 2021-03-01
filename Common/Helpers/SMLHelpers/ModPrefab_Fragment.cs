using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace Common.Helpers.SMLHelpers
{
    public abstract class ModPrefab_Fragment : ModPrefab
    {
        protected readonly string TechTypeName;
        protected readonly string FriendlyName;
        protected readonly TechType FragmentTemplate;
        protected readonly EntitySlot.Type SlotType;
        protected readonly bool PrefabZUp;
        protected readonly LargeWorldEntity.CellLevel CellLevel;
        protected readonly Vector3 LocalScale;
        public readonly float ScanTime;
        public readonly int TotalFragments;
        public readonly bool DestroyAfterScan;

        public GameObject GameObjectClone { get; private set; }
        public Atlas.Sprite UnlockSprite { get; private set; }

        protected ModPrefab_Fragment(
            string techTypeName,
            string friendlyName,
            TechType template,
            EntitySlot.Type slotType,
            bool prefabZUp,
            LargeWorldEntity.CellLevel cellLevel,
            Vector3 localScale,
            float scanTime = 2,
            int totalFragments = 2,
            bool destroyAfterScan = true
            )
            : base(techTypeName, $"{techTypeName}_Prefab")
        {
            TechTypeName = techTypeName;
            FriendlyName = friendlyName;
            FragmentTemplate = template;
            SlotType = slotType;
            PrefabZUp = prefabZUp;
            CellLevel = cellLevel;
            LocalScale = localScale;
            ScanTime = scanTime;
            TotalFragments = totalFragments;
            DestroyAfterScan = destroyAfterScan;
        }

        public void Patch()        
        {
            UnlockSprite = GetItemSprite();

            TechType = TechTypeHandler.Main.AddTechType(TechTypeName, FriendlyName, string.Empty, UnlockSprite, false);

            PrefabHandler.Main.RegisterPrefab(this);

            LootDistributionData.SrcData srcData = new LootDistributionData.SrcData()
            {
                prefabPath = $"{ClassID}_Prefab",
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
        }

        public override GameObject GetGameObject()
        {
            GameObjectClone = CraftData.InstantiateFromPrefab(FragmentTemplate);           

            ResourceTracker resourceTracker = GameObjectClone.GetComponent<ResourceTracker>();
            resourceTracker.overrideTechType = TechType.Fragment;

            //GameObjectClone.EnsureComponent<FragmentTracker>();

            ModifyGameObject();

            return GameObjectClone;
        }

        protected abstract void ModifyGameObject();

        protected abstract List<LootDistributionData.BiomeData> GetBiomeDatas();

        protected abstract Atlas.Sprite GetItemSprite();
    }
}