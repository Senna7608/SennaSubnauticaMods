using ModdedArmsHelper.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothDrillArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothDrillArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothDrillArmFragment",
                  friendlyName: "Seamoth drill arm fragment",
                  fragmentTemplate: ArmTemplate.DrillArm,
                  prefabFilePath: null,
                  scanTime: 5,
                  totalFragments: 3                  
                  )
        {
        }

        protected override List<LootDistributionData.BiomeData> GetBiomeDatas()
        {
            return new List<LootDistributionData.BiomeData>()
            {                                
                new LootDistributionData.BiomeData() { biome = BiomeType.MushroomForest_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.GrassyPlateaus_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.Dunes_SandPlateau, count = 1, probability = 0.08f },
            };
        }

        protected override Sprite GetUnlockSprite()
        {
            return GetUnitySprite(SpriteManager.Get(TechType.ExosuitDrillArmModule));
        }

        protected override IEnumerator PostModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}