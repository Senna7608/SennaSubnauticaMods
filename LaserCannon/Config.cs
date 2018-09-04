using SMLHelper.V2.Options;
using UnityEngine;

namespace LaserCannon
{
    public class Config : ModOptions
    {
        public Config() :base(SettingsHelper.title)
        {            
            ToggleChanged += HostileOnly;
            ChoiceChanged += BeamColorChanged;
        }
       
        public override void BuildModOptions()
        {
            AddToggleOption(SettingsHelper.toggleID, SettingsHelper.toggle, SettingsHelper.OnlyHostile);
            AddChoiceOption(SettingsHelper.choiceID, SettingsHelper.color, SettingsHelper.beamColors, SettingsHelper.LaserBeamColor);
        }

        private void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != SettingsHelper.toggleID)
                return;

            SettingsHelper.OnlyHostile = args.Value;
            SettingsHelper.WriteConfigFile(true);

            if (LaserCannon_Seamoth.Main != null)
            {
                LaserCannon_Seamoth.Main.SendMessage("ShootOnlyHostile", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void BeamColorChanged(object sender, ChoiceChangedEventArgs args)
        {
            if (args.Id != SettingsHelper.choiceID)
                return;

            SettingsHelper.LaserBeamColor = args.Index;
            SettingsHelper.WriteConfigFile(true);

            if (LaserCannon_Seamoth.Main != null)
            {
                LaserCannon_Seamoth.Main.SendMessage("SetBeamColor", SendMessageOptions.DontRequireReceiver);
            }
        }        
    }
}
