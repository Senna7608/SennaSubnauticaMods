using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;
using Common.Helpers.SMLHelpers;
using Common.Helpers;

namespace PlasmaCannonArm
{
    internal class PlasmaCannonArmPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal PlasmaCannonArmPrefab()
            : base(nameID: "ExosuitPlasmaCannonArmModule",
                  iconFilePath: $"{Main.modFolder}/Assets/PlasmaCannonArm.png",
                  iconTechType: TechType.None,
                  friendlyName: "P.R.A.W.N Plasma Cannon Arm",
                  description: "Allows P.R.A.W.N suit to firing small plasma bullets.",
                  template: TechType.ExosuitTorpedoArmModule,
                  newTabNode: null,
                  fabricatorTypes: new CraftTree.Type[] { CraftTree.Type.SeamothUpgrades },
                  fabricatorTabs: new string[][] { new string[] { "ExosuitModules" } },
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

            TechTypeID = TechType;
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
                GameObject model = _GameObject.transform.Find("model/exosuit_rig_armLeft:exosuit_torpedoLauncher_geo").gameObject;

                model.name = "exosuit_rig_armLeft:exosuit_plasmaCannon_geo";

                GraphicsHelper.SetMeshMaterial(model, Main.cannon_material, 1);

                SNLogger.Debug("PlasmaCannonArm", $"Arm module created. Mesh material changed. Name: [{model.name}]");
            }
            catch
            {
                SNLogger.Error("PlasmaCannonArm", "child [model] not found in game object!");
            }            
            

            return _GameObject;
        }
        
    }
}
