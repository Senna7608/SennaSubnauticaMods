using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using UWE;
using static Common.Helpers.GraphicsHelper;
using SMLHelper.V2.Utility;
using MoreCyclopsUpgrades.API;
using MoreCyclopsUpgrades.API.Upgrades;
using System.IO;
using QModManager.API.ModLoading;

namespace CyclopsLaserCannonModule
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static AssetBundle assetBundle;
        public static Material cannon_material;
        public static Sprite buttonSprite;        
        public static bool isAssetsLoaded;

        public static Event<string> onConfigurationChanged = new Event<string>();

        [QModPatch]
        public static void Load()
        {
            try
            {
                isAssetsLoaded = LoadAssets();

                CannonConfig.LoadConfig();

                new CannonPrefab().Patch();               

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.CyclopsLaserCannonModule.mod");                

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);                

                RegisterUpgrade();                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }        

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                Language.main.OnLanguageChanged += CannonConfig.OnLanguageChanged;
            }
        }

        private static bool LoadAssets()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile($"{modFolder}/Assets/laser_sounds");

                Texture2D cannon_Button = ImageUtils.LoadTextureFromFile($"{modFolder}/Assets/cannon_Button.png");

                buttonSprite = Sprite.Create(cannon_Button, new Rect(0, 0, cannon_Button.width, cannon_Button.height), new Vector2(cannon_Button.width * 0.5f, cannon_Button.height * 0.5f));

                GetResourceMaterial("worldentities/doodads/precursor/precursorteleporter", "precursor_interior_teleporter_02_01", 0, out cannon_material);

                cannon_material.name = "cannon_material";

                return true;
            }
            catch
            {
                SNLogger.Error("CyclopsLaserCannon", "Loading assets failed!");
                return false;
            }

        }

        public static void RegisterUpgrade()
        {
            MCUServices.Register.CyclopsUpgradeHandler((SubRoot cyclops) =>
            {
                return new UpgradeHandler(CannonPrefab.TechTypeID, cyclops)
                {
                    MaxCount = 1                    
                };                

            });
        }
    }        
      
}
