using System.Collections.Generic;
using Common;
using Common.Helpers;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine;

namespace ModdedArmsHelper.API
{
    public sealed class ArmServices
    {
        static ArmServices()
        {
            SNLogger.Debug("ArmServices created.");
        }

        private ArmServices()
        {
        }

        private static readonly ArmServices _main = new ArmServices();

        public static ArmServices main => _main;

        private ObjectHelper _objectHelper;

        public ObjectHelper objectHelper
        {
            get
            {
                if (_objectHelper == null)
                {
                    _objectHelper = new ObjectHelper();
                }

                return _objectHelper;
            }
        }

        internal Dictionary<CraftableModdedArm, IArmModdingRequest> waitForRegistration = new Dictionary<CraftableModdedArm, IArmModdingRequest>();
       
        internal bool isWaitForRegistration = false;

        public void RegisterArm(CraftableModdedArm armPrefab, IArmModdingRequest armModdingRequest)
        {
            if (!waitForRegistration.ContainsKey(armPrefab))
            {
                waitForRegistration.Add(armPrefab, armModdingRequest);
            }

            isWaitForRegistration = true;
        }

        public bool HasRoomForItem(SeaMoth seamoth, Pickupable pickupable)
        {
            return seamoth.GetComponent<SeamothArmManager>().HasRoomForItem(pickupable);
        }

        public ItemsContainer GetRoomForItem(SeaMoth seamoth, Pickupable pickupable)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetRoomForItem(pickupable);
        }

        public TechType GetLeftArmType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().currentLeftArmType;
        }

        public TechType GetRightArmType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().currentRightArmType;
        }

        public int GetLeftArmSlotID(SeaMoth seamoth)
        {
            return seamoth.GetSlotIndex("SeamothArmLeft");
        }

        public int GetRightArmSlotID(SeaMoth seamoth)
        {
            return seamoth.GetSlotIndex("SeamothArmRight");
        }

        public bool IsArmSlotSelected(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().IsArmSlotSelected;
        }

        public bool IsAnyArmAttached(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().IsAnyArmAttached;
        }

        public TechType GetSelectedArmTechType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetSelectedArmTechType();
        }

        public GameObject GetActiveTarget(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetActiveTarget();
        }
    }
}
