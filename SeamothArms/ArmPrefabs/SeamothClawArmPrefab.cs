using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;

namespace SeamothArms.ArmPrefabs
{
    internal class SeamothClawArmPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal SeamothClawArmPrefab()
            : base(nameID: "SeamothClawArmModule",
                  iconFileName: null,
                  iconTechType: TechType.ExosuitClawArmModule,
                  friendlyName: "Seamoth Claw Arm",
                  description: "Allows Seamoth to use Claw Arm.",
                  template: TechType.ExosuitDrillArmModule,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: new string[] { "SeamothModules" },
                  requiredAnalysis: TechType.Exosuit,
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
                    new Ingredient(TechType.TitaniumIngot, 1),                                        
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.ComputerChip, 1)
                })
            };
        }        
        
        public override GameObject GetGameObject()
        {
            base.GetGameObject();            

            _GameObject.AddIfNeedComponent<SeamothClawArmPrefabFixer>();

            return _GameObject;
        }        
    }
}
