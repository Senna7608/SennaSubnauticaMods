#if DEBUG
using UnityEngine;
#endif

namespace CheatManager
{
    internal static class VehicleControl
    {
        public static void SeamothControl(ref Vehicle vehicle, float multiplier, bool canfly)
        {
            if (multiplier == 1)
            {
                vehicle.forwardForce = 11.5f;
                vehicle.backwardForce = 5f;
                vehicle.sidewardForce = 11.5f;
                vehicle.verticalForce = 11f;
            }
            else
            {
                vehicle.forwardForce = 10 * multiplier;
                vehicle.backwardForce = 10 * multiplier;
                vehicle.sidewardForce = 10 * multiplier;
                vehicle.verticalForce = 10 * multiplier;
            }

            vehicle.moveOnLand = canfly ? true : false;

            CheatManager.prevSeamothCanFly = canfly;
            CheatManager.prevSeamothSpeedMultiplier = multiplier;
#if DEBUG
            Logger.Log("Seamoth speed modified!", LogType.Log);
#endif
        }

        public static void ExosuitControl(ref Vehicle vehicle, float multiplier)
        {
            if (multiplier == 1)
            {
                vehicle.forwardForce = 6f;
                vehicle.backwardForce = 2f;
                vehicle.sidewardForce = 3f;
                vehicle.verticalForce = 2f;
            }
            else
            {
                vehicle.forwardForce = 6 * multiplier;
                vehicle.backwardForce = 6 * multiplier;
                vehicle.sidewardForce = 6 * multiplier;
                vehicle.verticalForce = 6 * multiplier;
            }

            CheatManager.prevExosuitSpeedMultiplier = multiplier;
#if DEBUG
            Logger.Log("Exosuit speed modified!", LogType.Log);
#endif

        }

        public static void CyclopsControl(ref SubControl subControl, float multiplier)
        { 
            switch (subControl.cyclopsMotorMode.cyclopsMotorMode)
            {
                case CyclopsMotorMode.CyclopsMotorModes.Slow:
                    subControl.BaseForwardAccel = 4.5f * multiplier;
                    subControl.BaseVerticalAccel = 4.5f * multiplier;
                    subControl.BaseTurningTorque = 0.75f * multiplier;
                    CheatManager.currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Slow;
                    break;

                case CyclopsMotorMode.CyclopsMotorModes.Standard:
                    subControl.BaseForwardAccel = 6 * multiplier;
                    subControl.BaseVerticalAccel = 6 * multiplier;
                    subControl.BaseTurningTorque = 0.75f * multiplier;
                    CheatManager.currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Standard;
                    break;

                case CyclopsMotorMode.CyclopsMotorModes.Flank:
                    subControl.BaseForwardAccel = 7 * multiplier;
                    subControl.BaseVerticalAccel = 7 * multiplier;
                    subControl.BaseTurningTorque = 0.75f * multiplier;
                    CheatManager.currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Flank;
                    break;
                default:
                    break;
            }

            CheatManager.prevCyclopsSpeedMultiplier = multiplier;
#if DEBUG
            Logger.Log("Cyclops speed modified", LogType.Log);
#endif

        }
    }
}
