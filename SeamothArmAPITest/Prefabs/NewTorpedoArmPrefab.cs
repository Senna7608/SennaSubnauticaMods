using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SeamothArms.API;
using Common.Helpers.SMLHelpers;

namespace SeamothArmAPITest.Prefabs
{
    internal class NewTorpedoArmPrefab : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewTorpedoArmPrefab()
           : base(
                 techTypeName: "NewSeamothTorpedoArmModule",
                 friendlyName: "API based Seamoth Torpedo Arm",
                 description: "This is the new API based Seamoth Torpedo Arm.",                 
                 armTemplate: ArmTemplate.TorpedoArm,
                 requiredForUnlock: TechType.ExosuitTorpedoArmModule,
                 fragment: null
                 )
        {
        }

        public override void Patch()
        {
            base.Patch();
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
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 1)                    
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
            return SpriteManager.Get(TechType.ExosuitTorpedoArmModule);
        }
    }
}
