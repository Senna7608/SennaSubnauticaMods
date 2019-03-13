using System;
using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using Harmony;
using System.Reflection;

namespace AncientSword
{
    public static class Main
    {
        
        public static TechType SwordTechType;

        public static void Load()
        {
            try
            { 
                Atlas.Sprite swordIcon = null;
                swordIcon = ImageUtils.LoadSpriteFromFile($"./QMods/AncientSword/Assets/AncientSword.png");

                SwordTechType = TechTypeHandler.AddTechType("AncientSword", "Ancient Sword", "An ancient sword from Earth.\nFound in an ancient Precursor facility.", false);

                SpriteHandler.RegisterSprite(SwordTechType, swordIcon);

                SwordPrefab swordPrefab = new SwordPrefab("AncientSword", "WorldEntities/Tools/AncientSword", SwordTechType);                

                var techData = new TechData
                {
                    craftAmount = 1,
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient(TechType.Titanium, 3),
                        new Ingredient(TechType.Copper, 2), 
                        new Ingredient(TechType.Nickel, 2),
                        new Ingredient(TechType.Diamond, 2),
                    },
                };

                CraftDataHandler.SetTechData(SwordTechType, techData);

                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, SwordTechType, new string[] { "Personal", "Tools" , "AncientSword"});

                CraftDataHandler.SetItemSize(SwordTechType, new Vector2int(1, 1));

                CraftDataHandler.SetEquipmentType(SwordTechType, EquipmentType.Hand);

                //KnownTechHandler.SetAnalysisTechEntry(TechType.PrecursorPrisonArtifact8, new TechType[1] { SwordTechType }, "Ancient Sword blueprint discovered!");

                PrefabHandler.RegisterPrefab(swordPrefab);

                HarmonyInstance.Create("Subnautica.AncientSword.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        [HarmonyPatch(typeof(PDAScanner))]
        [HarmonyPatch("Unlock")]
        public static class PDAScannerUnlockPatch
        {
            public static bool Prefix(PDAScanner.EntryData entryData)
            {
                if (entryData.key == TechType.PrecursorPrisonArtifact8)
                {
                    if (!KnownTech.Contains(SwordTechType))
                    {
                        KnownTech.Add(SwordTechType);
                        ErrorMessage.AddMessage("Added blueprint for Ancient Sword fabrication to database");
                    }
                }
                return true;
            }
        }

        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null)
            {
                return gameObject.GetComponent<T>();
            }
            return gameObject.AddComponent<T>();
        }

        public static bool AddIfNeedComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() == null)
            {
                gameObject.AddComponent<T>();
                return true;
            }
            return false;
        }
    }

}
