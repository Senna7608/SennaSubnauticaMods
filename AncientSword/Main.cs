using System;
using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;

namespace AncientSword
{
    public static class Main
    {
        public static AssetBundle assetBundle;
        public static TechType SwordTechType;

        public static void Load()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile("./QMods/AncientSword/Assets/ancient_sword");

                if (assetBundle == null)
                    Debug.Log("[AncientSword] AssetBundle is NULL!");
                else
                    Debug.Log($"[AncientSword] AssetBundle loaded, name: {assetBundle.name}");

                Atlas.Sprite swordIcon = null;
                swordIcon = ImageUtils.LoadSpriteFromFile($"./QMods/AncientSword/Assets/AncientSword.png");

                SwordTechType = TechTypeHandler.AddTechType("AncientSword", "Ancient Sword", "An ancient sword from Earth.\nFound in an ancient Precursor facility.");

                SpriteHandler.RegisterSprite(SwordTechType, swordIcon);

                SwordPrefab swordPrefab = new SwordPrefab("AncientSword", "WorldEntities/Tools/AncientSword", SwordTechType);

                PrefabHandler.RegisterPrefab(swordPrefab);

                var techData = new TechData
                {
                    craftAmount = 1,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient(TechType.Titanium, 2),
                        new Ingredient(TechType.Copper, 2)                        
                    },
                };

                CraftDataHandler.SetTechData(SwordTechType, techData);

                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, SwordTechType, new string[] { "Personal", "Tools" , "AncientSword"});

                CraftDataHandler.SetItemSize(SwordTechType, new Vector2int(2, 2));

                CraftDataHandler.SetEquipmentType(SwordTechType, EquipmentType.Hand);
                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }        
    }
}
