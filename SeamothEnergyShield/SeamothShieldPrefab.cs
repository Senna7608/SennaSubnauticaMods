using System.Collections;
using System.Collections.Generic;
using SMLExpander;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;

namespace SeamothEnergyShield
{
    internal class SeamothShieldPrefab : ModPrefab_Craftable
    {
        public static TechType TechTypeID { get; private set; }

        internal SeamothShieldPrefab()
            : base(techTypeName: "SeamothEnergyShield",
                  friendlyName: "Seamoth Energy Shield",
                  description: "Allows Seamoth to use a miniaturized Cyclops energy shield.",
                  template: TechType.SeamothElectricalDefense,
                  requiredAnalysis: TechType.CyclopsShieldModule,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.SeamothModule,
                  quickSlotType: QuickSlotType.Toggleable,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1,1),                  
                  gamerResourceFileName: null,
                  fragment: null
                  )
        {
        }
               
        protected override void PrePatch()
        {
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
        
        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }

        protected override CrafTreeTypesData GetCraftTreeTypesData()
        {
            return new CrafTreeTypesData()
            {
                TreeTypes = new List<CraftTreeType>()
                {
                    new CraftTreeType(CraftTree.Type.SeamothUpgrades, new string[] { "SeamothModules" } )
                }
            };
        }

        protected override TabNode GetTabNodeData()
        {
            return null;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/SeamothEnergyShield.png");
        }
    }
}
