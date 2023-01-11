using UnityEngine;
using UWE;

namespace CheatManager
{
    public class SeaglideOverDrive : MonoBehaviour
    {
        public SeaglideOverDrive Instance { get; private set; }       

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void Start()
        {
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));
            Main.Instance.isSeaglideFast.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(IsSeaglideFast));
        }

        public void OnDestroy()
        {
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
            Main.Instance.isSeaglideFast.changedEvent.RemoveHandler(this, IsSeaglideFast);
            Destroy(this);
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {            
            UnderwaterMotor underwaterMotor = Player.main.GetComponent<UnderwaterMotor>();

            if (Main.Instance.isSeaglideFast.value && newMotorMode == Player.MotorMode.Seaglide)
            {                
                underwaterMotor.playerSpeedModifier = 2f;                
            }
            else
            {
                underwaterMotor.playerSpeedModifier = 1f;                
            }
        }

        private void IsSeaglideFast(Utils.MonitoredValue<bool> SeaglideFastSpeedEnable)
        {
            OnPlayerMotorModeChanged(Player.main.motorMode);            
        } 
    }
}
