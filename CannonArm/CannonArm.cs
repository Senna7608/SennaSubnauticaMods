using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;

namespace CannonArm
{
    internal class CannonArm : CraftArm
    {
        public static TechType TechTypeID { get; private set; }        

        internal CannonArm()
            : base(nameID: "CannonArm",
                  friendlyName: "P.R.A.W.N Cannon Arm",
                  description: "Allows P.R.A.W.N suit to firing bullets.",                  
                  template: TechType.ExosuitTorpedoArmModule,
                  fabricatorType: CraftTree.Type.SeamothUpgrades,
                  fabricatorTab: "ExosuitModules",
                  requiredAnalysis: TechType.Exosuit,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades
                  )
        {
        }
               
        public override void Patch()
        {
            base.Patch();
           
            CraftDataHandler.SetEquipmentType(TechType, EquipmentType.ExosuitArm);
            CraftDataHandler.SetQuickSlotType(TechType, QuickSlotType.Selectable);
            TechTypeID = TechType;
            
        }
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                    new Ingredient(TechType.Gold, 1)                                        
                })
            };
        }
    }
}
