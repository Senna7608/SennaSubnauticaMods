using SMLHelper.V2.Options;
using UnityEngine;
using Common;
using Common.Helpers;

namespace LaserCannon
{
    public class LaserCannonOptions : ModOptions
    {
        public LaserCannonOptions() : base(LaserCannonConfig.language_settings["Option_Title"])        
        {            
            ToggleChanged += HostileOnly;
            ChoiceChanged += BeamColorChanged;
            SliderChanged += LaserDamageChanged;
        }

        public override void BuildModOptions()
        {
            AddToggleOption(LaserCannonConfig.SECTION_PROGRAM[0], LaserCannonConfig.language_settings["Option_OnlyHostile"], bool.Parse(LaserCannonConfig.program_settings["OnlyHostile"]));
            AddChoiceOption(LaserCannonConfig.SECTION_PROGRAM[1], LaserCannonConfig.language_settings["Option_BeamColor"], LaserCannonConfig.colorNames.ToArray(), LaserCannonConfig.beamColor);
            AddSliderOption(LaserCannonConfig.SECTION_PROGRAM[2], LaserCannonConfig.language_settings["Option_Damage"], 1f, 100f, float.Parse(LaserCannonConfig.program_settings["Damage"]));
        }
        
        private void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != LaserCannonConfig.SECTION_PROGRAM[0])
                return;

            LaserCannonConfig.program_settings["OnlyHostile"] = args.Value.ToString();
            LaserCannonConfig.WriteConfig();

            Main.OnConfigChanged.Trigger(true);
        }

        private void BeamColorChanged(object sender, ChoiceChangedEventArgs args)
        {
            if (args.Id != LaserCannonConfig.SECTION_PROGRAM[1])
                return;            

            LaserCannonConfig.program_settings["BeamColor"] = ColorHelper.ColorNames[args.Index];
            LaserCannonConfig.beamColor = args.Index;

            LaserCannonConfig.WriteConfig();

            Main.OnConfigChanged.Trigger(true);
        }

        private void LaserDamageChanged(object sender, SliderChangedEventArgs args)
        {
            if (args.Id != LaserCannonConfig.SECTION_PROGRAM[2])
                return;

            LaserCannonConfig.program_settings["Damage"] = args.Value.ToString();
            LaserCannonConfig.WriteConfig();

            Main.OnConfigChanged.Trigger(true);
        }
    }
}
