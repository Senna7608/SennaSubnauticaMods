using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common.Helpers.SMLHelpers;
using Common.Helpers;
using Common;

namespace RepulsionCannonArm
{
    internal class RepulsionCannonArmPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal RepulsionCannonArmPrefab()
            : base(nameID: "ExosuitRepulsionCannonArmModule",
                  iconFilePath: $"{Main.modFolder}/Assets/RepulsionCannonArm.png",
                  iconTechType: TechType.None,
                  friendlyName: "P.R.A.W.N Repulsion Cannon Arm",
                  description: "Allows P.R.A.W.N suit to use Repulsion Cannon Arm.",
                  template: TechType.ExosuitPropulsionArmModule,
                  newTabNode: null,
                  fabricatorTypes: new CraftTree.Type[] { CraftTree.Type.SeamothUpgrades },
                  fabricatorTabs: new string[][] { new string[] { "ExosuitModules" } },
                  requiredAnalysis: TechType.ExosuitPropulsionArmModule,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.ExosuitArm,
                  quickSlotType: QuickSlotType.Selectable,
                  backgroundType: CraftData.BackgroundType.ExosuitArm,                 
                  itemSize: new Vector2int(1,1),                  
                  gamerResourceFileName: null
                  )
        {
        }
               
        public override void Patch()
        {
            base.Patch();
            TechTypeID = TechType;
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
        
        
        public override GameObject GetGameObject()
        {
            base.GetGameObject();

            try
            {
                GameObject model = _GameObject.transform.Find("model/exosuit_rig_armLeft:exosuit_propulsion_geo").gameObject;

                model.name = "exosuit_rig_armLeft:exosuit_repulsion_geo";

                GraphicsHelper.ChangeObjectTexture(model, 2, illumTex: Main.objectHelper.GetObjectClone(Main.illumTex));                
            }
            catch
            {
                SNLogger.Error("RepulsionCannonArm", "child [model] not found in game object!");
            }           

            return _GameObject;
        }
        
    }
}
