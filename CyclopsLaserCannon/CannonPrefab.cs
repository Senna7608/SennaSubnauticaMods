using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Common;
using UnityEngine;

namespace CyclopsLaserCannonModule
{
    internal class CannonPrefab : Craftable
    {              
        public static CannonOptions Config { get; } = new CannonOptions();       

        internal CannonPrefab()
            : base(nameID: "CyclopsLaserCannonModule",
                  nameUsingForFiles: "CyclopsLaserCannonModule",
                  friendlyName: CannonConfig.language_settings["Item_Name"],
                  description: CannonConfig.language_settings["Item_Description"],
                  template: TechType.CyclopsHullModule1,
                  fabricatorType: CraftTree.Type.CyclopsFabricator,
                  fabricatorTab: "Root",
                  requiredAnalysis: TechType.PrecursorPrisonArtifact7,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.CyclopsModule,
                  quickSlotType: QuickSlotType.None,
                  itemSize: new Vector2int(1, 1),                  
                  gamerResourceFileName: null
                  )
        {
        }
               
        public override void Patch()
        {
            base.Patch();                   
            OptionsPanelHandler.RegisterModOptions(Config);
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 2),
                    new Ingredient(TechType.PrecursorIonCrystal, 2),                    
                    new Ingredient(TechType.AdvancedWiringKit, 2),
                    new Ingredient(TechType.ComputerChip, 2)
                })
            };
        }        
    }
}
