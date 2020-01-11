using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;

namespace PlasmaCannonArm
{
    internal class PlasmaCannonArmPrefab : CraftableModItem
    {
        internal PlasmaCannonArmPrefab()
            : base(nameID: "ExosuitPlasmaCannonArmModule",
                  iconFileName: "PlasmaCannonArm",
                  iconTechType: TechType.None,
                  friendlyName: "P.R.A.W.N Plasma Cannon Arm",
                  description: "Allows P.R.A.W.N suit to firing small plasma bullets.",
                  template: TechType.ExosuitTorpedoArmModule,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: new string[] { "ExosuitModules" },
                  requiredAnalysis: TechType.PrecursorPrisonArtifact7,
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
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 1),
                    new Ingredient(TechType.PrecursorIonCrystal, 1),                    
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
                GameObject model = _GameObject.FindChild("model").FindChild("exosuit_rig_armLeft:exosuit_torpedoLauncher_geo");

                model.name = "exosuit_rig_armLeft:exosuit_plasmaCannon_geo";

                model.SetMeshMaterial(Main.cannon_material, 1);                
            }
            catch
            {
                SNLogger.Log("[PlasmaCannonArm] ***ERROR: child [model] not found in game object!");
            }            
            

            return _GameObject;
        }
        
    }
}
