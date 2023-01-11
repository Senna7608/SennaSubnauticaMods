namespace ModdedArmsHelper.API.ArmHandlers
{
    /// <summary>
    /// The abstract class to inherit for handling the Propulsion arm base mechanics.
    /// </summary>
    public abstract class PropulsionArmHandler : ArmHandler
    {
#pragma warning disable CS1591 //XML documentation

        private PropulsionCannon _propulsionCannon = null;
        public PropulsionCannon propulsionCannon
        {
            get
            {
                if (_propulsionCannon == null)
                {
                    _propulsionCannon = GetComponent<PropulsionCannon>();
                }

                return _propulsionCannon;
            }
        }                

        public bool usingTool;

#pragma warning restore CS1591 //XML documentation

        /// <summary>
        /// If you implement this method in your modded arm handler, the first line should be 'base.Awake()'
        /// </summary>
        public override void Awake()
        {
            vehicle.gameObject.EnsureComponent<ImmuneToPropulsioncannon>();            
            propulsionCannon.energyInterface = energyInterface;
            propulsionCannon.shootForce = 60f;
            propulsionCannon.attractionForce = 145f;
            propulsionCannon.massScalingFactor = 0.005f;
            propulsionCannon.pickupDistance = 25f;
            propulsionCannon.maxMass = 1800f;
            propulsionCannon.maxAABBVolume = 400f;
        }
    }
}
