using UnityEngine;

namespace CyclopsLaserCannonModule
{
    public partial class CannonControl
    {
        private bool isOnlyHostile;
        private string lowPower_title;
        private string lowPower_message;
        private float laserDamage;
        private float powerConsumption;

        private void SetOnlyHostile()
        {
            isOnlyHostile = bool.Parse(CannonConfig.program_settings["OnlyHostile"].ToString());
        }

        private void SetLaserStrength()
        {
            float.TryParse(CannonConfig.program_settings["Damage"], out float damage);
            laserDamage = Mathf.Clamp(damage, 1f, 100f);
            powerConsumption = 1 + (laserDamage * 0.05f);
        }

        private void SetWarningMessage()
        {
            lowPower_title = CannonConfig.language_settings["LowPower_Title"];
            lowPower_message = CannonConfig.language_settings["LowPower_Message"];
        }

        private void SetLaserSFXVolume()
        {
            float.TryParse(CannonConfig.program_settings["SFX_Volume"], out float volume);
            volume = volume / 100f;            
            //audioSource.volume = Mathf.Clamp(volume, 0.01f, 0.1f);            
        }

    }
}