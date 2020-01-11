using Common;
using UnityEngine;

namespace SNTestMOD
{
    public class Radiation_NuclearReactor : MonoBehaviour
    {        
        public DamagePlayerInRadius damagePlayerInRadius;        
        public RadiatePlayerInRange radiatePlayerInRange;
        public PlayerDistanceTracker playerDistanceTracker;

        GameObject RadiationTrigger;


        private void Awake()
        {
            RadiationTrigger = new GameObject("RadiationTrigger");

            RadiationTrigger.transform.SetParent(transform, false);

            playerDistanceTracker = RadiationTrigger.GetOrAddComponent<PlayerDistanceTracker>();
            radiatePlayerInRange = RadiationTrigger.GetOrAddComponent<RadiatePlayerInRange>();
            damagePlayerInRadius = RadiationTrigger.GetOrAddComponent<DamagePlayerInRadius>();            
        }
        
        private void Start()
        {
            radiatePlayerInRange.radiateRadius = 5f;
            damagePlayerInRadius.updateInterval = 1f;
            damagePlayerInRadius.damageAmount = 2f;
            damagePlayerInRadius.damageType = DamageType.Radiation;
            damagePlayerInRadius.damageRadius = 5f;                       
        }

        private void LateUpdate()
        {
            if (Player.main.radiationAmount > 0)
            {
                print($"radiationAmount: {Player.main.radiationAmount}");
            }
        }
    }
}
