//#define DEBUG_SEAGLIDE_OVERDRIVE

using UnityEngine;
using UWE;

namespace CheatManager
{
    public class SeaglideOverDrive : MonoBehaviour
    {
        public static SeaglideOverDrive Main { get; private set; }       
              
        public void Awake()
        {
            if (gameObject.GetComponent<Seaglide>() == null)
            {
                Destroy(this);
            }
            else
            {
                Main = this;
            }
        }

        public void Start()
        { 
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(PlayerIsSeaglideUse));            
        }       

        private void PlayerIsSeaglideUse(Player.MotorMode motorMode)
        {           
            SetSeaglideSpeed();            
        }

        public void SetSeaglideSpeed()
        {
#if DEBUG_SEAGLIDE_OVERDRIVE
            Logger.Log($"SetSeaGlideSpeed():\nMotorMode: {Player.main.motorMode}\nisUnderWater: {Player.main.IsUnderwater()}\nisSeaglideFast: {CheatManager.instance.isSeaglideFast.value}");
#endif          
            if (CheatManager.Instance.isSeaglideFast.value && Player.main.motorMode == Player.MotorMode.Seaglide && Player.main.IsUnderwater())
            {
                Player.main.playerController.underWaterController.acceleration = 60f;
                Player.main.playerController.underWaterController.verticalMaxSpeed = 75f;

#if DEBUG_SEAGLIDE_OVERDRIVE
                Logger.Log($"True\nacceleration: {Player.main.playerController.underWaterController.acceleration}\nverticalMaxSpeed: {Player.main.playerController.underWaterController.verticalMaxSpeed}");
#endif
            }
            else
            {
                Player.main.playerController.underWaterController.acceleration = 20;
                Player.main.playerController.underWaterController.verticalMaxSpeed = 5f;

#if DEBUG_SEAGLIDE_OVERDRIVE
                Logger.Log($"False\nacceleration: {Player.main.playerController.underWaterController.acceleration}\nverticalMaxSpeed: {Player.main.playerController.underWaterController.verticalMaxSpeed}");
#endif
            }
        }        
    }
}
