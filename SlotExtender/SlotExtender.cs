using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using SMLHelper.V2.Handlers;
using ProtoBuf;

namespace SlotExtender
{
    //Working Modules: SeamothSolarCharge, VehiclePowerUpgradeModule, VehicleArmorPlating,
    //                 VehicleHullModule1, VehicleHullModule2, VehicleHullModule3
    //Working modded modules: SeamothHullModule4, SeamothHullModule5, SpeedModule, SeamothThermalModule

    [ProtoContract]
    public class SlotExtender : MonoBehaviour, IProtoEventListener, IProtoTreeEventListener
    {
        public static SlotExtender Instance { get; private set; }        

        private const int currentVersion = 2;

        //from PrimeSonic's UpgradedVehicles MOD
        private const float SpeedBonusPerModule = 0.35f;
        private const float ForwardForce = 13f;
        private const float OnGroundForceMultiplier = 1f;
        private const float KSpeed = 100f;

        [ProtoMember(1)]
        [NonSerialized]
        public int version;

        [ProtoMember(3, OverwriteList = true)]
        [NonSerialized]
        public SlotExtenderSaveData SaveData;

        private static readonly string[] slotIDs = new string[]
        {
           "SeamothModule1",
           "SeamothModule2",
           "SeamothModule3",
           "SeamothModule4",           
        };

        internal Vehicle vehicleBase;
        internal SeaMoth seamoth;
        public Equipment SlotExtender_equipment { get; private set; }
        public ChildObjectIdentifier EquipmentRoot;
        private EnergyInterface energyInterface;
        internal VehicleUpgradeConsoleInput SlotExtender_upgradesInput;
        internal Dictionary<string, int> SlotExtender_slotIndexes;
        internal static FieldInfo __enginepowerRating;
        internal AnimationCurve curve;

        internal Dictionary<string, TechType> validModules = new Dictionary<string, TechType>
        {
            { "VehicleHullModule1", TechType.VehicleHullModule1 },
            { "VehicleHullModule2", TechType.VehicleHullModule2 },
            { "VehicleHullModule3", TechType.VehicleHullModule3 },
            { "SeamothSolarCharge", TechType.SeamothSolarCharge },
            { "VehiclePowerUpgradeModule", TechType.VehiclePowerUpgradeModule },
            { "VehicleArmorPlating", TechType.VehicleArmorPlating  }
        };

        internal Dictionary<TechType, float> dephtModules = new Dictionary<TechType, float>
        {
            { TechType.VehicleHullModule1, 100f },
            { TechType.VehicleHullModule2, 300f },
            { TechType.VehicleHullModule3, 700f }
        };

        public void Awake()
        {            
            Instance = this;
            seamoth = gameObject.GetComponent<SeaMoth>();
            vehicleBase = seamoth.GetComponent<Vehicle>();
            energyInterface = seamoth.GetComponentInChildren<EnergyInterface>();            

            CheckModdedTechTypes();

            InitializeSlotIndexes();

            if (SaveData == null)
            {
                string id = GetComponentInParent<PrefabIdentifier>().Id;
                SaveData = new SlotExtenderSaveData(id);
            }

            if (SlotExtender_equipment == null)
            {
                InitializeEquipment();
            }           

            gameObject.AddComponent<Access>();
        }

        internal void InitializePhysicalSlots()
        {            
            VehicleUpgradeConsoleInput.Slot[] slots = new VehicleUpgradeConsoleInput.Slot[]
            {
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule5", model = Instantiate(vehicleBase.upgradesInput.slots[0].model, vehicleBase.upgradesInput.slots[0].model.transform.parent) },
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule6", model = Instantiate(vehicleBase.upgradesInput.slots[1].model, vehicleBase.upgradesInput.slots[1].model.transform.parent) },
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule7", model = Instantiate(vehicleBase.upgradesInput.slots[2].model, vehicleBase.upgradesInput.slots[2].model.transform.parent) },
                new VehicleUpgradeConsoleInput.Slot() { id = "SeamothModule8", model = Instantiate(vehicleBase.upgradesInput.slots[3].model, vehicleBase.upgradesInput.slots[3].model.transform.parent) },
            };

            SlotExtender_upgradesInput = Instantiate(vehicleBase.upgradesInput, EquipmentRoot.transform);
            SlotExtender_upgradesInput.slots = slots;
            SlotExtender_upgradesInput.collider.enabled = false;
            SlotExtender_upgradesInput.equipment = SlotExtender_equipment;
        }

