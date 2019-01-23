﻿using UnityEngine;
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
                Debug.Log("Unknown Vehicle type error! Instance destroyed!");
                Destroy(Instance);
            }

            OnPlayerModeChanged(Player.main.GetMode());                  
        }

        public void Start()
        {
            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);
            ThisEquipment = ThisVehicle.modules;
            ThisEquipment.onAddItem += OnAddItem;
            ThisEquipment.onRemoveItem += OnRemoveItem;
            Player.main.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
        }

        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (Player.main.GetVehicle() == ThisVehicle)
                {
                    isActive = true;
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
                float multiplier = Main.Instance.seamothSpeedMultiplier;

                if (multiplier == 1)
                {
                    ThisVehicle.forwardForce = Seamoth_Default_ForwardForce + boost;
                    ThisVehicle.backwardForce = Seamoth_Default_BackwardForce;
                    ThisVehicle.sidewardForce = Seamoth_Default_SidewardForce;
                    ThisVehicle.verticalForce = Seamoth_Default_VerticalForce;
                    prev_seamothmultiplier = multiplier;
                }
                else
                {
                    float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                    ThisVehicle.forwardForce = CalcForce(Seamoth_Default_ForwardForce, boost, overDrive, multiplier);
                    ThisVehicle.backwardForce = CalcForce(Seamoth_Default_BackwardForce, boost, overDrive, multiplier);
                    ThisVehicle.sidewardForce = CalcForce(Seamoth_Default_SidewardForce, boost, overDrive, multiplier);
                    ThisVehicle.verticalForce = CalcForce(Seamoth_Default_VerticalForce, boost, overDrive, multiplier);                   
                    prev_seamothmultiplier = multiplier;
                }

                prevSpeedModuleCount = SpeedModuleCount;
                ThisVehicle.moveOnLand = Main.Instance.seamothCanFly;
                prev_seamothCanFly = Main.Instance.seamothCanFly;
            }

        }

        private void Exosuit_SetSpeed()
        {
            if (prev_exosuitmultiplier != Main.Instance.exosuitSpeedMultiplier || prevSpeedModuleCount != SpeedModuleCount)
            {
                float boost = SpeedModuleCount * SpeedModuleBoost;
                float multiplier = Main.Instance.exosuitSpeedMultiplier;                

                if (multiplier == 1)
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
                    ThisVehicle.forwardForce = CalcForce(Exosuit_Default_ForwardForce, boost, overDrive, multiplier);
                    ThisVehicle.backwardForce = CalcForce(Exosuit_Default_BackwardForce, boost, overDrive, multiplier);
                    ThisVehicle.sidewardForce = CalcForce(Exosuit_Default_SidewardForce, boost, overDrive, multiplier);
                    ThisVehicle.verticalForce = CalcForce(Exosuit_Default_VerticalForce, boost, overDrive, multiplier);                    
                    prev_exosuitmultiplier = multiplier;
                }

                prevSpeedModuleCount = SpeedModuleCount;
            }
        }

        private float CalcForce(float defaultForce, float boost, float overDrive, float multiplier)
        {
          return  defaultForce + boost + (multiplier * ((overDrive - (defaultForce + boost)) / 5));
        }

    }
}
