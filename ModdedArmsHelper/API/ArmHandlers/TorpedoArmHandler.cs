using Common.Helpers;
using UnityEngine;

namespace ModdedArmsHelper.API.ArmHandlers
{
    /// <summary>
    /// The abstract class to inherit for handling the Torpedo arm base mechanics.
    /// </summary>
    public abstract class TorpedoArmHandler : ArmHandler
    {
#pragma warning disable CS1591 //XML documentation

        public GenericHandTarget handTarget { get; private set; }        
        public Transform siloFirst { get; private set; }
        public Transform siloSecond { get; private set; }
        public GameObject visualTorpedoFirst { get; private set; }
        public GameObject visualTorpedoSecond { get; private set; }
        public GameObject visualTorpedoReload { get; private set; }
        public FMODAsset fireSound { get; private set; }
        public FMODAsset torpedoDisarmed { get; private set; }

        public ItemsContainer container;
        public float cooldownTime = 5f;
        public float cooldownInterval = 1f;
        public float timeFirstShot = float.NegativeInfinity;
        public float timeSecondShot = float.NegativeInfinity;

#pragma warning restore CS1591 //XML documentation

        /// <summary>
        /// If you implement this method in your modded arm handler, the first line should be 'base.Awake()'
        /// </summary>
        public override void Awake()
        {
            ObjectHelper objectHelper = ArmServices.main.objectHelper;           

            siloFirst = objectHelper.FindDeepChild(gameObject, "TorpedoSiloFirst").transform;
            siloSecond = objectHelper.FindDeepChild(gameObject, "TorpedoSiloSecond").transform;

            visualTorpedoFirst = objectHelper.FindDeepChild(gameObject, "TorpedoFirst");
            visualTorpedoSecond = objectHelper.FindDeepChild(gameObject, "TorpedoSecond");
            visualTorpedoReload = objectHelper.FindDeepChild(gameObject, "TorpedoReload");

            handTarget = GetComponentInChildren<GenericHandTarget>(true);            

            fireSound = ScriptableObject.CreateInstance<FMODAsset>();
            fireSound.path = "event:/sub/seamoth/torpedo_fire";
            fireSound.name = "torpedo_fire";

            torpedoDisarmed = ScriptableObject.CreateInstance<FMODAsset>();
            torpedoDisarmed.path = "event:/sub/seamoth/torpedo_disarmed";
            torpedoDisarmed.name = "torpedo_disarmed";
        }

        /// <summary>
        /// If you implement this method in your modded arm handler, the first line should be 'base.Awake()'
        /// </summary>
        public TechType[] GetTorpedoTypes()
        {
            TechType[] techTypes = new TechType[vehicle.torpedoTypes.Length];

            for (int i = 0; i < vehicle.torpedoTypes.Length; i++)
            {
                techTypes[i] = vehicle.torpedoTypes[i].techType;
            }

            return techTypes;
        }
    }
}
