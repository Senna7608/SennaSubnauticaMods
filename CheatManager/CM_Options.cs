using CheatManager.Configuration;
using SMLHelper.V2.Options;
using System;
using UnityEngine;

namespace CheatManager
{
    public class CM_Options : ModOptions
    {
        public CM_Options() : base ("Cheat Manager settings")
        {
            KeybindChanged += CM_Options_KeybindChanged;
            ToggleChanged += CM_Options_ToggleChanged;
            SliderChanged += CM_Options_SliderChanged;            
        }

        public override void BuildModOptions()
        {
            AddKeybindOption("ToggleWindow", "Toggle window", GameInput.Device.Keyboard, (KeyCode)Enum.Parse(typeof(KeyCode), CmConfig.Section_hotkeys["ToggleWindow"]));
            AddKeybindOption("ToggleMouse", "Toggle mouse pointer", GameInput.Device.Keyboard, (KeyCode)Enum.Parse(typeof(KeyCode), CmConfig.Section_hotkeys["ToggleMouse"]));
            AddKeybindOption("ToggleConsole", "Toggle log window", GameInput.Device.Keyboard, (KeyCode)Enum.Parse(typeof(KeyCode), CmConfig.Section_hotkeys["ToggleConsole"]));

            AddToggleOption("EnableConsole", "Enable log window in the lower left corner", bool.Parse(CmConfig.Section_program["EnableConsole"]));
            AddToggleOption("EnableInfoBar", "Enable info bar on top of screen", bool.Parse(CmConfig.Section_program["EnableInfoBar"]));

            AddToggleOption("fastbuild", "Fast build", bool.Parse(CmConfig.Section_toggleButtons["fastbuild"]));
            AddToggleOption("fastscan", "Fast scan", bool.Parse(CmConfig.Section_toggleButtons["fastscan"]));
            AddToggleOption("fastgrow", "Fast grow", bool.Parse(CmConfig.Section_toggleButtons["fastgrow"]));
            AddToggleOption("fasthatch", "Fast hatch", bool.Parse(CmConfig.Section_toggleButtons["fasthatch"]));
            AddToggleOption("filterfast", "Filter fast", bool.Parse(CmConfig.Section_toggleButtons["filterfast"]));
            AddToggleOption("radiation", "No radiation", bool.Parse(CmConfig.Section_toggleButtons["radiation"]));
            AddToggleOption("invisible", "Invisible", bool.Parse(CmConfig.Section_toggleButtons["invisible"]));
            AddToggleOption("nodamage", "No damage", bool.Parse(CmConfig.Section_toggleButtons["nodamage"]));
            AddToggleOption("alwaysday", "Always day", bool.Parse(CmConfig.Section_toggleButtons["alwaysday"]));
            AddToggleOption("noinfect", "No infect", bool.Parse(CmConfig.Section_toggleButtons["noinfect"]));
            AddToggleOption("overpower", "Overpower (Health and Oxygen)", bool.Parse(CmConfig.Section_toggleButtons["overpower"]));

            AddSliderOption("OverPowerMultiplier", "Overpower multiplier", 2f, 10f, float.Parse(CmConfig.Section_settings["OverPowerMultiplier"]), 2f, "{0:F0}", 1f);
            AddSliderOption("HungerAndThirstInterval", "Hunger and thirst interval", 1f, 10f, float.Parse(CmConfig.Section_settings["HungerAndThirstInterval"]), 10f, "{0:F0}", 1f);
        }

        private void CM_Options_KeybindChanged(object sender, KeybindChangedEventArgs args)
        {
            switch (args.Id)
            {
                case "ToggleWindow":
                    CmConfig.Section_hotkeys["ToggleWindow"] = args.KeyName;
                    SyncConfig();
                    break;

                case "ToggleMouse":
                    CmConfig.Section_hotkeys["ToggleMouse"] = args.KeyName;
                    SyncConfig();
                    break;

                case "ToggleConsole":
                    CmConfig.Section_hotkeys["ToggleConsole"] = args.KeyName;
                    SyncConfig();
                    break;
            }
        }

        private void CM_Options_ToggleChanged(object sender, ToggleChangedEventArgs args)
        {
            switch (args.Id)
            {
                case "EnableConsole":
                    CmConfig.Section_program["EnableConsole"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "EnableInfoBar":
                    CmConfig.Section_program["EnableInfoBar"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "fastbuild":
                    CmConfig.Section_toggleButtons["fastbuild"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "fastscan":
                    CmConfig.Section_toggleButtons["fastscan"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "fastgrow":
                    CmConfig.Section_toggleButtons["fastgrow"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "fasthatch":
                    CmConfig.Section_toggleButtons["fasthatch"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "filterfast":
                    CmConfig.Section_toggleButtons["filterfast"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "radiation":
                    CmConfig.Section_toggleButtons["radiation"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "invisible":
                    CmConfig.Section_toggleButtons["invisible"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "nodamage":
                    CmConfig.Section_toggleButtons["nodamage"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "alwaysday":
                    CmConfig.Section_toggleButtons["alwaysday"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "noinfect":
                    CmConfig.Section_toggleButtons["noinfect"] = args.Value.ToString();
                    SyncConfig();
                    break;
                case "overpower":
                    CmConfig.Section_toggleButtons["overpower"] = args.Value.ToString();
                    SyncConfig();
                    break;
            }
        }

        private void CM_Options_SliderChanged(object sender, SliderChangedEventArgs args)
        {
            switch (args.Id)
            {
                case "OverPowerMultiplier":
                    CmConfig.Section_settings["OverPowerMultiplier"] = args.Value.ToString();
                    SyncConfig();
                    break;

                case "HungerAndThirstInterval":
                    CmConfig.Section_settings["HungerAndThirstInterval"] = args.Value.ToString();
                    SyncConfig();
                    break;
            }
        }

        private void SyncConfig()
        {
            CmConfig.Set();
            CmConfig.Write();            
        }        
    }
}
