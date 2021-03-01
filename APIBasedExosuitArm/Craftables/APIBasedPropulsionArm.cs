using APIBasedExosuitArms.ArmModdingRequest;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SMLHelper.V2.Crafting;
using System.Collections.Generic;

namespace APIBasedExosuitArms.Craftables
{
    internal class APIBasedPropulsionArm : CraftableModdedArm
    {
        internal APIBasedPropulsionArm()
            : base(
                  techTypeName: "APIBasedPropulsionArm",
                  friendlyName: "API Based Exosuit Propulsion Arm",
                  description: "Allows Exosuit to use new modded propulsion arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.PropulsionArm,
                  requiredForUnlock: TechType.Exosuit,
                  fragment: null
                  )
        {
        }

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new APIBasedPropulsionArmModdingRequest());
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitPropulsionArmModule);
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Magnetite, 2),
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
