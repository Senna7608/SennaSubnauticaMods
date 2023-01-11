using ModdedArmsHelper.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothTorpedoArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothTorpedoArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothTorpedoArmFragment",
                  friendlyName: "Seamoth torpedo arm fragment",
                  fragmentTemplate: ArmTemplate.TorpedoArm,
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
                new LootDistributionData.BiomeData() { biome = BiomeType.UnderwaterIslands_IslandTop, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.UnderwaterIslands_TechSite_Scatter, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.BloodKelp_TechSite_Scatter, count = 1, probability = 0.08f }                
            };
        }

        protected override Sprite GetUnlockSprite()
        {
            return GetUnitySprite(SpriteManager.Get(TechType.ExosuitTorpedoArmModule));
        }

        protected override IEnumerator PostModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}