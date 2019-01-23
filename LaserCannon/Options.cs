using SMLHelper.V2.Options;
using UnityEngine;
using Common;

namespace LaserCannon
{
    public class Options : ModOptions
    {
        public Options() :base(Config.language_settings["Option_Title"])        
        {            
            ToggleChanged += HostileOnly;
            ChoiceChanged += BeamColorChanged;
            SliderChanged += LaserDamageChanged;
        }

        public override void BuildModOptions()
        {
            AddToggleOption(Config.SECTION_PROGRAM[0], Config.language_settings["Option_OnlyHostile"], bool.Parse(Config.program_settings["OnlyHostile"]));
            AddChoiceOption(Config.SECTION_PROGRAM[1], Config.language_settings["Option_BeamColor"], Config.colorNames.ToArray(), Config.beamColor);
            AddSliderOption(Config.SECTION_PROGRAM[2], Config.language_settings["Option_Damage"], 1f, 100f, float.Parse(Config.program_settings["Damage"]));
        }
        
        private void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != Config.SECTION_PROGRAM[0])
                return;

            Config.program_settings["OnlyHostile"] = args.Value.ToString();
            Config.WriteConfig();

            if (Main.Instance != null)
            {
                Main.Instance.SendMessage("ShootOnlyHostile", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void BeamColorChanged(object sender, ChoiceChangedEventArgs args)
        {
            if (args.Id != Config.SECTION_PROGRAM[1])
                return;            

            Config.program_settings["BeamColor"] = Modules.Colors.ColorNames[args.Index];
            Config.beamColor = args.Index;

            Config.WriteConfig();

            if (Main.Instance != null)
            {
                Main.Instance.SendMessage("SetBeamColor", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void LaserDamageChanged(object sender, SliderChangedEventArgs args)
        {
            if (args.Id != Config.SECTION_PROGRAM[2])
                return;

            Config.program_settings["Damage"] = args.Value.ToString();
            Config.WriteConfig();

            if (Main.Instance != null)
            {
                Main.Instance.SendMessage("SetLaserStrength", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
