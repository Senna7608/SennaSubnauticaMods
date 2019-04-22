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

        private void EnableCannon()
        {
            isModuleInserted = true;
            LaserCannonSetActive(true);            
        }

        private void DisableCannon()
        {
            isModuleInserted = false;
            LaserCannonSetActive(false);            
        }

        private void OnClearUpgrades(SubRoot cyclops)
        {
            if (cyclops == subroot)
            {
                DisableCannon();
            }
        }

        private void OnFinishedUpgrades(SubRoot cyclops)
        {
            if (cyclops == subroot && Main.upgradeHandler.techType == CannonPrefab.TechTypeID)
            {                
                EnableCannon();                              
            }
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
        
        /*
        private bool IsAllowedToAdd(SubRoot subRoot, Pickupable pickupable, bool verbose)
        {
            if (isModuleInserted && pickupable.GetTechType() == CannonPrefab.TechTypeID)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsAllowedToRemove(SubRoot subRoot, Pickupable pickupable, bool verbose)
        {
            if (isModuleInserted && pickupable.GetTechType() == CannonPrefab.TechTypeID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */
    }
}
