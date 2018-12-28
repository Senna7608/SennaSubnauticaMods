//#define DEBUG_VEHICLE_OVERDRIVE

#if DEBUG_VEHICLE_OVERDRIVE
using System.Collections;
using Common.DebugHelper;
#endif

using UnityEngine;
using UWE;


namespace CheatManager
{
    public class VehicleOverDrive : MonoBehaviour
    {
        public VehicleOverDrive Instance { get; private set; }
        public Vehicle ThisVehicle { get; private set; }
        public Equipment ThisEquipment { get; private set; }

        internal bool isActive = false;
        private TechType SpeedModule;
        private const float MaxSpeed = 50f;
        private const float SpeedModuleBoost = 4.025f;
        private int SpeedModuleCount = 0;
        private int prevSpeedModuleCount = 0;
        private float prev_seamothmultiplier = 1;
        private float prev_exosuitmultiplier = 1;
        private bool prev_seamothCanFly = false;

        private const float Seamoth_Default_ForwardForce = 11.5f;
        private const float Seamoth_Default_BackwardForce = 5f;
        private const float Seamoth_Default_SidewardForce = 11.5f;
        private const float Seamoth_Default_VerticalForce = 11f;

        private const float Exosuit_Default_ForwardForce = 6f;
        private const float Exosuit_Default_BackwardForce = 2f;
        private const float Exosuit_Default_SidewardForce = 3f;
        private const float Exosuit_Default_VerticalForce = 2f;

#if DEBUG_VEHICLE_OVERDRIVE
        private string vehicleID;        
#endif
        public void Awake()
        {
            Instance = gameObject.GetComponent<VehicleOverDrive>();

            if (Instance.GetComponent<SeaMoth>())
            {
                ThisVehicle = Instance.GetComponent<SeaMoth>();
            }
            else if (Instance.GetComponent<Exosuit>())
            {
                ThisVehicle = Instance.GetComponent<Exosuit>();
            }
            else
            {
                Main.logger.Log("Unknown Vehicle type error! Instance destroyed!");
                Destroy(Instance);
            }

#if DEBUG_VEHICLE_OVERDRIVE
            DebugHelper.DebugVehicle(Instance.GetType().ToString(), "Awake", ThisVehicle);
#endif
            OnPlayerModeChanged(Player.main.GetMode());                  
        }

#if DEBUG_VEHICLE_OVERDRIVE
        private void PrintForces(string callFrom)
        {
            Logger.Log("[CheatManager]\n" +
                $"Object class: {Instance.GetType()}\n" +
                $"Object name: {ThisVehicle.name}\n" +
                $"Object ID: {vehicleID}\n" +
                $"Method name: {callFrom}\n" +                
                $"ForwardForce: {ThisVehicle.forwardForce}\n" +
                $"BackWardForce: {ThisVehicle.backwardForce}\n" +
                $"VerticalForce: {ThisVehicle.verticalForce}\n" +
                $"SidewardForce: {ThisVehicle.sidewardForce}\n" +
                $"SidewaysTorque: {ThisVehicle.sidewaysTorque}");
        }
#endif

        public void Start()
        {
            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);

#if DEBUG_VEHICLE_OVERDRIVE
            vehicleID = Instance.GetInstanceID().ToString();
            PrintForces("Start");            
#endif
            ThisEquipment = ThisVehicle.modules;
            ThisEquipment.onAddItem += OnAddItem;
            ThisEquipment.onRemoveItem += OnRemoveItem;
            Player.main.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
        }

#if DEBUG_VEHICLE_OVERDRIVE
        private IEnumerator CheckForces()
        {
            yield return new WaitForSeconds(10);
            PrintForces("Coroutine");
            if (isActive)
                StartCoroutine(CheckForces());
        }
#endif
        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (Player.main.GetVehicle() == ThisVehicle)
                {
                    isActive = true;

#if DEBUG_VEHICLE_OVERDRIVE
                    StartCoroutine(CheckForces());
#endif
                }
                else
                {
                    isActive = false;                    
                }
            }
            else
            {
                isActive = false;
            }
#if DEBUG_VEHICLE_OVERDRIVE
            print($"OnPlayermodeChanged: {playerMode}, isActive: {isActive}");
