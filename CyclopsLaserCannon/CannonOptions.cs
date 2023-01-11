using SMLHelper.V2.Options;

namespace CyclopsLaserCannonModule
{
    public class CannonOptions : ModOptions
    {
        public CannonOptions() : base(CannonConfig.language_settings["Option_Title"])        
        {            
            ToggleChanged += HostileOnly;
            SliderChanged += CannonOptions_SliderChanged;            
        }

        private void CannonOptions_SliderChanged(object sender, SliderChangedEventArgs args)
        {
            switch (args.Id)
            {
                case "Damage":
                    CannonConfig.program_settings["Damage"] = args.Value.ToString();
                    Main.onConfigurationChanged.Trigger("Damage");
                    CannonConfig.WriteConfig();
                    break;

                case "SFX_Volume":
                    CannonConfig.program_settings["SFX_Volume"] = args.Value.ToString();
                    Main.onConfigurationChanged.Trigger("SFX_Volume");
                    CannonConfig.WriteConfig();
                    break;

            }
        }

        public override void BuildModOptions()
        {
            AddToggleOption(CannonConfig.SECTION_PROGRAM[0], CannonConfig.language_settings["Option_OnlyHostile"], bool.Parse(CannonConfig.program_settings["OnlyHostile"]));           
            AddSliderOption(CannonConfig.SECTION_PROGRAM[1], CannonConfig.language_settings["Option_Damage"], 1f, 100f, float.Parse(CannonConfig.program_settings["Damage"]), 50f, "{0:F0}", 1f);
            AddSliderOption(CannonConfig.SECTION_PROGRAM[3], CannonConfig.language_settings["Option_SFXvolume"], 1f, 10f, float.Parse(CannonConfig.program_settings["SFX_Volume"]), 5f, "{0:F0}", 1f);
        }
        
        public void HostileOnly(object sender, ToggleChangedEventArgs args)
        {
            if (args.Id != "OnlyHostile")
                return;

            CannonConfig.program_settings["OnlyHostile"] = args.Value.ToString();
            CannonConfig.WriteConfig();

            Main.onConfigurationChanged.Trigger("OnlyHostile");            
        }        
    }
}
