using ModdedArmsHelper.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothClawArmFragmentPrefab : SpawnableArmFragment
    {
        internal SeamothClawArmFragmentPrefab()
            : base(
                  techTypeName: "SeamothClawArmFragment",
                  friendlyName: "Seamoth claw arm fragment",                  
                  fragmentTemplate: ArmTemplate.ClawArm,
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
                new LootDistributionData.BiomeData() { biome = BiomeType.CrashZone_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.Kelp_Sand, count = 1, probability = 0.08f },
                new LootDistributionData.BiomeData() { biome = BiomeType.GrassyPlateaus_Sand, count = 1, probability = 0.08f }                
            };
        }
        
        protected override Sprite GetUnlockSprite()
        {
            return GetUnitySprite(SpriteManager.Get(TechType.ExosuitClawArmModule));
        }

        protected override IEnumerator PostModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }
}