#endif
        }

        public void OnDestroy()
        {
            ThisEquipment.onAddItem -= OnAddItem;
            ThisEquipment.onRemoveItem -= OnRemoveItem;
            Player.main.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            Main.Instance.seamothCanFly = false;
            Main.Instance.initToggleButtons = false;
            Destroy(Instance);
        }

        private void OnAddItem(InventoryItem invItem)
        {
            SpeedModuleCount = ThisVehicle.modules.GetCount(SpeedModule);
        }

        private void OnRemoveItem(InventoryItem invItem)
        {
            SpeedModuleCount = ThisVehicle.modules.GetCount(SpeedModule);
        }

        public void Update()
        {
            if (!isActive)
                return;           

            if (Player.main.inSeamoth)            
                Seamoth_SetSpeed();

            if (Player.main.inExosuit)
                Exosuit_SetSpeed();            
        }

        private void Seamoth_SetSpeed()
        {
            if (prev_seamothmultiplier != Main.Instance.seamothSpeedMultiplier || prev_seamothCanFly != Main.Instance.seamothCanFly || prevSpeedModuleCount != SpeedModuleCount)
            {
                float boost = SpeedModuleCount * SpeedModuleBoost;

                if (Main.Instance.seamothSpeedMultiplier == 1)
                {
                    ThisVehicle.forwardForce = Seamoth_Default_ForwardForce + boost;
                    ThisVehicle.backwardForce = Seamoth_Default_BackwardForce;
                    ThisVehicle.sidewardForce = Seamoth_Default_SidewardForce;
                    ThisVehicle.verticalForce = Seamoth_Default_VerticalForce;
                    prev_seamothmultiplier = Main.Instance.seamothSpeedMultiplier;
                }
                else
                {
                    float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                    ThisVehicle.forwardForce = (Seamoth_Default_ForwardForce + boost) + (Main.Instance.seamothSpeedMultiplier * ((overDrive - (Seamoth_Default_ForwardForce + boost)) / 5));
                    ThisVehicle.backwardForce = (Seamoth_Default_BackwardForce + boost) + (Main.Instance.seamothSpeedMultiplier * ((overDrive - (Seamoth_Default_BackwardForce + boost)) / 5));
                    ThisVehicle.sidewardForce = (Seamoth_Default_SidewardForce + boost) + (Main.Instance.seamothSpeedMultiplier * ((overDrive - (Seamoth_Default_SidewardForce + boost)) / 5));
                    ThisVehicle.verticalForce = (Seamoth_Default_VerticalForce + boost) + (Main.Instance.seamothSpeedMultiplier * ((overDrive - (Seamoth_Default_VerticalForce + boost)) / 5));
                    prev_seamothmultiplier = Main.Instance.seamothSpeedMultiplier;
                }

                prevSpeedModuleCount = SpeedModuleCount;
                ThisVehicle.moveOnLand = Main.Instance.seamothCanFly;
                prev_seamothCanFly = Main.Instance.seamothCanFly;

#if DEBUG_VEHICLE_OVERDRIVE
                PrintForces("Update");
#endif
            }

        }

        private void Exosuit_SetSpeed()
        {
            if (prev_exosuitmultiplier != Main.Instance.exosuitSpeedMultiplier || prevSpeedModuleCount != SpeedModuleCount)
            {
                float boost = 0;
                float multiplier = Main.Instance.exosuitSpeedMultiplier;

                if (SpeedModuleCount > 0)
                {
                    boost = SpeedModuleCount * SpeedModuleBoost;
                }

                if (Main.Instance.exosuitSpeedMultiplier == 1)
                {

                    ThisVehicle.forwardForce = Exosuit_Default_ForwardForce + boost;
                    ThisVehicle.backwardForce = Exosuit_Default_BackwardForce;
                    ThisVehicle.sidewardForce = Exosuit_Default_SidewardForce;
                    ThisVehicle.verticalForce = Exosuit_Default_VerticalForce;
                    prev_exosuitmultiplier = multiplier;
                }
                else
                {
                    float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                    ThisVehicle.forwardForce = (Exosuit_Default_ForwardForce + boost) + (multiplier * ((overDrive - (Exosuit_Default_ForwardForce + boost)) / 5));
                    ThisVehicle.backwardForce = (Exosuit_Default_BackwardForce + boost) + (multiplier * ((overDrive - (Exosuit_Default_BackwardForce + boost)) / 5));
                    ThisVehicle.sidewardForce = (Exosuit_Default_SidewardForce + boost) + (multiplier * ((overDrive - (Exosuit_Default_SidewardForce + boost)) / 5));
                    ThisVehicle.verticalForce = (Exosuit_Default_VerticalForce + boost) + (multiplier * ((overDrive - (Exosuit_Default_VerticalForce + boost)) / 5));
                    prev_exosuitmultiplier = multiplier;
                }

                prevSpeedModuleCount = SpeedModuleCount;

#if DEBUG_VEHICLE_OVERDRIVE
                PrintForces("Update");
#endif
            }
        }

    }
}
