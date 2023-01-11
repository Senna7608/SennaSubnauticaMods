using ModdedArmsHelper.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothGrapplingArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothGrapplingArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothGrapplingArmFragment",
                  friendlyName: "Seamoth grappling arm fragment",
                  fragmentTemplate: ArmTemplate.GrapplingArm,
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
                new LootDistributionData.BiomeData() { biome = BiomeType.CragField_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.CragField_Ground, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.CragField_Rock, count = 1, probability = 0.08f }                
            };
        }

        protected override Sprite GetUnlockSprite()
        {
            return GetUnitySprite(SpriteManager.Get(TechType.ExosuitGrapplingArmModule));
        }

        protected override IEnumerator PostModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}