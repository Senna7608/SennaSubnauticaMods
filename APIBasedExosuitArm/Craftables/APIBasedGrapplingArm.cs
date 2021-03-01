using APIBasedExosuitArms.ArmModdingRequest;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SMLHelper.V2.Crafting;
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

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new APIBasedGrapplingArmModdingRequest());
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

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override void ModifyGameObject()
        {
        }

        protected override void SetCustomLanguageText()
        {            
        }
    }
}
