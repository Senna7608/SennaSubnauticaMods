using UnityEngine;
using UWE;

namespace CheatManager
{
    public class SeaglideOverDrive : MonoBehaviour
    {
        public static SeaglideOverDrive Instance { get; private set; }       
              
        public void Awake()
        {
            if (gameObject.GetComponent<Seaglide>() == null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void Start()
        { 
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));            
        }

        public void OnDestroy()
        {
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
            Destroy(this);
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode motorMode)
        {           
            SetSeaglideSpeed();            
        }

        public void SetSeaglideSpeed()
        {         
            if (CheatManager.Instance.isSeaglideFast.value && Player.main.motorMode == Player.MotorMode.Seaglide && Player.main.IsUnderwater())
            {
                Player.main.playerController.underWaterController.acceleration = 60f;
                Player.main.playerController.underWaterController.verticalMaxSpeed = 75f;
            }
            else
            {
                Player.main.playerController.underWaterController.acceleration = 20;
                Player.main.playerController.underWaterController.verticalMaxSpeed = 5f;
            }
        }        
    }
}
