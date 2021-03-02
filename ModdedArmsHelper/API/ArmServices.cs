using System.Collections.Generic;
using Common;
using Common.Helpers;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine;
#pragma warning disable IDE1006 //Naming styles

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The defining helper class for all new modded arms. This class is a Singleton.
    /// </summary>
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


        /// <summary>
        /// A property that can be used to access this class.
        /// </summary>
        public static ArmServices main => _main;

        private ObjectHelper _objectHelper;

        /// <summary>
        /// A property that can be used to access the <see cref="ObjectHelper"/> helper class.
        /// </summary>
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

        /// <summary>
        /// This method can be used to register the new modded arm.
        /// </summary>
        /// <remarks>
        /// Call it from the <see cref="CraftableModdedArm.RegisterArm"/> method.
        /// </remarks>
        public void RegisterArm(CraftableModdedArm armPrefab, IArmModdingRequest armModdingRequest)
        {
            if (!waitForRegistration.ContainsKey(armPrefab))
            {
                waitForRegistration.Add(armPrefab, armModdingRequest);
            }

            isWaitForRegistration = true;
        }

        /// <summary>
        /// This method examines whether there is enough space for the <see cref="Pickupable"/> item in the <see cref="SeaMoth"/> storage modules.
        /// </summary>
        /// <returns>
        /// <c>true</c> if any of <see cref="SeaMoth"/> storage module have an enough space for the <see cref="Pickupable"/> item.<br/>Otherwise, returns <c>false</c>. 
        /// </returns>
        public bool HasRoomForItem(SeaMoth seamoth, Pickupable pickupable)
        {
            return seamoth.GetComponent<SeamothArmManager>().HasRoomForItem(pickupable);
        }

        /// <summary>
        /// This method is examine if any of the <see cref="SeaMoth"/> storage module that can fits the <see cref="Pickupable"/> item.
        /// </summary>
        /// <returns>
        /// an <see cref="ItemsContainer"/> if any of <see cref="SeaMoth"/> storage module have an enough space for the <see cref="Pickupable"/> item.<br/>Otherwise, returns <c>null</c>. 
        /// </returns>
        public ItemsContainer GetRoomForItem(SeaMoth seamoth, Pickupable pickupable)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetRoomForItem(pickupable);
        }

        /// <summary>
        /// This method is examine of the the current mounted left arm <see cref="TechType"/> of the <see cref="SeaMoth"/>.
        /// </summary>
        /// <returns>
        /// If any arm mounted in the left arm socket, returns the arm <see cref="TechType"/>.<br/>Otherwise, returns <see cref="TechType.None"/>. 
        /// </returns>
        public TechType GetLeftArmType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().currentLeftArmType;
        }

        /// <summary>
        /// This method is examine of the current mounted right arm <see cref="TechType"/> of the <see cref="SeaMoth"/>.
        /// </summary>
        /// <returns>
        /// If any arm mounted in the right arm socket, returns the arm <see cref="TechType"/>.<br/>Otherwise, returns <see cref="TechType.None"/>. 
        /// </returns>
        public TechType GetRightArmType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().currentRightArmType;
        }

        /// <summary>
        /// This method is examine of the <see cref="SeaMoth"/> left arm slot.
        /// </summary>
        /// <returns>
        /// If any arm mounted in the left arm slot, returns the arm slot ID.<br/>Otherwise, returns <c>-1</c>. 
        /// </returns>
        public int GetLeftArmSlotID(SeaMoth seamoth)
        {
            return seamoth.GetSlotIndex("SeamothArmLeft");
        }

        /// <summary>
        /// This method is examine of the <see cref="SeaMoth"/> right arm slot.
        /// </summary>
        /// <returns>
        /// If any arm mounted in the right arm slot, returns the arm slot ID.<br/>Otherwise, returns <c>-1</c>. 
        /// </returns>
        public int GetRightArmSlotID(SeaMoth seamoth)
        {
            return seamoth.GetSlotIndex("SeamothArmRight");
        }

        /// <summary>
        /// This method is examine if any arm selected in the quickslot bar.
        /// </summary>
        /// <returns>
        /// If any arm selected in the quickslot bar, returns <c>true</c>.<br/>Otherwise, returns <c>false</c>. 
        /// </returns>
        public bool IsArmSlotSelected(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().IsArmSlotSelected;
        }

        /// <summary>
        /// This method is examine if any arm attached to any of the <see cref="SeaMoth"/> arm slots.
        /// </summary>
        /// <returns>
        /// If any arm attached to any of the <see cref="SeaMoth"/> arm slots, returns <c>true</c>.<br/>Otherwise, returns <c>false</c>. 
        /// </returns>
        public bool IsAnyArmAttached(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().IsAnyArmAttached;
        }

        /// <summary>
        /// This method is examine for the selected arm <see cref="TechType"/>.
        /// </summary>
        /// <returns>
        /// If any arm selected in the quickslot bar, returns the arm <see cref="TechType"/>.<br/>Otherwise, returns <see cref="TechType.None"/>. 
        /// </returns>
        public TechType GetSelectedArmTechType(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetSelectedArmTechType();
        }

        /// <summary>
        /// This method is examine of the current active target on crosshair.
        /// </summary>
        /// <returns>
        /// If current active target not null, returns the target <see cref="GameObject"/>.<br/>Otherwise, returns <c>null</c>. 
        /// </returns>
        public GameObject GetActiveTarget(SeaMoth seamoth)
        {
            return seamoth.GetComponent<SeamothArmManager>().GetActiveTarget();
        }
    }
}
