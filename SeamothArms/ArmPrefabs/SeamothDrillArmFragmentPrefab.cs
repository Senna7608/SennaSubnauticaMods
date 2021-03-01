using ModdedArmsHelper.API;
using System.Collections.Generic;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothDrillArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothDrillArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothDrillArmFragment",
                  friendlyName: "Seamoth drill arm fragment",
                  fragmentTemplate: ArmTemplate.DrillArm,                  
                  cellLevel: LargeWorldEntity.CellLevel.Near,
                  scanTime: 5,
                  totalFragments: 2                  
                  )
        {
        }

        protected override List<LootDistributionData.BiomeData> GetBiomeDatas()
        {
            return new List<LootDistributionData.BiomeData>()
            {
                new LootDistributionData.BiomeData() { biome = BiomeType.JellyShroomCaves_AbandonedBase_Outside, count = 1, probability =   0.1f },
                new LootDistributionData.BiomeData() { biome = BiomeType.CrashZone_Sand, count = 1, probability =   0.1f },
                new LootDistributionData.BiomeData() { biome = BiomeType.GrassyPlateaus_Grass, count = 1, probability =   0.1f },
                new LootDistributionData.BiomeData() { biome = BiomeType.MushroomForest_Grass, count = 1, probability =   0.1f },
                new LootDistributionData.BiomeData() { biome = BiomeType.MushroomForest_Sand, count = 1, probability =   0.1f },
            };
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitClawArmModule);
        }        

        protected override void PostModify()
        {
            return;
        }
    }
}