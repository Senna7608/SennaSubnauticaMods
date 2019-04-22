using SMLHelper.V2.Options;

namespace CyclopsLaserCannonModule
{
    public class CannonOptions : ModOptions
    {
        public CannonOptions() :base(CannonConfig.language_settings["Option_Title"])        
        {            
            ToggleChanged += HostileOnly;            
            SliderChanged += LaserDamageChanged;
        }

        public override void BuildModOptions()
        {
            AddToggleOption(CannonConfig.SECTION_PROGRAM[0], CannonConfig.language_settings["Option_OnlyHostile"], bool.Parse(CannonConfig.program_settings["OnlyHostile"]));           
            AddSliderOption(CannonConfig.SECTION_PROGRAM[1], CannonConfig.language_settings["Option_Damage"], 1f, 100f, float.Parse(CannonConfig.program_settings["Damage"]));
        }
        
        private void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != CannonConfig.SECTION_PROGRAM[0])
                return;

            CannonConfig.program_settings["OnlyHostile"] = args.Value.ToString();
            CannonConfig.WriteConfig();

            Main.onConfigurationChanged.Trigger("OnlyHostile");            
        }
        
        private void LaserDamageChanged(object sender, SliderChangedEventArgs args)
        {
            if (args.Id != CannonConfig.SECTION_PROGRAM[1])
                return;

            CannonConfig.program_settings["Damage"] = args.Value.ToString();
            CannonConfig.WriteConfig();

            Main.onConfigurationChanged.Trigger("Damage");           
        }
    }
}
