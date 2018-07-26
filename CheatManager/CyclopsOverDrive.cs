using UnityEngine;

namespace CheatManager
{
    public class CyclopsOverDrive : MonoBehaviour
    {
        public static CyclopsOverDrive Main { get; private set; }

        private SubControl subcontrol;

        private float prev_multiplier;

        private CyclopsMotorMode.CyclopsMotorModes currentCyclopsMotorMode;

        private float def_Slow_BaseForwardAccel;
        private float def_Slow_BaseVerticalAccel;
        private float def_Slow_BaseTurningTorque;

        private float def_Standard_BaseForwardAccel;
        private float def_Standard_BaseVerticalAccel;
        private float def_Standard_BaseTurningTorque;

        private float def_Flank_BaseForwardAccel;
        private float def_Flank_BaseVerticalAccel;
        private float def_Flank_BaseTurningTorque;

        public void Awake()
        {
            Main = this;

            subcontrol = Player.main.currentSub.gameObject.GetComponent<SubControl>();

            prev_multiplier = 1;

            subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Slow;

            def_Slow_BaseForwardAccel = subcontrol.BaseForwardAccel;
            def_Slow_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
            def_Slow_BaseTurningTorque = subcontrol.BaseTurningTorque;
#if DEBUG
            Logger.Log($"Cyclops GetDefaults: MotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif
            subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Flank;

            def_Flank_BaseForwardAccel = subcontrol.BaseForwardAccel;
            def_Flank_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
            def_Flank_BaseTurningTorque = subcontrol.BaseTurningTorque;
#if DEBUG
            Logger.Log($"Cyclops GetDefaults: MotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif

            subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Standard;

            def_Standard_BaseForwardAccel = subcontrol.BaseForwardAccel;
            def_Standard_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
            def_Standard_BaseTurningTorque = subcontrol.BaseTurningTorque;            

            currentCyclopsMotorMode = subcontrol.cyclopsMotorMode.cyclopsMotorMode;

#if DEBUG
            Logger.Log($"Cyclops GetDefaults: MotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif

        }

        public void Update()
        {
            if (prev_multiplier != CheatManager.cyclopsSpeedMultiplier || currentCyclopsMotorMode != subcontrol.cyclopsMotorMode.cyclopsMotorMode)
            {
                switch (subcontrol.cyclopsMotorMode.cyclopsMotorMode)
                {
                    case CyclopsMotorMode.CyclopsMotorModes.Slow:
                        subcontrol.BaseForwardAccel = def_Slow_BaseForwardAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseVerticalAccel = def_Slow_BaseVerticalAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseTurningTorque = def_Slow_BaseTurningTorque * CheatManager.cyclopsSpeedMultiplier;
                        currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Slow;
                        break;

                    case CyclopsMotorMode.CyclopsMotorModes.Standard:
                        subcontrol.BaseForwardAccel = def_Standard_BaseForwardAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseVerticalAccel = def_Standard_BaseVerticalAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseTurningTorque = def_Standard_BaseTurningTorque * CheatManager.cyclopsSpeedMultiplier;
                        currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Standard;
                        break;

                    case CyclopsMotorMode.CyclopsMotorModes.Flank:
                        subcontrol.BaseForwardAccel = def_Flank_BaseForwardAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseVerticalAccel = def_Flank_BaseVerticalAccel * CheatManager.cyclopsSpeedMultiplier;
                        subcontrol.BaseTurningTorque = def_Flank_BaseTurningTorque * CheatManager.cyclopsSpeedMultiplier;
                        currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Flank;
                        break;
                }

                prev_multiplier = CheatManager.cyclopsSpeedMultiplier;
#if DEBUG
                Logger.Log($"Cyclops MotorMode: {currentCyclopsMotorMode}: current multiplier: {prev_multiplier}");
#endif
            }

        }
    }
}
