using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;
using SMLHelper.V2.Handlers;
using SMLExpander;
using System.Collections;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothPropulsionArmPrefab : CraftableModdedArm
    {
        internal SeamothPropulsionArmPrefab(SpawnableArmFragment fragment)
           : base(
                 techTypeName: "SeamothPropulsionArmModule",
                 friendlyName: "Seamoth Propulsion Arm",
                 description: "Allows Seamoth to use Propulsion Arm.",
                 armType: ArmType.SeamothArm,
                 armTemplate: ArmTemplate.PropulsionArm,
                 requiredForUnlock: TechType.None,
                 fragment: fragment
                 )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new SeamothPropulsionArmModdingRequest());
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

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitPropulsionArmModule);
        }

        protected override void SetCustomLanguageText()
        {
            LanguageHandler.Main.SetLanguageLine("SeamothPropulsionCannonNoItems", "No suitable items found in seamoth storage containers.");
            LanguageHandler.Main.SetLanguageLine("SeamothPropulsionCannonNoContainers", "This seamoth does not contain storage container(s).");
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
    }
}