using APIBasedExosuitArms.ArmModdingRequest;
using ModdedArmsHelper.API;
using SMLExpander;
using SMLHelper.V2.Crafting;
using System.Collections;
using System.Collections.Generic;

namespace APIBasedExosuitArms.Craftables
{
    internal class APIBasedGrapplingArm : CraftableModdedArm
    {
        internal APIBasedGrapplingArm()
            : base(
                  techTypeName: "APIBasedGrapplingArm",
                  friendlyName: "API Based Exosuit Grappling Arm",
                  description: "Allows Exosuit to use new modded grappling arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.GrapplingArm,
                  requiredForUnlock: TechType.Exosuit,
                  fragment: null
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new APIBasedGrapplingArmModdingRequest());
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
            return SpriteManager.Get(TechType.ExosuitGrapplingArmModule);
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.Benzene, 1),
                    new Ingredient(TechType.Titanium, 2),
                    new Ingredient(TechType.Lithium, 1)
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
