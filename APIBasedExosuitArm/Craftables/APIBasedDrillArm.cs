using APIBasedExosuitArms.ArmModdingRequest;
using ModdedArmsHelper.API;
using SMLHelper.V2.Crafting;
using System.Collections;
using System.Collections.Generic;
using SMLExpander;

namespace APIBasedExosuitArms.Craftables
{
    internal class APIBasedDrillArm : CraftableModdedArm
    {
        internal APIBasedDrillArm()
            : base(
                  techTypeName: "APIBasedDrillArm",
                  friendlyName: "API Based Exosuit Drill Arm",
                  description: "Allows Exosuit to use new modded drill arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.DrillArm,
                  requiredForUnlock: TechType.Exosuit,
                  fragment: null
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new APIBasedDrillArmModdingRequest());
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
            return SpriteManager.Get(TechType.ExosuitDrillArmModule);
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[3]
                {
                    new Ingredient(TechType.Titanium, 5),
                    new Ingredient(TechType.Lithium, 1),
                    new Ingredient(TechType.Diamond, 4)
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
