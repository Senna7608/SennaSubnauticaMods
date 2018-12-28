//#define DEBUG_CYCLOPS_OVERDRIVE

using UnityEngine;

namespace CheatManager
{
    public class CyclopsOverDrive : MonoBehaviour
    {
        public CyclopsOverDrive Instance { get; private set; }

        private SubRoot subroot;
        private SubControl subcontrol;

        private float prev_multiplier;        

        private CyclopsMotorMode.CyclopsMotorModes currentCyclopsMotorMode;

        private const float def_Slow_BaseForwardAccel = 4.5f;
        private const float def_Slow_BaseVerticalAccel = 4.5f;
        private const float def_Slow_BaseTurningTorque = 0.75f;

        private const float def_Standard_BaseForwardAccel = 6f;
        private const float def_Standard_BaseVerticalAccel = 6f;
        private const float def_Standard_BaseTurningTorque = 0.75f;

        private const float def_Flank_BaseForwardAccel = 7f;
        private const float def_Flank_BaseVerticalAccel = 7f;
        private const float def_Flank_BaseTurningTorque = 0.75f;

        public void Awake()
        {
            Instance = gameObject.GetComponent<CyclopsOverDrive>();

            if (Instance == null)
                 Destroy(this);
        }

        public void Start()
        {
            subroot = gameObject.GetComponentInParent<SubRoot>();            

            subcontrol = subroot.GetComponentInParent<SubControl>();            

            prev_multiplier = 1;            
        }       

        public void Update()
        {
            if (prev_multiplier != Main.Instance.cyclopsSpeedMultiplier || currentCyclopsMotorMode != subcontrol.cyclopsMotorMode.cyclopsMotorMode)
            {
                switch (subcontrol.cyclopsMotorMode.cyclopsMotorMode)
                {
                    case CyclopsMotorMode.CyclopsMotorModes.Slow:
                    subcontrol.BaseForwardAccel = def_Slow_BaseForwardAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseVerticalAccel = def_Slow_BaseVerticalAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseTurningTorque = def_Slow_BaseTurningTorque * Main.Instance.cyclopsSpeedMultiplier; ;
                    break;
                
                    case CyclopsMotorMode.CyclopsMotorModes.Standard:
                    subcontrol.BaseForwardAccel = def_Standard_BaseForwardAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseVerticalAccel = def_Standard_BaseVerticalAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseTurningTorque = def_Standard_BaseTurningTorque * Main.Instance.cyclopsSpeedMultiplier; ;
                    break;
                
                    case CyclopsMotorMode.CyclopsMotorModes.Flank:
                    subcontrol.BaseForwardAccel = def_Flank_BaseForwardAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseVerticalAccel = def_Flank_BaseVerticalAccel * Main.Instance.cyclopsSpeedMultiplier; ;
                    subcontrol.BaseTurningTorque = def_Flank_BaseTurningTorque * Main.Instance.cyclopsSpeedMultiplier; ;
                    break;
                }

                currentCyclopsMotorMode = subcontrol.cyclopsMotorMode.cyclopsMotorMode;
                prev_multiplier = Main.Instance.cyclopsSpeedMultiplier;

#if DEBUG_CYCLOPS_OVERDRIVE
                    Logger.Log($"[CheatManager]\nCyclopsOverDrive().Update()\nMotorMode: {subcontrol.cyclopsMotorMode.cyclopsMotorMode}:\nBaseForwardAccel: {subcontrol.BaseForwardAccel}\nBaseVerticalAccel: {subcontrol.BaseVerticalAccel}\nBaseTurningTorque: {subcontrol.BaseTurningTorque}");
#endif
            }
           
        }
    }
}
