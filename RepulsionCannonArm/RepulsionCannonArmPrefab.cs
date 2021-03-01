using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common.Helpers.SMLHelpers;
using ModdedArmsHelper.API;
using SMLHelper.V2.Utility;

namespace RepulsionCannonArm
{
    internal class RepulsionCannonArmPrefab : CraftableModdedArm
    {
        internal RepulsionCannonArmPrefab()
            : base(
                  techTypeName: "ExosuitRepulsionCannonArm",                  
                  friendlyName: "P.R.A.W.N Repulsion Cannon Arm",
                  description: "Allows P.R.A.W.N suit to use Repulsion Cannon Arm.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.PropulsionArm,
                  requiredForUnlock: TechType.ExosuitPropulsionArmModule,
                  fragment: null                  
                  )
        {
        }

        protected override void RegisterArm()
        {
            ArmServices.main.RegisterArm(this, new RepulsionCannonArmModdingRequest());
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
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/RepulsionCannonArm.png");
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[3]
                {
                    new Ingredient(TechType.ExosuitPropulsionArmModule, 1),                                        
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.ComputerChip, 1)
                })
            };
        }


        protected override void ModifyGameObject()
        {            
            GameObject model = PrefabClone.transform.Find("model/exosuit_rig_armLeft:exosuit_propulsion_geo").gameObject;                

            Common.Helpers.GraphicsHelper.ChangeObjectTexture(model, 2, illumTex: Main.illumTex);            
        }        
    }
}
