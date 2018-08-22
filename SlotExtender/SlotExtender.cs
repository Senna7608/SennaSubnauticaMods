using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace SlotExtender
{
    //Working Modules: SeamothSolarCharge, VehiclePowerUpgradeModule, VehicleArmorPlating,
    //                 VehicleHullModule1, VehicleHullModule2, VehicleHullModule3

    public class SlotExtender : MonoBehaviour
    {
        public static SlotExtender Instance { get; private set; }

        private static readonly string[] slotIDs = new string[]
        {
           "SeamothModule1",
           "SeamothModule2",
           "SeamothModule3",
           "SeamothModule4",           
        };
        
        private Vehicle vehicleBase;
        private SeaMoth seamoth;        
        
        private Equipment equipment;
        private EnergyInterface energyInterface;
        private VehicleUpgradeConsoleInput upgradesInput;              
        private Dictionary<string, int> slotIndexes;
        private FieldInfo __enginepowerRating;         
        
        public void Awake()
        {
            Instance = this;           
            seamoth = gameObject.GetComponent<SeaMoth>();
            vehicleBase = seamoth.GetComponent<Vehicle>();
            energyInterface = seamoth.GetComponentInChildren<EnergyInterface>();
        }        

        public void Update()
        {
            // Working only with one Seamoth! if two or more: call every upgradesInput in the hierarchy but the upper one active only
            if (Player.main.inSeamoth)
            {
                if(Input.GetKeyDown(KeyCode.T))
                {
                    upgradesInput.OpenFromExternal();
                }
            }
        }        

        public void Start()
        {
            vehicleBase = seamoth.GetComponent<Vehicle>();
            energyInterface = seamoth.GetComponent<EnergyInterface>();            

            slotIndexes = new Dictionary<string, int>();          
            
            int num = 0;

            foreach (string text in slotIDs)
            {                
                slotIndexes.Add(slotIDs[num], num);
                num++;
            }
            
            //this is the copy of invisible 'physical' slots for modules: if not copy the slot model the slots are mixed and only four may active             
            VehicleUpgradeConsoleInput.Slot[] slots = new VehicleUpgradeConsoleInput.Slot[]
            {
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule1", model = Instantiate(vehicleBase.upgradesInput.slots[0].model, vehicleBase.upgradesInput.slots[0].model.transform.parent) },        
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule2", model = Instantiate(vehicleBase.upgradesInput.slots[1].model, vehicleBase.upgradesInput.slots[1].model.transform.parent) },
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule3", model = Instantiate(vehicleBase.upgradesInput.slots[2].model, vehicleBase.upgradesInput.slots[2].model.transform.parent) },
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule4", model = Instantiate(vehicleBase.upgradesInput.slots[3].model, vehicleBase.upgradesInput.slots[3].model.transform.parent) },
            };                      
            
            upgradesInput = Instantiate(vehicleBase.upgradesInput, seamoth.transform);
            upgradesInput.slots = slots;

            //Disabled the collider for Cyclops incopatibility.
            //If enabled: when docked the Seamoth the cyclops physics motor will go crazy spin while destroy oneself.
            upgradesInput.collider.enabled = false;

            equipment = new Equipment(seamoth.gameObject, vehicleBase.modules.tr);
            equipment.tr.SetParent(seamoth.transform);
            equipment.SetLabel("SlotExtender");
            equipment.onEquip += OnEquip;
            equipment.onUnequip += OnUnequip;
            equipment.AddSlots(slotIDs);
            equipment.isAllowedToRemove = new IsAllowedToRemove(IsAllowedToRemove);
            equipment.isAllowedToAdd = new IsAllowedToAdd(IsAllowedToAdd);
            upgradesInput.equipment = equipment;            
        }        

        protected void OnEquip(string slot, InventoryItem item)
        {
            if (slotIndexes.ContainsKey(slot))
            {
                if (slotIndexes.TryGetValue(slot, out int slotID))
                {
                    Pickupable item2 = item.item;
                    TechType techType = (!(item2 != null)) ? TechType.None : item2.GetTechType();
                    UpgradeModuleChanged(slotID, techType, true);
                }
            }
        }

        protected void OnUnequip(string slot, InventoryItem item)
        {
            if(slotIndexes.ContainsKey(slot))
            {
                if (slotIndexes.TryGetValue(slot, out int slotID))
                {
                    Pickupable item2 = item.item;
                    TechType techType = (!(item2 != null)) ? TechType.None : item2.GetTechType();
                    UpgradeModuleChanged(slotID, techType, false);
                }
            }            
        }

        protected void UpgradeModuleChanged(int slotID, TechType techType, bool added)
        { 
            int count = equipment.GetCount(techType);
           
            switch (techType)
            {
                case TechType.VehicleHullModule1:
                case TechType.VehicleHullModule2:
                case TechType.VehicleHullModule3:
                    break;
                default:
                    switch (techType)
                    {
                        case TechType.SeamothReinforcementModule:
                            break;
                        case TechType.SeamothSolarCharge:
                           CancelInvoke("UpdateSolarRecharge");                           
                            if (count > 0)
                            {
                                InvokeRepeating("UpdateSolarRecharge", 1f, 1f);                                
                            }

                            return;
                    }                    
                    OnUpgradeModuleChange(slotID, techType, added);
                    return;
            }
            
            Dictionary<TechType, float> dictionary = new Dictionary<TechType, float>
            {               
                { TechType.VehicleHullModule1, 100f },
                { TechType.VehicleHullModule2, 300f },
                { TechType.VehicleHullModule3, 700f }
            };

            float num = 0f;
            for (int i = 0; i < slotIDs.Length; i++)
            {
                string slot = slotIDs[i];
                TechType techTypeInSlot = equipment.GetTechTypeInSlot(slot);
                if (dictionary.ContainsKey(techTypeInSlot))
                {
                    float num2 = dictionary[techTypeInSlot];
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }
            }
            vehicleBase.crushDamage.SetExtraCrushDepth(num);            
        }        

        protected void OnUpgradeModuleChange(int slotID, TechType techType, bool added)
        {
            int count = equipment.GetCount(techType);

            if (techType != TechType.VehiclePowerUpgradeModule)
            {
                if (techType == TechType.VehicleArmorPlating)
                {                    
                    DealDamageOnImpact component = seamoth.GetComponent<DealDamageOnImpact>();                    
                    component.mirroredSelfDamageFraction = 0.5f * Mathf.Pow(0.5f, (float)count);                    
                }
            }
            else
            {
                __enginepowerRating = typeof(Vehicle).GetField("enginePowerRating", BindingFlags.NonPublic | BindingFlags.Instance);               
                __enginepowerRating.SetValue(vehicleBase, 1f + 1f * count);                
            }            
        }

        protected void UpdateSolarRecharge()
        {
            DayNightCycle main = DayNightCycle.main;
            if (main == null)
            {
                return;
            }
            int count = equipment.GetCount(TechType.SeamothSolarCharge);
            float num = Mathf.Clamp01((200f + seamoth.transform.position.y) / 200f);
            float localLightScalar = main.GetLocalLightScalar();
            float amount = 1f * localLightScalar * num * count;
            energyInterface.AddEnergy(amount);
        }

        protected bool IsAllowedToAdd(Pickupable pickupable, bool verbose)
        {
            TechType techType = pickupable.GetTechType();

            switch (techType)
            {
                case TechType.SeamothSolarCharge:
                case TechType.VehicleArmorPlating:
                case TechType.VehicleHullModule1:
                case TechType.VehicleHullModule2:
                case TechType.VehicleHullModule3:
                case TechType.VehiclePowerUpgradeModule:
                    return true;
            }
            return false;
        }


        protected bool IsAllowedToRemove(Pickupable pickupable, bool verbose)
        {  
            return true;
        }

    }
}