        internal void InitializeSlotIndexes()
        {            
            SlotExtender_slotIndexes = new Dictionary<string, int>();

            int num = 0;

            foreach (string text in slotIDs)
            {
                SlotExtender_slotIndexes.Add(slotIDs[num], num);
                num++;
            }
        }

        internal void InitializeEquipment()
        {            
            if (EquipmentRoot == null)
            {
                GameObject equipmentRoot = new GameObject("SlotExtenderRoot");
                equipmentRoot.transform.SetParent(transform, false);
                EquipmentRoot = equipmentRoot.AddComponent<ChildObjectIdentifier>();
            }
            
            SlotExtender_equipment = new Equipment(gameObject, EquipmentRoot.transform);            
            SlotExtender_equipment.SetLabel("SlotExtender");
            SlotExtender_equipment.onEquip += OnEquip;            
            SlotExtender_equipment.onUnequip += OnUnequip;            
            SlotExtender_equipment.AddSlots(slotIDs);
            SlotExtender_equipment.isAllowedToRemove = new IsAllowedToRemove(IsAllowedToRemove);
            SlotExtender_equipment.isAllowedToAdd = new IsAllowedToAdd(IsAllowedToAdd);

            InitializePhysicalSlots();
        }

        internal void RefreshModules()
        {            
            for (int i = 0; i < slotIDs.Length; i++)
            {
                string slot = slotIDs[i];

                TechType techTypeInExtendedSlot = SlotExtender_equipment.GetTechTypeInSlot(slot);                

                if (validModules.ContainsValue(techTypeInExtendedSlot))
                {
                    UpgradeModuleChanged(i, techTypeInExtendedSlot, true);                    
                }                
            }
        }

        internal void CheckModdedTechTypes()
        {            
            if (TechTypeHandler.TryGetModdedTechType("SeamothHullModule4", out TechType SeamothHullModule4))
                validModules.Add("SeamothHullModule4", SeamothHullModule4);

            if (TechTypeHandler.TryGetModdedTechType("SeamothHullModule5", out TechType SeamothHullModule5))
                validModules.Add("SeamothHullModule5", SeamothHullModule5);

            if (TechTypeHandler.TryGetModdedTechType("SpeedModule", out TechType SpeedModule))
                validModules.Add("SpeedModule", SpeedModule);

            if (TechTypeHandler.TryGetModdedTechType("SeamothThermalModule", out TechType SeamothThermalModule))
                validModules.Add("SeamothThermalModule", SeamothThermalModule);

            if (validModules.ContainsKey("SeamothHullModule4"))
            {
                dephtModules.Add(validModules["SeamothHullModule4"], 1100f);
            }
            if (validModules.ContainsKey("SeamothHullModule5"))
            {
                dephtModules.Add(validModules["SeamothHullModule5"], 1500f);
            }
        }


        public void Start()
        {            
            seamoth.modules.onEquip += SyncSlotItems;
            seamoth.modules.onUnequip += SyncSlotItems;
            RefreshModules();
        }

        internal void SyncSlotItems(string slot, InventoryItem item)
        {
            RefreshModules();
        }
             

        internal void UnlockDefaultModuleSlots()
        {            
            SlotExtender_equipment.AddSlots(slotIDs);
        }

        internal void OnEquip(string slot, InventoryItem item)
        {            
            if (SlotExtender_slotIndexes.ContainsKey(slot))
            {
                if (SlotExtender_slotIndexes.TryGetValue(slot, out int slotID))
                {
                    Pickupable item2 = item.item;
                    TechType techType = (!(item2 != null)) ? TechType.None : item2.GetTechType();
                    UpgradeModuleChanged(slotID, techType, true);
                }
            }
        }

