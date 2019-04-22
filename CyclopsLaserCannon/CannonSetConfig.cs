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

        public void ShootOnlyHostile()
        {
            isOnlyHostile = bool.Parse(CannonConfig.program_settings["OnlyHostile"].ToString());
        }

        public void SetLaserStrength()
        {
            laserDamage = float.Parse(CannonConfig.program_settings["Damage"]);
            powerConsumption = 1 + (laserDamage * 0.05f);
        }

        public void SetWarningMessage()
        {
            lowPower_title = CannonConfig.language_settings["LowPower_Title"];
            lowPower_message = CannonConfig.language_settings["LowPower_Message"];
        }
        
    }
}