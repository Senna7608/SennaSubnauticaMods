using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SeamothArms.API;
using Common.Helpers.SMLHelpers;

namespace SeamothArmAPITest.Prefabs
{
    public class NewDrillArmPrefab : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewDrillArmPrefab()
            : base(techTypeName: "NewSeamothDrillArmModule",
                  friendlyName: "API based Seamoth Drill Arm",
                  description: "This is the new API based Seamoth Drill Arm.",                  
                  armTemplate: ArmTemplate.DrillArm,
                  requiredForUnlock: TechType.ExosuitDrillArmModule,
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
                    new Ingredient(TechType.ExosuitDrillArmModule, 1)                    
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
            return SpriteManager.Get(TechType.ExosuitDrillArmModule);
        }
    }
}
