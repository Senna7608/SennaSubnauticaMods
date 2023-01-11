using System.Collections;
using System.Collections.Generic;
using SMLExpander;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;

namespace CyclopsLaserCannonModule
{
    internal class CannonPrefab : ModPrefab_Craftable
    {
        public static TechType TechTypeID { get; private set; }

        public static CannonOptions Config { get; } = new CannonOptions();       

        internal CannonPrefab()
            : base(techTypeName: "CyclopsLaserCannonModule",
                  friendlyName: CannonConfig.language_settings["Item_Name"],
                  description: CannonConfig.language_settings["Item_Description"],
                  template: TechType.CyclopsHullModule1,
                  gamerResourceFileName: null,
                  requiredAnalysis: TechType.PrecursorPrisonArtifact7,
                  groupForPDA: TechGroup.VehicleUpgrades,
                  categoryForPDA: TechCategory.VehicleUpgrades,
                  equipmentType: EquipmentType.CyclopsModule,
                  quickSlotType: QuickSlotType.None,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1, 1),
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
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 2),
                    new Ingredient(TechType.PrecursorIonCrystal, 2),                    
                    new Ingredient(TechType.AdvancedWiringKit, 2),
                    new Ingredient(TechType.ComputerChip, 2)
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
                    new CraftTreeType(CraftTree.Type.CyclopsFabricator, new string[] { "" })
                }
            };
        }

        protected override TabNode GetTabNodeData()
        {
            return null;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/CyclopsLaserCannonModule.png");
        }
    }
}
