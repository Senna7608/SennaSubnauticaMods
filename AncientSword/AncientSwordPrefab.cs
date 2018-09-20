using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using UnityEngine;

namespace AncientSword
{
    internal class AncientSwordPrefab : Craftable
    {
        public static TechType TechTypeID { get; private set; }

        public static GameObject sword;
        public static GameObject knife;

        internal AncientSwordPrefab()
            : base(nameID: "AncientSword",
                  friendlyName: "Ancient Sword", //Language.main.Get(TechTypeExtensions.AsString(TechType.PrecursorPrisonArtifact8, false)),
                  description: "An ancient sword from Earth.\nFound in an ancient Precursor facility.",
                  template: TechType.Knife,
                  fabricatorType: CraftTree.Type.Fabricator,
                  fabricatorTab: new string[] { "Personal", "Tools"},
                  requiredAnalysis: TechType.None, //TechType.PrecursorPrisonArtifact8,
                  groupForPDA: TechGroup.Personal,
                  categoryForPDA: TechCategory.Tools)
        {
        }

        public override void Patch()
        {
            base.Patch();
            

            TechTypeID = TechType;
            sword = Resources.Load<GameObject>("worldentities/doodads/precursor/prison/relics/alien_relic_08");
            knife = Resources.Load<GameObject>("WorldEntities/Tools/Knife");
            knife.AddComponent<AncientSword>();
            MeshRenderer knifeRenderer = knife.GetComponentInChildren<MeshRenderer>(true);
            MeshRenderer swordRenederer = sword.GetComponentInChildren<MeshRenderer>(true);

            Debug.Log($"[AncientSword] resource name: {sword.name}");
            Debug.Log($"[AncientSword] knifeMesh name: {knifeRenderer.name}");           
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[2]
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Copper, 1)
                })
            };
        }
    }
}
