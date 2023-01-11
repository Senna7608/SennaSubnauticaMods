using APIBasedExosuitArms.ArmModdingRequest;
using ModdedArmsHelper.API;
using SMLExpander;
using SMLHelper.V2.Crafting;
using System.Collections;
using System.Collections.Generic;

namespace APIBasedExosuitArms.Craftables
{
    internal class APIBasedTorpedoArm : CraftableModdedArm
    {
        internal APIBasedTorpedoArm()
            : base(
                  techTypeName: "APIBasedTorpedoArm",
                  friendlyName: "API Based Exosuit Torpedo Arm",
                  description: "Allows Exosuit to use new modded torpedo arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.TorpedoArm,
                  requiredForUnlock: TechType.Exosuit,
                  fragment: null
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new APIBasedTorpedoArmModdingRequest());
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override void SetCustomLanguageText()
        {
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.ExosuitTorpedoArmModule);
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
            success.Set(true);
            yield break;
        }
    }
}
