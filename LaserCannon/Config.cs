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
        internal const string configKey = "LaserCannonConfig";        
        private const string toggleID = "onlyHostile";
        private const string choiceID = "laserBeamColor";
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
                Debug.Log($"[LaserCannon] Error loading {configKey}: " + ex.ToString());
                WriteConfigFile();
            }
        }              

        public Config() : base("Laser Cannon Module Options")
        {
            ToggleChanged += HostileOnly;
            ChoiceChanged += BeamColorChanged;
        }

       
        public override void BuildModOptions()
        {
            AddToggleOption(toggleID, "Damage hostile target only", OnlyHostile);
            AddChoiceOption(choiceID, "Laser Beam Color", beamColors, LaserBeamColor);
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
                configKey,
                toggleID,
                OnlyHostile.ToString(),
                choiceID,
                LaserBeamColor.ToString()
                
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
            SetValues(configText);
        }

        private bool SetValues(string[] configText)
        {
            if (configText[0] != configKey)
            {
                return false;
            }

            for (int i = 1; i < configText.Length; i++)
            {
                switch (configText[i])
                {
                    case toggleID: OnlyHostile = bool.Parse(configText[i + 1]);                        
                        break;
                    case choiceID:
                        LaserBeamColor = int.Parse(configText[i + 1]);
                        break;
                }
            }

            return true;
        }
    }





}
