using Common;

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
        
        private void OnFinishedUpgrades()
        {            
            if (upgradeHandler.TechType == Main.techTypeID && upgradeHandler.Count > 0)
            {
                isModuleInserted = true;
                LaserCannonSetActive(isModuleInserted);
            }
        }

        private void OnClearUpgrades()
        {
            isModuleInserted = false;
            LaserCannonSetActive(isModuleInserted);
        }

        private void OnFirstTimeCheckModuleIsExists()
        {
            SNLogger.Log($"[CyclopsLaserCannonModule] Trying to enable module...");

            if (upgradeHandler.Count > 0)
            {
                isModuleInserted = true;
                LaserCannonSetActive(isModuleInserted);
            }
        }
    }
}
