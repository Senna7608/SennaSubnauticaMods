using Common;
using System;
using System.Text;
using System.IO;
using SMLHelper.V2.Options;
using UnityEngine;


namespace LaserCannon
{
    public class Config : ModOptions
    {
        private const string configFile = "./QMods/LaserCannon/LaserCannonConfig.txt";
        private const string configKey_main = "***LaserCannonConfig***";
        private const string config_splitter = " = ";        
        private const string configKey_language = "Language";
        public string mainLang = "English";
        private const string toggleID = "onlyHostile";
        private const string choiceID = "laserBeamColor";

        private const string settings_title_eng = "Laser Cannon Module Options";
        private const string settings_toggle_eng = "Damage hostile target only";
        private const string settings_color_eng = "Laser Beam Color";
        
        private const string settings_title_hun = "Lézerágyú beállítások";
        private const string settings_toggle_hun = "Csak ellenséges lényeket sebez";
        private const string settings_color_hun = "Lézersugár színe";

        private static readonly string[] beamColors = Modules.Colors.ColorNames;

        public int LaserBeamColor { get; private set; }
        public bool OnlyHostile { get; private set; }

        internal void Init()
        {
            try
            {
                LoadFromFile();                
            }
            catch (Exception ex)
            {
                Debug.Log($"[LaserCannon] Error loading {configKey_main}: " + ex.ToString());
                WriteConfigFile();
            }            
        }              

        public Config() :base(settings_title_hun)
        {            
            ToggleChanged += HostileOnly;
            ChoiceChanged += BeamColorChanged;
        }

       
        public override void BuildModOptions()
        {
            AddToggleOption(toggleID, settings_toggle_hun, OnlyHostile);
            AddChoiceOption(choiceID, settings_color_hun, beamColors, LaserBeamColor);
        }

        private void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != toggleID)
                return;

           OnlyHostile = args.Value;
           WriteConfigFile();

            if (LaserCannon_Seamoth.Main != null)
            {
                LaserCannon_Seamoth.Main.SendMessage("ShootOnlyHostile", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void BeamColorChanged(object sender, ChoiceChangedEventArgs args)
        {
            if (args.Id != choiceID)
                return;

            LaserBeamColor = args.Index;           
            WriteConfigFile();

            if (LaserCannon_Seamoth.Main != null)
            {
                LaserCannon_Seamoth.Main.SendMessage("SetBeamColor", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void WriteConfigFile()
        {
            File.WriteAllLines(configFile, new[]
            {
                configKey_main,
                toggleID + config_splitter + OnlyHostile.ToString(),
                choiceID + config_splitter + Modules.Colors.ColorNames[LaserBeamColor],
                configKey_language + config_splitter + mainLang

            }, Encoding.UTF8);
        }

        private void LoadFromFile()
        { 
            if (!File.Exists(configFile))
            {
                Debug.Log($"[LaserCannon] Laser Cannon module config file not found. Writing default file.");
                WriteConfigFile();
                return;
            }

            string[] configText = File.ReadAllLines(configFile, Encoding.UTF8);

            if (!SetValues(configText))
            {
                Debug.Log($"[LaserCannon] Laser Cannon module config file damaged. Writing default file.");
                WriteConfigFile();
                return;
            }
        }

        private bool SetValues(string[] configText)
        {           
            if (configText[0] != configKey_main)
            {
                return false;
            }
                     
            for (int i = 1; i < configText.Length; i++)
            {
                string[] splittedText = configText[i].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                
                switch (splittedText[0])
                {
                    case toggleID: OnlyHostile = bool.Parse(splittedText[1]);                        
                        break;
                    case choiceID:
                        LaserBeamColor = Array.IndexOf(Modules.Colors.ColorNames, splittedText[1]);
                        break;
                    case configKey_language:
                        mainLang = splittedText[1];
                        break;
                }
            }
            
            return true;
        }
    }





}
