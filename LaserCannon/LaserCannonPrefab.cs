using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Common;

namespace LaserCannon
{
    internal class LaserCannonPrefab : Craftable
    {
        public static TechType TechTypeID { get; private set; }        
        public static LaserCannonOptions Config { get; } = new LaserCannonOptions();       

        internal LaserCannonPrefab()
            : base(nameID: "LaserCannon",
                  nameUsingForFiles: "LaserCannon",
                  friendlyName: LaserCannonConfig.language_settings["Item_Name"].ToString(),
                  description: LaserCannonConfig.language_settings["Item_Description"].ToString(),
                  template: TechType.SeamothSonarModule,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: "SeamothModules",
                  requiredAnalysis: TechType.BaseUpgradeConsole,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.SeamothModule,
                  quickSlotType: QuickSlotType.Toggleable,
                  itemSize: new Vector2int(1, 1),
                  gamerResourceFileName: null
                  )
        {
        }
               
        public override void Patch()
        {
            base.Patch();                        
            TechTypeID = TechType;           
            OptionsPanelHandler.RegisterModOptions(Config);
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.RepulsionCannon, 1),
                    new Ingredient(TechType.PropulsionCannon, 1),
                    new Ingredient(TechType.PowerCell, 2),
                    new Ingredient(TechType.AdvancedWiringKit, 2)
                    
                })
            };
        }        
    }
}
