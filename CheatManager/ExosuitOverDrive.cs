using UnityEngine;

namespace CheatManager
{
    public class ExosuitOverDrive : MonoBehaviour
    {
        public static ExosuitOverDrive Main { get; private set; }

        private Vehicle exosuit;

        private float prev_multiplier;
        private float Def_backwardForce;
        private float Def_forwardForce;
        private float Def_sidewardForce;
        private float Def_verticalForce;
        
        public void Awake()
        {
            Main = this;
            exosuit = Player.main.GetVehicle();

            Def_backwardForce = exosuit.backwardForce;
            Def_forwardForce = exosuit.forwardForce;
            Def_sidewardForce = exosuit.sidewardForce;
            Def_verticalForce = exosuit.verticalForce;

            prev_multiplier = 1;
        }        

        public void Update()
        {
            
            if (prev_multiplier != CheatManager.exosuitSpeedMultiplier)
            {
                if (CheatManager.exosuitSpeedMultiplier == 1f)
                {
                    exosuit.forwardForce = Def_forwardForce;
                    exosuit.backwardForce = Def_backwardForce;
                    exosuit.sidewardForce = Def_sidewardForce;
                    exosuit.verticalForce = Def_verticalForce;
                    prev_multiplier = CheatManager.exosuitSpeedMultiplier;
                }
                else
                {
                    exosuit.forwardForce = 6 * CheatManager.exosuitSpeedMultiplier;
                    exosuit.backwardForce = 6 * CheatManager.exosuitSpeedMultiplier;
                    exosuit.sidewardForce = 6 * CheatManager.exosuitSpeedMultiplier;
                    exosuit.verticalForce = 6 * CheatManager.exosuitSpeedMultiplier;
                    prev_multiplier = CheatManager.exosuitSpeedMultiplier;
                }
#if DEBUG
                Logger.Log($"Exosuit current multiplier: {prev_multiplier}");
#endif
            }
        }
    }
}
