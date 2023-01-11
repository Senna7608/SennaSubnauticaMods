using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;
using SMLExpander;
using System.Collections;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothClawArmPrefab : CraftableModdedArm
    {
        internal SeamothClawArmPrefab(SpawnableArmFragment fragment)
            : base(
                  techTypeName: "SeamothClawArmModule",
                  friendlyName: "Seamoth claw arm",
                  description: "Allows Seamoth to use claw arm.",
                  armType: ArmType.SeamothArm,
                  armTemplate: ArmTemplate.ClawArm,
                  requiredForUnlock: TechType.None,
                  fragment: fragment
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new SeamothClawArmModdingRequest());
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[3]
                {
                    new Ingredient(TechType.TitaniumIngot, 1),
                    new Ingredient(TechType.WiringKit, 1),
                    new Ingredient(TechType.ComputerChip, 1)
                })
            };
        }        

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitClawArmModule);
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }

        protected override void SetCustomLanguageText()
        {
        }        
    }
}
