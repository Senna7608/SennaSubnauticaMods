using APIBasedExosuitArms.ArmModdingRequest;
using ModdedArmsHelper.API;
using SMLExpander;
using SMLHelper.V2.Crafting;
using System.Collections;
using System.Collections.Generic;

namespace APIBasedExosuitArms.Craftables
{
    internal class APIBasedClawArm : CraftableModdedArm
    {
        internal APIBasedClawArm()
            : base(
                  techTypeName: "APIBasedClawArm",
                  friendlyName: "API Based Exosuit Claw Arm",
                  description: "Allows Exosuit to use new modded claw arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.ClawArm,
                  requiredForUnlock: TechType.Exosuit,
                  fragment: null
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new APIBasedClawArmModdingRequest());
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override void SetCustomLanguageText()
        {
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitClawArmModule);
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[2]
                {
                    new Ingredient(TechType.TitaniumIngot, 1),
                    new Ingredient(TechType.ComputerChip, 1)
                })
            };
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {            
            success.Set(true);
            yield break;
        }
    }
}
