namespace ModdedArmsHelper.API.ArmHandlers
{
    public abstract class PropulsionArmHandler : ArmHandler
    {
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
