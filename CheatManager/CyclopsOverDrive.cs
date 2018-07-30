#define DEBUG_CYCLOPS_OVERDRIVE

using System;
using UnityEngine;

namespace CheatManager
{
    public class CyclopsOverDrive : MonoBehaviour
    {
        public static CyclopsOverDrive Main { get; private set; }

        private SubRoot subroot;
        private SubControl subcontrol;

        private float prev_multiplier;
        private bool getDefaultsComplete = false;

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
        }

        public void Start()
        {
            subroot = gameObject.GetComponent<SubRoot>();            

            subcontrol = subroot.GetComponent<SubControl>();
            
            Player.main.playerModeChanged.AddHandler(gameObject, GetDefaults);

            prev_multiplier = 1;                       
        }        

        private void GetDefaults(Player.Mode parms)
        {
            if (!getDefaultsComplete && parms == Player.Mode.Piloting)
            {
                subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Slow;

                def_Slow_BaseForwardAccel = subcontrol.BaseForwardAccel;
                def_Slow_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
                def_Slow_BaseTurningTorque = subcontrol.BaseTurningTorque;

#if DEBUG_CYCLOPS_OVERDRIVE
                Logger.Log($"[CheatManager]\nCyclopsOverDrive().GetDefaults()\nMotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif
                subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Flank;

                def_Flank_BaseForwardAccel = subcontrol.BaseForwardAccel;
                def_Flank_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
                def_Flank_BaseTurningTorque = subcontrol.BaseTurningTorque;

#if DEBUG_CYCLOPS_OVERDRIVE
                Logger.Log($"[CheatManager]\nCyclopsOverDrive().GetDefaults()\nMotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif

                subcontrol.cyclopsMotorMode.cyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Standard;

                def_Standard_BaseForwardAccel = subcontrol.BaseForwardAccel;
                def_Standard_BaseVerticalAccel = subcontrol.BaseVerticalAccel;
                def_Standard_BaseTurningTorque = subcontrol.BaseTurningTorque;

                currentCyclopsMotorMode = subcontrol.cyclopsMotorMode.cyclopsMotorMode;

#if DEBUG_CYCLOPS_OVERDRIVE
                Logger.Log($"[CheatManager]\nCyclopsOverDrive().GetDefaults()\nMotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}");
#endif                
            }

            getDefaultsComplete = true;
        }

        public void Update()
        {
            if (getDefaultsComplete)
            {
                float multiplier = CheatManager.cyclopsSpeedMultiplier;

                if (prev_multiplier != multiplier || currentCyclopsMotorMode != subcontrol.cyclopsMotorMode.cyclopsMotorMode)
                {
                    switch (subcontrol.cyclopsMotorMode.cyclopsMotorMode)
                    {
                        case CyclopsMotorMode.CyclopsMotorModes.Slow:
                            subcontrol.BaseForwardAccel = def_Slow_BaseForwardAccel * multiplier;
                            subcontrol.BaseVerticalAccel = def_Slow_BaseVerticalAccel * multiplier;
                            subcontrol.BaseTurningTorque = def_Slow_BaseTurningTorque * multiplier;
                            currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Slow;
                            break;

                        case CyclopsMotorMode.CyclopsMotorModes.Standard:
                            subcontrol.BaseForwardAccel = def_Standard_BaseForwardAccel * multiplier;
                            subcontrol.BaseVerticalAccel = def_Standard_BaseVerticalAccel * multiplier;
                            subcontrol.BaseTurningTorque = def_Standard_BaseTurningTorque * multiplier;
                            currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Standard;
                            break;

                        case CyclopsMotorMode.CyclopsMotorModes.Flank:
                            subcontrol.BaseForwardAccel = def_Flank_BaseForwardAccel * multiplier;
                            subcontrol.BaseVerticalAccel = def_Flank_BaseVerticalAccel * multiplier;
                            subcontrol.BaseTurningTorque = def_Flank_BaseTurningTorque * multiplier;
                            currentCyclopsMotorMode = CyclopsMotorMode.CyclopsMotorModes.Flank;
                            break;
                    }

                    prev_multiplier = multiplier;
#if DEBUG_CYCLOPS_OVERDRIVE
                    Logger.Log($"[CheatManager]\nCyclopsOverDrive().Update()\nMotorMode: {currentCyclopsMotorMode}: current multiplier: {prev_multiplier}");
#endif
                }
            }
        }
    }
}