        internal void OnUnequip(string slot, InventoryItem item)
        {            
            if (SlotExtender_slotIndexes.ContainsKey(slot))
            {
                if (SlotExtender_slotIndexes.TryGetValue(slot, out int slotID))
                {
                    Pickupable item2 = item.item;
                    TechType techType = (!(item2 != null)) ? TechType.None : item2.GetTechType();
                    UpgradeModuleChanged(slotID, techType, false);
                }
            }            
        }

        internal void OverrideCrusDepht(int slotID, TechType techType, bool added)
        {            
            float num = 0f;

            for (int i = 0; i < slotIDs.Length; i++)
            {
                string slot = slotIDs[i];

                TechType techTypeInExtendedSlot = SlotExtender_equipment.GetTechTypeInSlot(slot);
                TechType techTypeInNormalSlot = seamoth.modules.GetTechTypeInSlot(slot);

                if (dephtModules.ContainsKey(techTypeInExtendedSlot))
                {
                    float num2 = dephtModules[techTypeInExtendedSlot];
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }

                if (dephtModules.ContainsKey(techTypeInNormalSlot))
                {
                    float num3 = dephtModules[techTypeInNormalSlot];
                    if (num3 > num)
                    {
                        num = num3;
                    }
                }
            }

            vehicleBase.crushDamage.SetExtraCrushDepth(num);

            ErrorMessage.AddMessage(Language.main.GetFormat("CrushDepthNow", seamoth.crushDamage.crushDepth));
        }




        internal void UpgradeModuleChanged(int slotID, TechType techType, bool added)
        {            
            if (dephtModules.ContainsKey(techType))
            {
                OverrideCrusDepht(slotID, techType, added);
                return;
            }
                       
            switch (techType)
            {
                case TechType.SeamothReinforcementModule:
                    break;
                case TechType.SeamothSolarCharge:
                    int count = SlotExtender_equipment.GetCount(techType);
                    CancelInvoke("UpdateSolarRecharge");                    
                    if (count > 0)
                    {
                        InvokeRepeating("UpdateSolarRecharge", 1f, 1f);                        
                    }
                    if (added)
                    {
                        ErrorMessage.AddMessage("Solar Charger now active");
                    }
                    else
                    {
                        ErrorMessage.AddMessage("Solar Charger removed");
                    }
                    return;
            }

            OnUpgradeModuleChange(slotID, techType, added);
            return;            
        }




        internal void OnUpgradeModuleChange(int slotID, TechType techType, bool added)
        {            
            int count = SlotExtender_equipment.GetCount(techType) + seamoth.modules.GetCount(techType);

            if (techType == TechType.VehiclePowerUpgradeModule)
            {
                __enginepowerRating = typeof(Vehicle).GetField("enginePowerRating", BindingFlags.NonPublic | BindingFlags.Instance);
                __enginepowerRating.SetValue(vehicleBase, 1f + 1f * (float)count);
                ErrorMessage.AddMessage(Language.main.GetFormat("PowerRatingNowFormat", 1f + 1f * (float)count));
            }
            else if (techType == TechType.VehicleArmorPlating)
            {                    
                DealDamageOnImpact component = seamoth.GetComponent<DealDamageOnImpact>();                    
                component.mirroredSelfDamageFraction = 0.5f * Mathf.Pow(0.5f, (float)count);
                ErrorMessage.AddMessage($"Armor rating is now {1f / component.mirroredSelfDamageFraction}");
            }           
            else if (techType == validModules["SpeedModule"])
            {
                UpdateSpeedRating(vehicleBase, count, true);
            }
            else if (techType == validModules["SeamothThermalModule"])
            {
                CancelInvoke("UpdateThermalRecharge");
                if (count > 0)
                {
                    InvokeRepeating("UpdateThermalRecharge", 1f, 1f);
                }
                if (added)
                {
                    ErrorMessage.AddMessage("Thermalr Charger now active");
                }
                else
                {
                    ErrorMessage.AddMessage("Thermal Charger removed");
                }
            }
        }
                
        internal void UpdateThermalRecharge()
        {            
            float temperature = seamoth.GetTemperature();
            curve = new AnimationCurve(new Keyframe(60f, 1f, 0.05194809f, 0.05194809f), new Keyframe(30f, 0f, 0.004784686f, 0.004784686f));
            float amount = curve.Evaluate(temperature);
            energyInterface.AddEnergy(amount);                
        }
        
