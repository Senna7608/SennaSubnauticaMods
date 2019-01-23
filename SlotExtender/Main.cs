﻿using Harmony;
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
        public static SEConfig sEConfig;

        public static void Load()
        {
            try
            {
                Config.LoadConfig();
                SlotHelper.InitSlotIDs();

                hInstance = HarmonyInstance.Create("Subnautica.SlotExtender.mod");

                //Harmony autopatch not working if MoreQuickSlots mod not installed therefore switch to manual patching mode
                //hInstance.PatchAll(Assembly.GetExecutingAssembly());
                
                //begin manual patch
                hInstance.Patch(typeof(Equipment).GetMethod("GetSlotType"),
                    new HarmonyMethod(typeof(Equipment_GetSlotType_Patch), "Prefix"),  null);
                
                hInstance.Patch(typeof(Equipment).GetMethod("AllowedToAdd"),
                    new HarmonyMethod(typeof(Equipment_AllowedToAdd_Patch), "Prefix"), null);                    

                hInstance.Patch(typeof(SeaMoth).GetProperty("slotIDs",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.GetProperty).GetGetMethod(true),
                    new HarmonyMethod(typeof(Seamoth_slotIDs_Patch), "Prefix"),  null);

                hInstance.Patch(typeof(SeaMoth).GetMethod("Awake"),  null,
                    new HarmonyMethod(typeof(SeaMoth_Awake_Patch), "Postfix"));

                hInstance.Patch(typeof(Exosuit).GetProperty("slotIDs",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.GetProperty).GetGetMethod(true),
                    new HarmonyMethod(typeof(Exosuit_slotIDs_Patch), "Prefix"), null);

                hInstance.Patch(typeof(Exosuit).GetMethod("Awake"), null,
                    new HarmonyMethod(typeof(Exosuit_Awake_Patch), "Postfix"));
                               
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
                Logger.Log("-> MoreQuickSlots namespace is exist! Trying to install a Cross-MOD patch...");
                //if yes construct a Harmony patch
                MQS_Patcher mqs_patcher = new MQS_Patcher(hInstance);

                if (mqs_patcher.InitPatch())
                    Logger.Log("-> MoreQuickSlots Cross-MOD patch installed!");
                else
                    Logger.Log("-> MoreQuickSlots Cross-MOD patch install failed!");
            }            
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                //enabling game console
                UnityHelper.EnableConsole();
                //loading config from file
                Config.InitConfig();
                //add console commad for configuration window
                sEConfig = new SEConfig();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;                
            }            
        }

        internal static void GameInput_OnBindingsChanged()
        {
            //input changed, refreshing key bindings
            Config.InitSLOTKEYS();            
            
            if (Initialize_uGUI.Instance != null)
            {
                Initialize_uGUI.Instance.RefreshText();
            }            
        }        
    }    
}
