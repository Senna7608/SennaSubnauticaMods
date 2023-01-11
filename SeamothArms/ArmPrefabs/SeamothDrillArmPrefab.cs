using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;
using SMLExpander;
using System.Collections;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothDrillArmPrefab : CraftableModdedArm
    {
        public static TechType TechTypeID { get; private set; }

        internal SeamothDrillArmPrefab(SpawnableArmFragment fragment)
           : base(
                 techTypeName: "SeamothDrillArmModule",
                 friendlyName: "Seamoth drill arm",
                 description: "Enables the mining of large resource deposits.",
                 armType: ArmType.SeamothArm,
                 armTemplate: ArmTemplate.DrillArm,
                 requiredForUnlock: TechType.None,
                 fragment: fragment
                 )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new SeamothDrillArmModdingRequest());
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

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitDrillArmModule);
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }

        protected override void SetCustomLanguageText()
        {
        }
    }
}
