using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SeamothArms.API;
using Common.Helpers.SMLHelpers;

namespace SeamothArmAPITest.Prefabs
{
    internal class NewGrapplingArmPrefab : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewGrapplingArmPrefab()
           : base(
                 techTypeName: "NewSeamothGrapplingArmModule",
                 friendlyName: "API based Seamoth Grappling Arm",
                 description: "This is the new API based Seamoth Grappling Arm.",                 
                 armTemplate: ArmTemplate.GrapplingArm,
                 requiredForUnlock: TechType.ExosuitGrapplingArmModule,
                 fragment: null
                 )
        {
        }

        protected override void PostPatch()
        {
            TechTypeID = TechType;
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                    new Ingredient(TechType.ExosuitGrapplingArmModule, 1)
                })
            };
        }
                
        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override void ModifyGameObject()
        {
            return;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitGrapplingArmModule);
        }
    }
}
