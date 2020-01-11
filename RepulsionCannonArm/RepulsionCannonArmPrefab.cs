using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;

namespace RepulsionCannonArm
{
    internal class RepulsionCannonArmPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal RepulsionCannonArmPrefab()
            : base(nameID: "ExosuitRepulsionCannonArmModule",
                  iconFileName: "RepulsionCannonArm",
                  iconTechType: TechType.None,
                  friendlyName: "P.R.A.W.N Repulsion Cannon Arm",
                  description: "Allows P.R.A.W.N suit to use Repulsion Cannon Arm.",
                  template: TechType.ExosuitPropulsionArmModule,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: new string[] { "ExosuitModules" },
                  requiredAnalysis: TechType.ExosuitPropulsionArmModule,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.ExosuitArm,
                  backgroundType: CraftData.BackgroundType.ExosuitArm,
                  quickSlotType: QuickSlotType.Selectable,
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
                GameObject model = _GameObject.FindChild("model").FindChild("exosuit_rig_armLeft:exosuit_propulsion_geo");

                model.name = "exosuit_rig_armLeft:exosuit_repulsion_geo";

                model.ChangeObjectTexture(2, illumTex: Main.illumTex.GetObjectClone());
            }
            catch
            {
                Debug.Log("[RepulsionCannonArm] ***ERROR: child [model] not found in game object!");
            }           

            return _GameObject;
        }
        
    }
}