        internal void UpdateSolarRecharge()
        {            
            DayNightCycle main = DayNightCycle.main;
            if (main == null)
            {
                return;
            }
            int count = SlotExtender_equipment.GetCount(TechType.SeamothSolarCharge);
            float num = Mathf.Clamp01((200f + seamoth.transform.position.y) / 200f);
            float localLightScalar = main.GetLocalLightScalar();
            float amount = 1f * localLightScalar * num * count;
            energyInterface.AddEnergy(amount);
        }


        //from PrimeSonic's UpgradedVehicles MOD
        internal void UpdateSpeedRating(Vehicle vehicle, int speedBoosterCount, bool announement)
        {            
            float speedMultiplier = 1f + speedBoosterCount * SpeedBonusPerModule;

            vehicle.forwardForce = speedMultiplier * ForwardForce;
            vehicle.onGroundForceMultiplier = speedMultiplier * OnGroundForceMultiplier;

            var motor = vehicle.GetComponent<VehicleMotor>();
            if (motor != null)
            {
                motor.kSpeedScalar = speedMultiplier * KSpeed;
                motor.kMaxSpeed = speedMultiplier * KSpeed;
            }

            if (announement)
                ErrorMessage.AddMessage($"Speed rating is now at {speedMultiplier * 100:00}%");
        }


        internal bool IsAllowedToAdd(Pickupable pickupable, bool verbose)
        {           
            TechType techType = pickupable.GetTechType();
            if (validModules.ContainsValue(techType))
                return true;           

            return false;
        }


        internal bool IsAllowedToRemove(Pickupable pickupable, bool verbose)
        {            
            return true;
        }

        //Original code found on PrimeSonic's MoreCyclopsUpgrades MOD 
        public void OnProtoSerialize(ProtobufSerializer serializer)
        {           
            version = 2;

            foreach (var slot in slotIDs)
            {
                EmModuleSaveData savedModule = SaveData.GetModuleInSlot(slot);
                InventoryItem item = SlotExtender_equipment.GetItemInSlot(slot);

                if (item == null)
                {
                    savedModule.ItemID = (int)TechType.None;                    
                }
                else
                {
                    savedModule.ItemID = (int)item.item.GetTechType();                    
                }

            }

            SaveData.Save();
        }

        //Original code found on PrimeSonic's MoreCyclopsUpgrades MOD 
        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {            
            if (SlotExtender_equipment == null)
                InitializeEquipment();

            SlotExtender_equipment.Clear();
        }


        public void OnProtoSerializeObjectTree(ProtobufSerializer serializer)
        {
        }

        //Original code found on PrimeSonic's MoreCyclopsUpgrades MOD 
        public void OnProtoDeserializeObjectTree(ProtobufSerializer serializer)
        {            
            bool hasSaveData = SaveData.Load();
            if (hasSaveData)
            {
                // Because the items here aren't being serialized with everything else normally,
                // I've used custom save data to handle whatever gets left in these slots.

                // The following is a recreation of the essential parts of the Equipment.ResponseEquipment method.
                foreach (string slot in slotIDs)
                {
                    // These slots need to be added before we can add items to them
                    SlotExtender_equipment.AddSlot(slot);

                    EmModuleSaveData savedModule = SaveData.GetModuleInSlot(slot);

                    if (savedModule.ItemID == (int)TechType.None)
                        continue;

                    InventoryItem spanwedItem = SpawnModule((TechType)savedModule.ItemID);

                    if (spanwedItem is null)
                        continue;

                    SlotExtender_equipment.AddItem(slot, spanwedItem, true);
                }
            }
            else
            {
                UnlockDefaultModuleSlots();
            }
        }

        //Original code found on PrimeSonic's MoreCyclopsUpgrades MOD 
        internal static InventoryItem SpawnModule(TechType techTypeID)
        {            
            GameObject gO;

            gO = Instantiate(CraftData.GetPrefabForTechType(techTypeID));

            Pickupable pickupable = gO.GetComponent<Pickupable>().Pickup(false);
            return new InventoryItem(pickupable);
        } 

    }
}
