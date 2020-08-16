using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using HarmonyLib;
using Common.Helpers;
using UWE;
using QModManager.API.ModLoading;

namespace LaserCannon
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static ObjectHelper objectHelper { get; private set; }

        public static Event<bool> OnConfigChanged = new Event<bool>();

        [QModPatch]
        public static void Load()
        {
            try
            {

                objectHelper = new ObjectHelper();

                LaserCannonConfig.Config_Load();

                new LaserCannonPrefab().Patch();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.LaserCannon.mod");                

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
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
                Language.main.OnLanguageChanged += LaserCannonConfig.OnLanguageChanged;
            }
        }
    }    
}
