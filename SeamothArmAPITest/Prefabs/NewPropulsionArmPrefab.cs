using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SeamothArms.API;
using Common.Helpers.SMLHelpers;

namespace SeamothArmAPITest.Prefabs
{
    internal class NewPropulsionArmPrefab : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewPropulsionArmPrefab()
           : base(
                 techTypeName: "NewSeamothPropulsionArmModule",
                 friendlyName: "API based Seamoth Propulsion Arm",
                 description: "This is the new API based Seamoth Propulsion Arm.",                 
                 armTemplate: ArmTemplate.PropulsionArm,
                 requiredForUnlock: TechType.ExosuitPropulsionArmModule,
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
                    new Ingredient(TechType.ExosuitPropulsionArmModule, 1)
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
            return SpriteManager.Get(TechType.ExosuitPropulsionArmModule);
        }
    }
}
