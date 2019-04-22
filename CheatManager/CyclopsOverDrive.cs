using UnityEngine;
using UWE;

namespace CheatManager
{
    public class CyclopsOverDrive : MonoBehaviour
    {
        public CyclopsOverDrive Instance { get; private set; }

        private SubRoot subroot;
        private SubControl subcontrol;
        
        public Event<CyclopsMotorMode.CyclopsMotorModes> onCyclopsMotorModeChanged = new Event<CyclopsMotorMode.CyclopsMotorModes>();

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
            if (Instance != null)
                Destroy(this);

            Instance = gameObject.GetComponent<CyclopsOverDrive>();            
        }

        public void Start()
        {
            subroot = gameObject.GetComponentInParent<SubRoot>();            

            subcontrol = subroot.GetComponentInParent<SubControl>();           

            Main.Instance.onCyclopsSpeedValueChanged.AddHandler(this, new Event<object>.HandleFunction(OnCyclopsSpeedValueChanged));
            onCyclopsMotorModeChanged.AddHandler(this, new Event<CyclopsMotorMode.CyclopsMotorModes>.HandleFunction(OnCyclopsMotorModeChanged));
        }

        private void OnCyclopsMotorModeChanged(CyclopsMotorMode.CyclopsMotorModes newMotorMode)
        {
            SetCyclopsOverDrive(Main.Instance.cyclopsSpeedMultiplier);
        }

        private void OnCyclopsSpeedValueChanged(object newValue)
        {            
            Main.Instance.cyclopsSpeedMultiplier = (float)newValue;
            SetCyclopsOverDrive((float)newValue);                       
        }

        private void SetCyclopsOverDrive(float multiplier)
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
        }
    }
}
