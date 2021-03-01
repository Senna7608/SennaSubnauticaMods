using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using Common.Helpers.SMLHelpers;

namespace RepairModule
{
    internal class RepairModulePrefab : ModPrefab_Craftable
    {
        public static TechType TechTypeID { get; private set; }

        internal RepairModulePrefab()
            : base(nameID: "RepairModule",
                  iconFilePath: $"{Main.modFolder}/Assets/RepairModule.png",
                  iconTechType: TechType.None,
                  friendlyName: "Repair Module",
                  description: "Allows to repair damaged Vehicles from inside.",
                  template: TechType.SeamothSonarModule,
                  newTabNode: null,
                  fabricatorTypes: new CraftTree.Type[] { CraftTree.Type.SeamothUpgrades },
                  fabricatorTabs: new string[][] { new string[] { "CommonModules" } },                  
                  requiredAnalysis: TechType.BaseUpgradeConsole,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.VehicleModule,
                  quickSlotType: QuickSlotType.Toggleable,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1, 1),
                  gamerResourceFileName: null,
                  fragment: null
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
                Ingredients = new List<Ingredient>(new Ingredient[2]
                {
                    new Ingredient(TechType.Welder, 1),
                    new Ingredient(TechType.AdvancedWiringKit, 1)                    
                })
            };
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }
    }
}
