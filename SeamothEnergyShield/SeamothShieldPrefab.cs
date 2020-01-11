using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;

namespace SeamothEnergyShield
{
    internal class SeamothShieldPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal SeamothShieldPrefab()
            : base(nameID: "SeamothEnergyShield",
                  iconFileName: "SeamothEnergyShield",
                  iconTechType: TechType.None,
                  friendlyName: "Seamoth Energy Shield",
                  description: "Allows Seamoth to use a miniaturized Cyclops energy shield.",
                  template: TechType.SeamothElectricalDefense,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: new string[] { "SeamothModules" },
                  requiredAnalysis: TechType.CyclopsShieldModule,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.SeamothModule,
                  quickSlotType: QuickSlotType.Toggleable,
                  backgroundType: CraftData.BackgroundType.Normal,
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
                    new Ingredient(TechType.CyclopsShieldModule, 1),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.AdvancedWiringKit, 1)

                })
            };
        }        
        
        public override GameObject GetGameObject()
        {
            base.GetGameObject();            

            return _GameObject;
        }        
    }
}
