using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;

namespace SNTestMOD
{
    internal class TestPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal TestPrefab()
            : base(nameID: "Test Item",
                  iconFileName: null,
                  iconTechType: TechType.WhirlpoolTorpedo,
                  friendlyName: "Test Item",
                  description: "Test description.",
                  template: TechType.WhirlpoolTorpedo,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: new string[] { "Torpedoes" },
                  requiredAnalysis: TechType.BaseUpgradeConsole,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.None,
                  quickSlotType: QuickSlotType.None,
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
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                    new Ingredient(TechType.Titanium, 1)                    
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
