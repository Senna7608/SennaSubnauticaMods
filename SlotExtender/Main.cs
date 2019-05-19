using Harmony;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using SlotExtender.Patchers;
using SlotExtender.Configuration;
using System.Reflection;

namespace SlotExtender
{
    public static class Main
    {        
        public static HarmonyInstance hInstance;
        public static SECommand sEConfig;

        internal static InputFieldListener ListenerInstance { get; set; }
        public static bool isConsoleActive;
        public static bool isKeyBindigsUpdate = false;

        public static void Load()
        {
            try
            {
                SEConfig.LoadConfig();
                SlotHelper.InitSlotIDs();

                hInstance = HarmonyInstance.Create("Subnautica.SlotExtender.mod");

                //Harmony autopatch not working if MoreQuickSlots mod not installed therefore switch to manual patching mode
                //hInstance.PatchAll(Assembly.GetExecutingAssembly());

                //begin manual patch
                hInstance.Patch(typeof(DevConsole).GetMethod("SetState"),
                    new HarmonyMethod(typeof(DevConsole_SetState_Patch), "Prefix"), null);

                hInstance.Patch(typeof(Equipment).GetMethod("GetSlotType"),
                    new HarmonyMethod(typeof(Equipment_GetSlotType_Patch), "Prefix"),  null);
                
                hInstance.Patch(typeof(Equipment).GetMethod("AllowedToAdd"),
                    new HarmonyMethod(typeof(Equipment_AllowedToAdd_Patch), "Prefix"), null);                    

                hInstance.Patch(typeof(SeaMoth).GetProperty("slotIDs",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.GetProperty).GetGetMethod(true),
                    new HarmonyMethod(typeof(Seamoth_slotIDs_Patch), "Prefix"),  null);

                hInstance.Patch(typeof(SeaMoth).GetMethod("Start"),  null,
                    new HarmonyMethod(typeof(SeaMoth_Start_Patch), "Postfix"));

                hInstance.Patch(typeof(Exosuit).GetProperty("slotIDs",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.GetProperty).GetGetMethod(true),
                    new HarmonyMethod(typeof(Exosuit_slotIDs_Patch), "Prefix"), null);

                hInstance.Patch(typeof(Exosuit).GetMethod("Start"), null,
                    new HarmonyMethod(typeof(Exosuit_Start_Patch), "Postfix"));
                               
                hInstance.Patch(typeof(uGUI_Equipment).GetMethod("Awake",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.SetField),
                    new HarmonyMethod(typeof(uGUI_Equipment_Awake_Patch), "Prefix"),
                    new HarmonyMethod(typeof(uGUI_Equipment_Awake_Patch), "Postfix"));                

                //end manual patch 

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
           
            //check MoreQuickSlots namespace is exists
            if (RefHelp.IsNamespaceExists("MoreQuickSlots"))
            {
                SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] -> MoreQuickSlots namespace is exist! Trying to install a Cross-MOD patch...");
                //if yes construct a Harmony patch
                MQS_Patcher mqs_patcher = new MQS_Patcher(hInstance);

                if (mqs_patcher.InitPatch())
                    SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] -> MoreQuickSlots Cross-MOD patch installed!");
                else
                    SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] -> MoreQuickSlots Cross-MOD patch install failed!");
            }            
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                //enabling game console
                GameHelper.EnableConsole();
                //loading config from file
                SEConfig.InitConfig();
                //add console commad for configuration window
                sEConfig = new SECommand();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;                
            }
            if (scene.name == "Main")
            {
                //creating a console input field listener to skip SlotExdender Update method key events conflict
                ListenerInstance = InitializeListener();
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {
            isKeyBindigsUpdate = true;

            //input changed, refreshing key bindings
            SEConfig.InitSLOTKEYS();            
            
            if (Initialize_uGUI.Instance.IsNotNull())
            {
                Initialize_uGUI.Instance.RefreshText();
            }

            isKeyBindigsUpdate = false;
        }

        internal static InputFieldListener InitializeListener()
        {
            if (ListenerInstance.IsNull())
            {
                ListenerInstance = UnityEngine.Object.FindObjectOfType(typeof(InputFieldListener)) as InputFieldListener;

                if (ListenerInstance.IsNull())
                {
                    GameObject inputFieldListener = new GameObject("InputFieldListener");
                    ListenerInstance = inputFieldListener.GetOrAddComponent<InputFieldListener>();                    
                }
            }

            return ListenerInstance;
        }
    }    
}
