using System.Collections;
using System.Collections.Generic;
using SMLExpander;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;

namespace LaserCannon
{
    internal class LaserCannonPrefab : ModPrefab_Craftable
    {
        public static TechType TechTypeID { get; private set; }        
        public static LaserCannonOptions Config { get; } = new LaserCannonOptions();       

        internal LaserCannonPrefab()
            : base(techTypeName: "LaserCannon",
                  friendlyName: LaserCannonConfig.language_settings["Item_Name"].ToString(),
                  description: LaserCannonConfig.language_settings["Item_Description"].ToString(),
                  template: TechType.SeamothSonarModule,                  
                  requiredAnalysis: TechType.BaseUpgradeConsole,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.SeamothModule,
                  quickSlotType: QuickSlotType.Selectable,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1, 1),
                  gamerResourceFileName: null,
                  fragment: null
                  )
        {
        }

        protected override void PrePatch()
        {                                   
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
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/LaserCannon.png");
        }
    }
}
