using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;

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

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new SeamothClawArmModdingRequest());
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

        protected override void ModifyGameObject()
        {
        }

        protected override void SetCustomLanguageText()
        {
        }
    }
}
