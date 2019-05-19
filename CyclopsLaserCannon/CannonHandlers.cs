namespace CyclopsLaserCannonModule
{
    public partial class CannonControl
    {        
        private bool isActive;
        private bool isShoot = false;
        private bool isModuleInserted = false;
        public bool isPiloting = false;
        public bool isLowPower = false;

        private void LaserCannonSetActive(bool value)
        {
            cannon_base_right.SetActive(value);
            cannon_base_left.SetActive(value);
            Button_Cannon.SetActive(value);
        }

        private void EnableCannonOnUpgradeCounted(SubRoot cyclops, Equipment modules, string slot)
        {
            if (cyclops != subroot)
                return;
            
            isModuleInserted = true;
            LaserCannonSetActive(isModuleInserted);                      
        }

        private void DisableCannonOnClearUpgrades(SubRoot cyclops)
        {
            if (cyclops != subroot)
                return;

            isModuleInserted = false;
            LaserCannonSetActive(isModuleInserted);            
        }        

        private void OnConfigurationChanged(string configToChange)
        {
            switch (configToChange)
            {
                case "Damage":
                    SetLaserStrength();
                    break;
                case "OnlyHostile":
                    SetOnlyHostile();
                    break;
                case "SFX_Volume":
                    SetLaserSFXVolume();
                    break;
            }            
        }        

        private void OnPlayerModeChanged(Player.Mode newMode)
        {
            if (newMode == Player.Mode.Piloting)
                isPiloting = true;
            else
                isPiloting = false;            
        }

        internal void OnSubRootChanged(SubRoot newSubRoot)
        {
            if (newSubRoot == subroot)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }            
        }           
    }
}
