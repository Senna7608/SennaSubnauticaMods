using Common;
using System.Collections;

namespace CyclopsLaserCannonModule
{
    public partial class CannonControl
    {        
        private bool isActive = true;
        private bool isShoot = false;
        private bool isModuleInserted = false;
        public bool isPiloting = false;
        public bool isLowPower = false;

        private IEnumerator LaserCannonSetActiveAsync(bool value)
        {
            SNLogger.Debug($"LaserCannonSetActiveAsync ({value})started...");

            while (!asyncOperationsComplete)
            {
                yield return null;
            }

            cannon_base_right.SetActive(value);
            cannon_base_left.SetActive(value);
            Button_Cannon.SetActive(value);

            yield break;
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
            SNLogger.Debug($"OnPlayerModeChanged: newMode: {newMode}");

            if (newMode == Player.Mode.Piloting)
                isPiloting = true;
            else
                isPiloting = false;            
        }

        internal void OnSubRootChanged(SubRoot newSubRoot)
        {
            SNLogger.Debug($"OnSubRootChanged: newSubRoot: {newSubRoot?.name}");

            if (newSubRoot == subroot)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }            
        }

        private void OnUnequip(string slot, InventoryItem item)
        {
            SNLogger.Debug($"OnUnequip: slot: {slot}, item: {item.techType}");

            if (item.techType == CannonPrefab.TechTypeID)
            {
                isModuleInserted = false;
                StartCoroutine(LaserCannonSetActiveAsync(isModuleInserted));
            }
        }

        private void OnEquip(string slot, InventoryItem item)
        {
            SNLogger.Debug($"OnEquip: slot: {slot}, item: {item.techType}");

            if (item.techType == CannonPrefab.TechTypeID)
            {
                isModuleInserted = true;
                StartCoroutine(LaserCannonSetActiveAsync(isModuleInserted));
            }
        }

        public void CyclopsUpgradeModuleChange(TechType techType)
        {
            SNLogger.Debug($"CyclopsUpgradeModuleChange: techType: {techType}");

            if (techType == CannonPrefab.TechTypeID)
            {
                isModuleInserted = true;
                StartCoroutine(LaserCannonSetActiveAsync(isModuleInserted));
            }
        }
    }
}
