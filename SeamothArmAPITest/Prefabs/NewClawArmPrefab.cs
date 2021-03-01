using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SeamothArms.API;
using Common.Helpers.SMLHelpers;

namespace SeamothArmAPITest.Prefabs
{
    public class NewClawArmPrefab : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewClawArmPrefab()
            : base(
                  techTypeName: "NewSeamothClawArmModule",
                  friendlyName: "API based Seamoth Claw Arm",
                  description: "This is the new API based Seamoth Claw Arm.",                  
                  armTemplate: ArmTemplate.ClawArm,
                  requiredForUnlock: TechType.Exosuit,
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
                    new Ingredient(TechType.TitaniumIngot, 2)                    
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
            return SpriteManager.Get(TechType.ExosuitClawArmModule);
        }
    }
}
