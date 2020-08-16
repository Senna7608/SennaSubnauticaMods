using Common.Helpers;
using CheatManager.Configuration;
using UnityEngine;

namespace CheatManager.NewCommands
{
    public class OverPowerConsoleCommand : MonoBehaviour
    {
        public static OverPowerConsoleCommand main;
        private bool OverPowerCheat;
        private float overPowerMultiplier;
        private float hungerAndThristInterval;        

        public void Awake()
        {
            main = this;
            
            DevConsole.RegisterConsoleCommand(this, "overpower");            
        }

        private void OnConsoleCommand_overpower(NotificationCenter.Notification n)
        {
            SetOverPowerCheat(!OverPowerCheat);
        }

        public bool GetOverPowerCheat() => OverPowerCheat;

        public void SetOverPowerCheat(bool value)
        {
            OverPowerCheat = value;

            if (value)
            {
                overPowerMultiplier = CmConfig.overPowerMultiplier;
                hungerAndThristInterval = CmConfig.hungerAndThirstInterval;
            }
            else
            {
                overPowerMultiplier = 1;
                hungerAndThristInterval = 10;
            }

            Oxygen o2 = Player.main.GetComponent<OxygenManager>().GetComponent<Oxygen>();
            
            Player.main.GetComponent<Survival>().SetPrivateField("kUpdateHungerInterval", hungerAndThristInterval);

            if (o2.isPlayer)
            {
                o2.oxygenCapacity = 45 * overPowerMultiplier;
            }

            Player.main.liveMixin.data.maxHealth = 100 * overPowerMultiplier;
            Player.main.liveMixin.health = 100 * overPowerMultiplier;

            ErrorMessage.AddDebug($"overpower cheat is now {OverPowerCheat}\nOverPower Multiplier: {overPowerMultiplier}x\nHunger and thirst interval: {hungerAndThristInterval}");
        }        
    }
}
