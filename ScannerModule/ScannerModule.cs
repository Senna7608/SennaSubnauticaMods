using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Common;

namespace ScannerModule
{
    internal class ScannerModule : Craftable
    {
        public static TechType TechTypeID { get; private set; }        

        internal ScannerModule()
            : base(nameID: "ScannerModule",
                  friendlyName: "Scanner Module",
                  description: "Allows to scan objects with Seamoth.",
                  template: TechType.SeamothElectricalDefense,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: "SeamothModules",
                  requiredAnalysis: TechType.BaseUpgradeConsole,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades)
        {
        }
               
        public override void Patch()
        {
            base.Patch();
            CraftDataHandler.SetEquipmentType(TechType, EquipmentType.VehicleModule);
            CraftDataHandler.SetQuickSlotType(TechType, QuickSlotType.Selectable);
            TechTypeID = TechType;
            
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[2]
                {
                    new Ingredient(TechType.Scanner, 1),
                    new Ingredient(TechType.AdvancedWiringKit, 1)                    
                })
            };
        }
    }
}
