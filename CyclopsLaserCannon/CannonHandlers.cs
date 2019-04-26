namespace CyclopsLaserCannonModule
{
    public partial class CannonControl
    {        
        private bool isActive;
        private bool isShoot = false;
        private bool isModuleInserted = false;
        public bool isPiloting = false;
        private bool isLowPower = false;

        private void LaserCannonSetActive(bool value)
        {
            cannon_base_right.SetActive(value);
            cannon_base_left.SetActive(value);
            Button_Cannon.SetActive(value);
        }

        private void EnableCannonOnUpgradeCounted(SubRoot cyclops, Equipment modules, string slot)
        {
            isModuleInserted = upgradeHandler.Count > 0;
            LaserCannonSetActive(isModuleInserted);
        }

        private void DisableCannonOnClearUpgrades(SubRoot cyclops)
        {
            isModuleInserted = false;
            LaserCannonSetActive(false);            
        }

        private void OnConfigurationChanged(string configToChange)
        {
            switch (configToChange)
            {
                case "Damage":
                    SetLaserStrength();
                    break;
                case "OnlyHostile":
                    ShootOnlyHostile();
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
