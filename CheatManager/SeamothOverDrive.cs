using Common;
using UnityEngine;
using UWE;

namespace CheatManager
{
    public class SeamothOverDrive : MonoBehaviour
    {
        public SeamothOverDrive Instance { get; private set; }
        public SeaMoth ThisSeamoth { get; private set; }
        public Equipment ThisEquipment { get; private set; }
                
        private const float Seamoth_Default_ForwardForce = 12.52f;
        private const float Seamoth_Default_BackwardForce = 5.45f;
        private const float Seamoth_Default_SidewardForce = 12.52f;
        private const float Seamoth_Default_VerticalForce = 11.93f;        

        public void Awake()
        {
            Instance = this;
            ThisSeamoth = gameObject.GetComponent<SeaMoth>();            
        }

        public void Start()
        {
            ThisEquipment = ThisSeamoth.modules;
            Main.Instance.isSeamothCanFly.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(OnSeamothCanFlyChanged));
            Main.Instance.onSeamothSpeedValueChanged.AddHandler(this, new Event<object>.HandleFunction(OnSeamothSpeedValueChanged));            
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));            
        }

        private void OnSeamothSpeedValueChanged(object newValue)
        {           
            Main.Instance.seamothSpeedMultiplier = (float)newValue;
            SetSeamothOverDrive(ThisSeamoth, Mathf.InverseLerp(1f, 5f, (float)newValue));                     
        }        

        private void OnSeamothCanFlyChanged(Utils.MonitoredValue<bool> parms)
        {
            SetSeamothToFly(ThisSeamoth, Main.Instance.isSeamothCanFly.value);
        }

        public void OnDestroy()
        {
            Main.Instance.isSeamothCanFly.changedEvent.RemoveHandler(this, OnSeamothCanFlyChanged);
            Main.Instance.onSeamothSpeedValueChanged.RemoveHandler(this, OnSeamothSpeedValueChanged);            
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
            Destroy(Instance);
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode == Player.MotorMode.Vehicle)
            {
                if (Player.main.GetVehicle() == ThisSeamoth)
                {
                    SetSeamothToFly(ThisSeamoth, Main.Instance.isSeamothCanFly.value);
                    SetSeamothOverDrive(ThisSeamoth, Mathf.InverseLerp(1f, 5f, Main.Instance.seamothSpeedMultiplier));                   
                }
            }
        }

        internal void SetSeamothToFly(SeaMoth seamoth, bool canFly)
        {
            seamoth.moveOnLand = canFly;
        }

        internal void SetSeamothOverDrive(SeaMoth seamoth, float multiplier)
        {
            SNLogger.Debug($"SeamothOverdrive before: {seamoth.forwardForce}");
            seamoth.forwardForce = Mathf.Lerp(Seamoth_Default_ForwardForce, 50f, multiplier);
            seamoth.backwardForce = Mathf.Lerp(Seamoth_Default_BackwardForce, 50f, multiplier);
            seamoth.sidewardForce = Mathf.Lerp(Seamoth_Default_SidewardForce, 50f, multiplier);
            seamoth.verticalForce = Mathf.Lerp(Seamoth_Default_VerticalForce, 50f, multiplier);
            SNLogger.Debug($"SeamothOverdrive after: {seamoth.forwardForce}");
        } 
    }
}
