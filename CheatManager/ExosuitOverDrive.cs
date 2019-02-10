using UnityEngine;
using UWE;

namespace CheatManager
{
    public class ExosuitOverDrive : MonoBehaviour
    {
        public ExosuitOverDrive Instance { get; private set; }
        public Exosuit ThisExosuit { get; private set; }
        public Equipment ThisEquipment { get; private set; }

        private TechType SpeedModule;
        private const float MaxSpeed = 50f;
        private const float SpeedModuleBoost = 4.025f;
        private int SpeedModuleCount = 0;                

        private const float Exosuit_Default_ForwardForce = 6f;
        private const float Exosuit_Default_BackwardForce = 2f;
        private const float Exosuit_Default_SidewardForce = 3f;
        private const float Exosuit_Default_VerticalForce = 2f;

        public void Awake()
        {
            Instance = gameObject.GetComponent<ExosuitOverDrive>();
            ThisExosuit = Instance.GetComponent<Exosuit>();           
        }

        public void Start()
        {
            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);
            ThisEquipment = ThisExosuit.modules;
            ThisEquipment.onAddItem += OnAddItem;
            ThisEquipment.onRemoveItem += OnRemoveItem;           
            Main.Instance.onExosuitSpeedValueChanged.AddHandler(this, new Event<object>.HandleFunction(OnExosuitSpeedValueChanged));
            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));            
        }

        private void OnExosuitSpeedValueChanged(object newValue)
        {
            Main.Instance.exosuitSpeedMultiplier = (float)newValue;                
            SetExosuitOverDrive(ThisExosuit, (float)newValue);                              
        }        

        public void OnDestroy()
        {
            ThisEquipment.onAddItem -= OnAddItem;
            ThisEquipment.onRemoveItem -= OnRemoveItem;            
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
                    SetExosuitOverDrive(ThisExosuit, Main.Instance.exosuitSpeedMultiplier);                   
                }
            }
        }

        private void OnAddItem(InventoryItem invItem)
        {
            if (invItem.item.GetTechType() == SpeedModule)
            {
                SpeedModuleCount = ThisExosuit.modules.GetCount(SpeedModule);                
            }

            SetExosuitOverDrive(ThisExosuit, Main.Instance.seamothSpeedMultiplier);
        }

        private void OnRemoveItem(InventoryItem invItem)
        {
            if (invItem.item.GetTechType() == SpeedModule)
            {
                SpeedModuleCount = ThisExosuit.modules.GetCount(SpeedModule);                
            }

            SetExosuitOverDrive(ThisExosuit, Main.Instance.seamothSpeedMultiplier);
        }

        internal void SetExosuitOverDrive(Exosuit exosuit, float multiplier)
        {
            float boost = SpeedModuleCount * SpeedModuleBoost;

            if (multiplier == 1)
            {
                exosuit.forwardForce = Exosuit_Default_ForwardForce + boost;
                exosuit.backwardForce = Exosuit_Default_BackwardForce;
                exosuit.sidewardForce = Exosuit_Default_SidewardForce;
                exosuit.verticalForce = Exosuit_Default_VerticalForce;
            }
            else
            {
                float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                exosuit.forwardForce = CalcForce(Exosuit_Default_ForwardForce, boost, overDrive, multiplier);
                exosuit.backwardForce = CalcForce(Exosuit_Default_BackwardForce, boost, overDrive, multiplier);
                exosuit.sidewardForce = CalcForce(Exosuit_Default_SidewardForce, boost, overDrive, multiplier);
                exosuit.verticalForce = CalcForce(Exosuit_Default_VerticalForce, boost, overDrive, multiplier);
            }            
        }

        private float CalcForce(float defaultForce, float boost, float overDrive, float multiplier)
        {
            return defaultForce + boost + (multiplier * ((overDrive - (defaultForce + boost)) / 5));
        }

    }
}
