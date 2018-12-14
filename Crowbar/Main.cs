using System;
using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;

namespace Crowbar
{
    public static class Main
    {
        public static AssetBundle assetBundle;
        public static TechType CrowbarTechType;

        public static void Load()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile("./QMods/Crowbar/Assets/crowbar");

                if (assetBundle == null)
                    Debug.Log("[Crowbar] AssetBundle is NULL!");
                else
                    Debug.Log($"[Crowbar] AssetBundle loaded, name: {assetBundle.name}");

                Atlas.Sprite crowbarIcon = null;
                crowbarIcon = ImageUtils.LoadSpriteFromFile($"./QMods/Crowbar/Assets/Crowbar.png");

                CrowbarTechType = TechTypeHandler.AddTechType("Crowbar", "Crowbar", "An ancient tool from Earth.");

                SpriteHandler.RegisterSprite(CrowbarTechType, crowbarIcon);

                CrowbarPrefab crowbarPrefab = new CrowbarPrefab("Crowbar", "WorldEntities/Tools/Crowbar", CrowbarTechType);

                PrefabHandler.RegisterPrefab(crowbarPrefab);

                var techData = new TechData
                {
                    craftAmount = 1,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient(TechType.Titanium, 2),
                        new Ingredient(TechType.Copper, 2)                        
                    },
                };

                CraftDataHandler.SetTechData(CrowbarTechType, techData);

                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, CrowbarTechType, new string[] { "Personal", "Tools" , "Crowbar" });

                CraftDataHandler.SetItemSize(CrowbarTechType, new Vector2int(2, 2));

                CraftDataHandler.SetEquipmentType(CrowbarTechType, EquipmentType.Hand);
                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null)
            {
                return gameObject.GetComponent<T>();
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }
    }
}
