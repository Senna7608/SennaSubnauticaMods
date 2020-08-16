using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common.Helpers.SMLHelpers;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothPropulsionArmPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal SeamothPropulsionArmPrefab()
            : base(nameID: "SeamothPropulsionArmModule",
                  iconFilePath: null,
                  iconTechType: TechType.None,
                  friendlyName: "Seamoth Propulsion Arm",
                  description: "Allows Seamoth to use Propulsion Arm.",
                  template: TechType.ExosuitPropulsionArmModule,
                  newTabNode: null,
                  fabricatorTypes: new CraftTree.Type[] { CraftTree.Type.SeamothUpgrades },
                  fabricatorTabs: new string[][] { new string[] { "SeamothModules" } },
                  requiredAnalysis: TechType.ExosuitPropulsionArmModule,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: (EquipmentType) 100,
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

            _GameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            return _GameObject;
        }
        
    }
}
