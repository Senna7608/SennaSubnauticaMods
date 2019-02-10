using UnityEngine;
using UWE;

namespace CheatManager
{
    public class SeamothOverDrive : MonoBehaviour
    {
        public SeamothOverDrive Instance { get; private set; }
        public SeaMoth ThisSeamoth { get; private set; }
        public Equipment ThisEquipment { get; private set; }

        private TechType SpeedModule;
        private const float MaxSpeed = 50f;
        private const float SpeedModuleBoost = 4.025f;
        private int SpeedModuleCount = 0;        

        private const float Seamoth_Default_ForwardForce = 11.5f;
        private const float Seamoth_Default_BackwardForce = 5f;
        private const float Seamoth_Default_SidewardForce = 11.5f;
        private const float Seamoth_Default_VerticalForce = 11f;        

        public void Awake()
        {
            Instance = gameObject.GetComponent<SeamothOverDrive>();
            ThisSeamoth = Instance.GetComponent<SeaMoth>();            
        }

        public void Start()
        {
            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);
            ThisEquipment = ThisSeamoth.modules;
            ThisEquipment.onAddItem += OnAddItem;
            ThisEquipment.onRemoveItem += OnRemoveItem;
            Main.Instance.isSeamothCanFly.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(OnSeamothCanFlyChanged));
            Main.Instance.onSeamothSpeedValueChanged.AddHandler(this, new Event<object>.HandleFunction(OnSeamothSpeedValueChanged));            
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));            
        }

        private void OnSeamothSpeedValueChanged(object newValue)
        {           
            Main.Instance.seamothSpeedMultiplier = (float)newValue;
            SetSeamothOverDrive(ThisSeamoth, (float)newValue);                     
        }        

        private void OnSeamothCanFlyChanged(Utils.MonitoredValue<bool> parms)
        {
            SetSeamothToFly(ThisSeamoth, Main.Instance.isSeamothCanFly.value);
        }

        public void OnDestroy()
        {
            ThisEquipment.onAddItem -= OnAddItem;
            ThisEquipment.onRemoveItem -= OnRemoveItem;
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
                    SetSeamothOverDrive(ThisSeamoth, Main.Instance.seamothSpeedMultiplier);                   
                }
            }
        }

        private void OnAddItem(InventoryItem invItem)
        {
            if (invItem.item.GetTechType() == SpeedModule)
            {
                SpeedModuleCount = ThisSeamoth.modules.GetCount(SpeedModule);                
            }

            SetSeamothOverDrive(ThisSeamoth, Main.Instance.seamothSpeedMultiplier);
        }

        private void OnRemoveItem(InventoryItem invItem)
        {
            if (invItem.item.GetTechType() == SpeedModule)
            {
                SpeedModuleCount = ThisSeamoth.modules.GetCount(SpeedModule);                
            }

            SetSeamothOverDrive(ThisSeamoth, Main.Instance.seamothSpeedMultiplier);
        }

        internal void SetSeamothToFly(SeaMoth seamoth, bool canFly)
        {
            seamoth.moveOnLand = canFly;
        }

        internal void SetSeamothOverDrive(SeaMoth seamoth, float multiplier)
        {
            float boost = SpeedModuleCount * SpeedModuleBoost;

            if (multiplier == 1)
            {
                seamoth.forwardForce = Seamoth_Default_ForwardForce + boost;
                seamoth.backwardForce = Seamoth_Default_BackwardForce;
                seamoth.sidewardForce = Seamoth_Default_SidewardForce;
                seamoth.verticalForce = Seamoth_Default_VerticalForce;
            }
            else
            {
                float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                seamoth.forwardForce = CalcForce(Seamoth_Default_ForwardForce, boost, overDrive, multiplier);
                seamoth.backwardForce = CalcForce(Seamoth_Default_BackwardForce, boost, overDrive, multiplier);
                seamoth.sidewardForce = CalcForce(Seamoth_Default_SidewardForce, boost, overDrive, multiplier);
                seamoth.verticalForce = CalcForce(Seamoth_Default_VerticalForce, boost, overDrive, multiplier);
            }            
        }        

        private float CalcForce(float defaultForce, float boost, float overDrive, float multiplier)
        {
            return defaultForce + boost + (multiplier * ((overDrive - (defaultForce + boost)) / 5));
        }

    }
}
