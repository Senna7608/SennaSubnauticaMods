using UnityEngine;
using UnityEngine.Rendering;

namespace PlasmaCannonArm
{
    public class PlasmaBullet : Bullet
    {
        public float lifeTime = 2f;        
        public float speed = 50f;        
        private readonly float plasmaDamage = 100f;

        private TrailRenderer trailR;        

        protected override bool speedDependsOnEnergy
        {
            get
            {
                return false;
            }
        }

        protected override float shellRadius
        {
            get
            {
                return base.shellRadius;
            }
        }        

        protected override void Awake()
        {
            base.Awake();

            hitLayers = ~LayerMask.GetMask("Terrain");

            GameObject bulletCore = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            bulletCore.transform.SetParent(transform, false);

            Utils.ZeroTransform(bulletCore.transform);

            MeshRenderer mr = bulletCore.GetComponent<MeshRenderer>();

            mr.material = Instantiate(Main.plasma_Material);

            mr.material.color = Color.green;

            bulletCore.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            
            bulletCore.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            trailR = gameObject.AddComponent<TrailRenderer>();
           
            trailR.shadowCastingMode = ShadowCastingMode.On;
            
            trailR.material = Instantiate(Main.plasma_Material);
            trailR.alignment = LineAlignment.View;
            trailR.material.color = Color.green;
            trailR.startWidth = 0.18f;
            trailR.endWidth = 0f;            
            trailR.time = 0.3f;            
        }        

        protected override void Update()
        {
            base.Update();            
        }

        public override void Shoot(Vector3 position, Quaternion rotation, float speed, float lifeTime)
        {            
            base.Shoot(position, rotation, this.speed + speed, this.lifeTime);           
        }

        protected override void OnHit(RaycastHit hitInfo)
        {            
            AddDamageToTarget(hitInfo.transform.gameObject);
            AddForceToTarget(gameObject, hitInfo.transform.gameObject);
            Destroy(go);
        }

        protected override void OnEnergyDepleted()
        {
            Destroy(go);
        }

        private void AddDamageToTarget(GameObject targetObject)
        {
            Vector3 position = targetObject.transform.position;

            if (targetObject != null)
            {
                LiveMixin liveMixin = targetObject.GetComponent<LiveMixin>();

                if (!liveMixin)
                {
                    liveMixin = Utils.FindEnabledAncestorWithComponent<LiveMixin>(targetObject);
                }
                if (liveMixin)
                {
                    if (liveMixin.IsAlive())
                    {
                        liveMixin.TakeDamage(plasmaDamage, position, DamageType.Normal, null);
                    }
                }
                else
                {
                    if (targetObject.GetComponent<BreakableResource>() != null)
                    {
                        targetObject.SendMessage("BreakIntoResources", null, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        private void AddForceToTarget(GameObject sourceObject, GameObject targetObject)
        {
            Rigidbody rb = targetObject.GetComponent<Rigidbody>();

            Vector3 forwardForce = sourceObject.transform.forward;            

            if (rb != null)
            {
                rb.AddForce(forwardForce * 20f, ForceMode.Impulse);
            }
            else
            {
                rb = targetObject.GetComponentInChildren<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(forwardForce * 20f, ForceMode.Impulse);
                }
            }
        }
    }
}
