using UnityEngine;

namespace CheatManager
{
    public class SeamothOverDrive : MonoBehaviour
    {
        public static SeamothOverDrive Main { get; private set; }        

        private Vehicle seamoth;

        private float prev_multiplier;
        private float Def_backwardForce;
        private float Def_forwardForce;
        private float Def_sidewardForce;
        private float Def_verticalForce;
        
        public void Awake()
        {
            Main = this;
            seamoth = Player.main.GetVehicle();
            Def_backwardForce = seamoth.backwardForce;
            Def_forwardForce = seamoth.forwardForce;
            Def_sidewardForce = seamoth.sidewardForce;
            Def_verticalForce = seamoth.verticalForce;
            prev_multiplier = 1;
        }        

        public void Update()
        {            

            if (prev_multiplier != CheatManager.seamothSpeedMultiplier)
            {
                if (CheatManager.seamothSpeedMultiplier == 1)
                {
                    seamoth.forwardForce = Def_forwardForce;
                    seamoth.backwardForce = Def_backwardForce;
                    seamoth.sidewardForce = Def_sidewardForce;
                    seamoth.verticalForce = Def_verticalForce;
                    prev_multiplier = CheatManager.seamothSpeedMultiplier;
                }
                else
                {
                    seamoth.forwardForce = 10 * CheatManager.seamothSpeedMultiplier;
                    seamoth.backwardForce = 10 * CheatManager.seamothSpeedMultiplier;
                    seamoth.sidewardForce = 10 * CheatManager.seamothSpeedMultiplier;
                    seamoth.verticalForce = 10 * CheatManager.seamothSpeedMultiplier;
                    prev_multiplier = CheatManager.seamothSpeedMultiplier;
                }
#if DEBUG
                Logger.Log($"Seamoth current multiplier: {prev_multiplier}");
#endif
            }

            seamoth.moveOnLand = CheatManager.seamothCanFly;
        }
    }
}
