using System;
using Common;

using UnityEngine;

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
            if (cyclops != subroot)
                return;

            TechType techtypeInSlot = modules.GetItemInSlot(slot).item.GetTechType();

            if (techtypeInSlot == CannonPrefab.TechTypeID)
            {
                isModuleInserted = true;
                LaserCannonSetActive(isModuleInserted);
                SNLogger.Log($"[CyclopsLaserCannonModule] EnableCannonOnUpgradeCounted() triggered");
            }            
        }

        private void DisableCannonOnClearUpgrades(SubRoot cyclops)
        {
            if (cyclops != subroot)
                return;

            isModuleInserted = false;
            LaserCannonSetActive(isModuleInserted);
            SNLogger.Log($"[CyclopsLaserCannonModule] DisableCannonOnClearUpgrades() triggered");
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
