using APIBasedExosuitArms.ArmModdingRequest;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SMLHelper.V2.Crafting;
using System.Collections.Generic;

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

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new APIBasedDrillArmModdingRequest());
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
