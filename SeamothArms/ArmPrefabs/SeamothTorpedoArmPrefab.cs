using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using ModdedArmsHelper.API;
using SeamothArms.ArmHandlerRequesters;
using SMLExpander;
using System.Collections;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothTorpedoArmPrefab : CraftableModdedArm
    {
        internal SeamothTorpedoArmPrefab(SpawnableArmFragment fragment)
           : base(
                 techTypeName: "SeamothTorpedoArmModule",
                 friendlyName: "Seamoth torpedo arm",
                 description: "A standard payload delivery system adapted to fire torpedoes.",
                 armType: ArmType.SeamothArm,
                 armTemplate: ArmTemplate.TorpedoArm,
                 requiredForUnlock: TechType.None,
                 fragment: fragment
                 )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new SeamothTorpedoArmModdingRequest());
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
                    new Ingredient(TechType.Aerogel, 1)
                })
            };
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            SeamothStorageContainer container = PrefabClone.GetComponent<SeamothStorageContainer>();

            container.width = 4;
            container.height = 4;

            success.Set(true);
            yield break;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitTorpedoArmModule);
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }        

        protected override void SetCustomLanguageText()
        {
        }
    }
}