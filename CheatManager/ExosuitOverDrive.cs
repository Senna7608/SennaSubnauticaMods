using UnityEngine;
using UWE;

namespace CheatManager
{
    public class ExosuitOverDrive : MonoBehaviour
    {
        public ExosuitOverDrive Instance { get; private set; }
        public Exosuit ThisExosuit { get; private set; }
        public Equipment ThisEquipment { get; private set; }

        private const float Exosuit_Default_ForwardForce = 8.2f;
        private const float Exosuit_Default_BackwardForce = 3f;
        private const float Exosuit_Default_SidewardForce = 4.2f;
        private const float Exosuit_Default_VerticalForce = 2f;

        public void Awake()
        {
            Instance = gameObject.GetComponent<ExosuitOverDrive>();
            ThisExosuit = Instance.GetComponent<Exosuit>();           
        }

        public void Start()
        {
            ThisEquipment = ThisExosuit.modules;
            Main.Instance.onExosuitSpeedValueChanged.AddHandler(this, new Event<object>.HandleFunction(OnExosuitSpeedValueChanged));
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));            
        }

        private void OnExosuitSpeedValueChanged(object newValue)
        {
            Main.Instance.exosuitSpeedMultiplier = (float)newValue;                
            SetExosuitOverDrive(ThisExosuit, Mathf.InverseLerp(1f, 5f, (float)newValue));
        }        

        public void OnDestroy()
        {
            Main.Instance.onExosuitSpeedValueChanged.RemoveHandler(this, OnExosuitSpeedValueChanged);
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
            Destroy(Instance);
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode == Player.MotorMode.Vehicle)
            {
                if (Player.main.GetVehicle() == ThisExosuit)
                {
                    SetExosuitOverDrive(ThisExosuit, Mathf.InverseLerp(1f, 5f, Main.Instance.exosuitSpeedMultiplier));                   
                }
            }
        }

        internal void SetExosuitOverDrive(Exosuit exosuit, float multiplier)
        {
            exosuit.forwardForce = Mathf.Lerp(Exosuit_Default_ForwardForce, 50f, multiplier);
            exosuit.backwardForce = Mathf.Lerp(Exosuit_Default_BackwardForce, 50f, multiplier);
            exosuit.sidewardForce = Mathf.Lerp(Exosuit_Default_SidewardForce, 50f, multiplier);
            exosuit.verticalForce = Mathf.Lerp(Exosuit_Default_VerticalForce, 50f, multiplier);
        }
    }
}
