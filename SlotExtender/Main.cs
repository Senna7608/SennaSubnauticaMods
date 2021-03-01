using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using HarmonyLib;
using SMLHelper.V2.Handlers;
using Common;
using SlotExtender.Configuration;
using SlotExtender.Patches;
using Common.Helpers;
using QModManager.API.ModLoading;

namespace SlotExtender
{
    [QModCore]
    public static class Main
    {
        public static CommandRoot commandRoot = null;
        public static Harmony hInstance;
        internal static InputFieldListener ListenerInstance { get; set; }

        public static bool isConsoleActive;
        public static bool isKeyBindigsUpdate = false;

        public static bool uGUI_PrefixComplete = false;
        public static bool uGUI_PostfixComplete = false;        

        [QModPatch]
        public static void Load()
        {
            SNLogger.Debug("Method call: Main.Load()");

            try
            {
                SEConfig.Config_Load();
                SlotHelper.InitSlotIDs();
                SlotHelper.ExpandSlotMapping();

                hInstance = new Harmony("Subnautica.SlotExtender.mod");

                SNLogger.Debug($"Main.Load(): Harmony Instance created, Name = [{hInstance.Id}]");

                hInstance.PatchAll(Assembly.GetExecutingAssembly());                

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
                
                SNLogger.Debug("Main.Load(): Added OnSceneLoaded method to SceneManager.sceneLoaded event.");

                //add console commad for configuration window
                commandRoot = new CommandRoot("SEConfigGO");                
                commandRoot.AddCommand<SECommand>();

                IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            // check MoreQuickSlots namespace is exists
            if (ReflectionHelper.IsNamespaceExists("MoreQuickSlots"))
            {
                SNLogger.Log("MoreQuickSlots namespace is exist! Trying to install a Cross-MOD patch...");

                // if yes construct a Harmony patch
                if (MQS_Patches.InitPatch(hInstance))
                    SNLogger.Log("MoreQuickSlots Cross-MOD patch installed!");
                else
                    SNLogger.Error("MoreQuickSlots Cross-MOD patch install failed!");
            }
        }

        private static void OnQuitEvent()
        {
            uGUI_PrefixComplete = false;
            uGUI_PostfixComplete = false;            
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                // enabling game console
                DevConsole.disableConsole = false;

                // init config
                SEConfig.Config_Init();

                // add an action if changed keybindings
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;

                SlotHelper.InitSessionAllSlots();
            }
            if (scene.name == "Main")
            {
                // creating a console input field listener to skip SlotExdender Update method key events conflict while console is active in game
                ListenerInstance = InitializeListener();                
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {
            SNLogger.Debug("Method call: Main.GameInput_OnBindingsChanged()");

            // SlotExtender Update() method now disabled until all keybinding updates are complete
            isKeyBindigsUpdate = true;

            // updating slot key bindings
            SEConfig.SLOTKEYBINDINGS_Update();

            // synchronizing keybindings to config file
            SEConfig.SLOTKEYBINDINGS_SyncToAll();

            // updating ALLSLOTS dictionary
            SlotHelper.ALLSLOTS_Update();

            // updating SlotTextHandler
            if (uGUI_SlotTextHandler.Instance != null)
            {
                uGUI_SlotTextHandler.Instance.UpdateSlotText();
            }

            // SlotExtender Update() method now enabled
            isKeyBindigsUpdate = false;
        }

        internal static InputFieldListener InitializeListener()
        {
            if (ListenerInstance == null)
            {
                ListenerInstance = UnityEngine.Object.FindObjectOfType(typeof(InputFieldListener)) as InputFieldListener;

                if (ListenerInstance == null)
                {
                    GameObject inputFieldListener = new GameObject("InputFieldListener");
                    ListenerInstance = inputFieldListener.AddComponent<InputFieldListener>();
                }
            }

            return ListenerInstance;
        }        
    }
}

