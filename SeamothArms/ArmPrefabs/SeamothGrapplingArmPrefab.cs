using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;
using SMLExpander;
using System.Collections;

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
                 requiredForUnlock: TechType.None,
                 fragment: fragment                 
                 )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new SeamothGrapplingArmModdingRequest());
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
