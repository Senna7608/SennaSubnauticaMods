using Common;
using UnityEngine;

namespace SNTestMOD
{
    public class Radiation_UraniniteCrystal : MonoBehaviour
    {
        private PlayerDistanceTracker tracker;

        private const float radiateRadius = 5f;
        private const float damageAmount = 2f;
        private const float updateInterval = 1f;

        public bool enter = false;
        public bool exit = false;

        private void Awake()
        {
            Debug.Log($"Awake({GetInstanceID()})");

            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = radiateRadius;

            tracker = gameObject.EnsureComponent<PlayerDistanceTracker>();
            tracker.maxDistance = radiateRadius + 5f;
        }

        private void Start()
        {
            Debug.Log($"Start({GetInstanceID()})");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!enter && IsPlayerCollided(other))
            {
                Main.isPlayerInRadiationZone = true;

                Debug.Log($"entered({GetInstanceID()})");

                if (IsAuroraLeaking() && !Main.isAuroraRadiationBypassed)
                {
                    LeakingRadiation.main.radiatePlayerInRange.CancelInvoke();
                    Main.isAuroraRadiationBypassed = true;
                }

                bool PlayerHasImmuneToRadiation = GameModeUtils.HasRadiation() && (NoDamageConsoleCommand.main == null || !NoDamageConsoleCommand.main.GetNoDamageCheat());

                if (PlayerHasImmuneToRadiation)
                {
                    InvokeRepeating("RadiateUranuim", 0f, 0.2f);
                    InvokeRepeating("DoDamage", 0f, updateInterval);
                }
                
                enter = true;
                exit = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!exit && IsPlayerCollided(other))
            {
                Debug.Log($"exited({GetInstanceID()})");

                CancelInvoke();

                if (IsAuroraLeaking() && Main.isAuroraRadiationBypassed && !Main.isPlayerInRadiationZone)
                {
                    LeakingRadiation.main.radiatePlayerInRange.InvokeRepeating("Radiate", 0f, 0.2f);
                    Main.isAuroraRadiationBypassed = false;
                }

                Main.isPlayerInRadiationZone = false;
                enter = false;
                exit = true;
            }
        }

        public void OnDestroy()
        {
            CancelInvoke();

            if (IsAuroraLeaking() && Main.isAuroraRadiationBypassed && !Main.isPlayerInRadiationZone)
            {
                LeakingRadiation.main.radiatePlayerInRange.InvokeRepeating("Radiate", 0f, 0.2f);
                Main.isAuroraRadiationBypassed = false;
            }

            Debug.Log($"OnDestroy({GetInstanceID()})");
        }

        private void RadiateUranuim()
        {
            float num = Mathf.Clamp01((1f - tracker.distanceToPlayer / radiateRadius));

            float num2 = num;

            if (Inventory.main.equipment.GetCount(TechType.RadiationSuit) > 0)
            {
                num -= num2 * 0.5f;
            }

            if (Inventory.main.equipment.GetCount(TechType.RadiationHelmet) > 0)
            {
                num -= num2 * 0.23f * 2f;
            }

            if (Inventory.main.equipment.GetCount(TechType.RadiationGloves) > 0)
            {
                num -= num2 * 0.23f;
            }

            num = Mathf.Clamp01(num);

            Player.main.SetRadiationAmount(num);
        }

        private void DoDamage()
        {
            if (tracker.distanceToPlayer <= radiateRadius)
            {
                Player.main.GetComponent<LiveMixin>().TakeDamage(damageAmount, transform.position, DamageType.Radiation, null);
            }
        }

        private bool IsPlayerCollided(Collider collider)
        {
            GameObject go = collider.gameObject;

            if (go == Player.main.gameObject)
            {
                return true;
            }
            else if (Player.main.inSeamoth && go.GetComponentInParent<SeaMoth>() != null)
            {
                return true;
            }
            else if (Player.main.inExosuit && go.GetComponentInParent<Exosuit>() != null)
            {
                return true;
            }

            return false;
        }

        private bool IsAuroraLeaking()
        {
            if (CrashedShipExploder.main.IsExploded())
            {
                return !LeakingRadiation.main.radiationFixed;
            }
            else
            {
                return false;
            }
        }
    }
}
