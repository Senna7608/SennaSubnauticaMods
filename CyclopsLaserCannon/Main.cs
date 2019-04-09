using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using UWE;
using CyclopsLaserCannonModule.Patch;
using MoreCyclopsUpgrades.Managers;
using MoreCyclopsUpgrades.CyclopsUpgrades;

namespace CyclopsLaserCannonModule
{
    public static class Main
    {
        public static HarmonyInstance hInstance;

        public static bool isExists_MoreCyclopsUpgrades = false;            

        public static AssetBundle assetBundle;        

        public static Event<string> onConfigurationChanged = new Event<string>();

        public static Event<InventoryItem> MCU_onEquip = new Event<InventoryItem>();
        public static Event<InventoryItem> MCU_onUnequip = new Event<InventoryItem>();

        public static void Load()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile("./QMods/CyclopsLaserCannonModule/Assets/laser_sounds");

                if (assetBundle == null)
                    SNLogger.Log("[CyclopsLaserCannonModule] AssetBundle is NULL!");
                else
                    SNLogger.Log($"[CyclopsLaserCannonModule] AssetBundle loaded, name: {assetBundle.name}");

                CannonConfig.InitConfig();
                var laserCannon = new CannonPrefab();
                laserCannon.Patch();

                hInstance = HarmonyInstance.Create("Subnautica.CyclopsLaserCannonModule.mod");              
                                
                hInstance.Patch(typeof(CyclopsExternalCams).GetMethod("Start",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance),
                    new HarmonyMethod(typeof(CyclopsExternalCams_Start_Patch), "Postfix"), null);                

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            isExists_MoreCyclopsUpgrades = RefHelp.IsNamespaceExists("MoreCyclopsUpgrades");
            
            if (isExists_MoreCyclopsUpgrades)
            {
                UpgradeManager.RegisterReusableHandlerCreator(() =>
                {
                    return new UpgradeHandler(CannonPrefab.TechTypeID)
                    {
                        IsAllowedToAdd = IsAllowedToAdd,
                        IsAllowedToRemove = IsAllowedToRemove,
                    };
                });
                
                SNLogger.Log("[CyclopsLaserCannonModule] -> MoreCyclopsUpgrades found! trying to work together...");

                MCU_Patcher mcu_patcher = new MCU_Patcher(hInstance);

                if (mcu_patcher.InitPatch())
                    SNLogger.Log($"[CyclopsLaserCannonModule] -> MCU Cross-MOD patch installed!");
                else
                    SNLogger.Log($"[CyclopsLaserCannonModule] -> MCU Cross-MOD patch install failed!");
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                Language.main.OnLanguageChanged += CannonConfig.OnLanguageChanged;
            }
        }        

        public static void OnEquip(string slot, InventoryItem item)
        {            
          MCU_onEquip.Trigger(item);
        }

        public static void OnUnequip(string slot, InventoryItem item)
        {            
          MCU_onUnequip.Trigger(item);
        }

        public static bool IsAllowedToAdd(SubRoot cyclops, Pickupable item, bool verbose)
        {
            Debug.Log($"[CyclopsLaserCannonModule] Main: IsAllowedToAdd: {item.GetTechName()}");
            return true;
        }

        public static bool IsAllowedToRemove(SubRoot cyclops, Pickupable item, bool verbose)
        {
            Debug.Log($"[CyclopsLaserCannonModule] Main: IsAllowedToRemove: {item.GetTechName()}");
            return true;
        }
    }     
}
