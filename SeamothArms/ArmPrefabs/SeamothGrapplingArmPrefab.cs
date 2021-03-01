using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothGrapplingArmPrefab : CraftableModdedArm
    {
        internal SeamothGrapplingArmPrefab(SpawnableArmFragment fragment)
           : base(
                 techTypeName: "SeamothGrapplingArmModule",
                 friendlyName: "Seamoth Grappling Arm",
                 description: "Fires a grappling hook for enhanced environment traversal.",
                 armType: ArmType.SeamothArm,
                 armTemplate: ArmTemplate.GrapplingArm,
                 requiredForUnlock: TechType.ExosuitGrapplingArmModule,
                 fragment: fragment                 
                 )
        {
        }

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new SeamothGrapplingArmModdingRequest());
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

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitGrapplingArmModule);
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
