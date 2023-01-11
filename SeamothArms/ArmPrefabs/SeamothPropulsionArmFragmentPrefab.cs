using ModdedArmsHelper.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothPropulsionArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothPropulsionArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothPropulsionArmFragment",
                  friendlyName: "Seamoth propulsion arm fragment",
                  fragmentTemplate: ArmTemplate.PropulsionArm,
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
                new LootDistributionData.BiomeData() { biome = BiomeType.Mountains_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.Mountains_Rock, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.KooshZone_Sand, count = 1, probability = 0.08f }                
            };
        }

        protected override Sprite GetUnlockSprite()
        {
            return GetUnitySprite(SpriteManager.Get(TechType.ExosuitPropulsionArmModule));
        }

        protected override IEnumerator PostModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}