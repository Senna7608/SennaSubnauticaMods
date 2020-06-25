using Harmony;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using SlotExtender.Patches;
using SlotExtender.Configuration;
using System.Reflection;
using System.Collections.Generic;

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
#if DEBUG
                SNLogger.Debug("SlotExtender", $"Harmony Instance created, Name = [{hInstance.Id}]");
#endif
                hInstance.PatchAll(Assembly.GetExecutingAssembly());

                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            //check MoreQuickSlots namespace is exists
            if (RefHelp.IsNamespaceExists("MoreQuickSlots"))
            {
                SNLogger.Log("SlotExtender", " -> MoreQuickSlots namespace is exist! Trying to install a Cross-MOD patch...");

                //if yes construct a Harmony patch
                if (MQS_Patches.InitPatch(hInstance))
                    SNLogger.Log("SlotExtender", " -> MoreQuickSlots Cross-MOD patch installed!");
                else
                    SNLogger.Error("SlotExtender", " -> MoreQuickSlots Cross-MOD patch install failed!");
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                //enabling game console
                GameHelper.EnableConsole();
                //init config
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

            if (uGUI_SlotTextHandler.Instance != null)
            {
                uGUI_SlotTextHandler.Instance.RefreshText();
            }

            /*
            if (Initialize_uGUI.Instance != null)
            {
                Initialize_uGUI.Instance.RefreshText();
            }
            */

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

        internal static MethodBase GetConstructorMethodBase(Type type, string ctorName)
        {
            List<ConstructorInfo> ctor_Infos = new List<ConstructorInfo>();

            ctor_Infos = AccessTools.GetDeclaredConstructors(type);

            foreach (ConstructorInfo ctor_info in ctor_Infos)
            {
#if DEBUG
                SNLogger.Debug("SlotExtender", $"Found constructor in [{type}] class: [{ctor_info.Name}]");

                MethodBase mBase = ctor_info as MethodBase;

                ParameterInfo[] pInfos = mBase.GetParameters();

                if (pInfos.Length == 0)
                {
                    SNLogger.Debug("SlotExtender", $"constructor [{ctor_info.Name}] has no parameters.");
                }
                else
                {
                    SNLogger.Debug("SlotExtender", $"listing constructor parameters...");

                    foreach (ParameterInfo pInfo in pInfos)
                    {
                        SNLogger.Debug("SlotExtender", $"ctor parameter[{pInfo.Position}] = [{pInfo.ToString()}]");
                    }
                }
#endif
                if (ctor_info.Name == ctorName)
                {
                    return ctor_info as MethodBase;
                }
            }

#if DEBUG
            SNLogger.Debug("SlotExtender", $"the required constructor [{ctorName}] in class [{type}] has not found!");
#endif
            return null;
        }
    }
}

