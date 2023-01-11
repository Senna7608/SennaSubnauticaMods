using UnityEngine;

#pragma warning disable CS1591 //XML documentation

namespace ModdedArmsHelper.API
{
    public class SeamothGrapplingHook : MonoBehaviour
    {
        public Rigidbody rb;
        public Collider collision;
        private Collider attachedCollider;
        public FMODAsset hitSound;
        public VFXController fxControl;
        private FixedJoint fixedJoint;
        private bool staticAttached;
        private const float grappleRetrieveSpeed = 10f;
        
        public bool attached
        {
            get
            {
                return fixedJoint != null || staticAttached;
            }
        }
                
        public bool flying
        {
            get
            {
                return transform.parent == null && !rb.isKinematic;
            }
        }
                
        public bool resting
        {
            get
            {
                return transform.localPosition == Vector3.zero;
            }
        }
        
        
        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            collision = GetComponent<Collider>();
            fxControl = GetComponent<VFXController>();
            fixedJoint = GetComponent<FixedJoint>();

            hitSound = ScriptableObject.CreateInstance<FMODAsset>();
            hitSound.path = "event:/sub/exo/hook_hit";
            hitSound.name = "hook_hit";            
        }
        

        public Rigidbody GetTargetRigidbody(GameObject go)
        {
            GameObject gameObject = UWE.Utils.GetEntityRoot(go);

            if (gameObject == null)
            {
                gameObject = go;
            }
            return gameObject.GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision collisionInfo)
        {
            SeaMoth componentInParent = collisionInfo.gameObject.GetComponentInParent<SeaMoth>();

            SeamothGrapplingHook component = collisionInfo.gameObject.GetComponent<SeamothGrapplingHook>();

            if (staticAttached || (fixedJoint && fixedJoint.connectedBody) || componentInParent != null || component != null || IsPropulsionCannonAmmo(collisionInfo.gameObject))
            {
                return;
            }

            attachedCollider = collisionInfo.collider;
            
            Rigidbody targetRigidbody = GetTargetRigidbody(collisionInfo.gameObject);

            rb.velocity = Vector3.zero;

            if (targetRigidbody != null && JointHelper.ConnectFixed(gameObject, targetRigidbody))
            {
                staticAttached = false;
            }
            else
            {
                staticAttached = true;

                MainGameController instance = MainGameController.Instance;

                if (instance != null)
                {
                    instance.DeregisterHighFixedTimestepBehavior(this);
                }
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(this.rb, true, false);
            }

            Utils.PlayFMODAsset(hitSound, transform, 5f);

            Vector3 vector = default(Vector3);

            int num = 0;

            for (int i = 0; i < collisionInfo.contacts.Length; i++)
            {
                ContactPoint contactPoint = collisionInfo.contacts[i];
                if (num == 0)
                {
                    vector = contactPoint.normal;
                }
                else
                {
                    vector += contactPoint.normal;
                }
                num++;
            }
            if (num > 0)
            {
                vector /= num;
                Vector3 eulerAngles = Quaternion.LookRotation(transform.forward, vector).eulerAngles;
                eulerAngles.z -= 90f;
                transform.eulerAngles = eulerAngles;
            }
            VFXSurface component2 = collisionInfo.gameObject.GetComponent<VFXSurface>();
            VFXSurfaceTypeManager.main.Play(component2, VFXEventTypes.impact, transform.position, transform.rotation, null);
        }
                
        private void OnJointConnected(FixedJoint joint)
        {
            fixedJoint = joint;
        }

        private bool IsPropulsionCannonAmmo(GameObject gameObject)
        {
            GameObject gameObject2 = UWE.Utils.GetEntityRoot(gameObject);
            if (gameObject2 == null)
            {
                gameObject2 = gameObject;
            }
            return gameObject2.GetComponent<PropulseCannonAmmoHandler>() != null;
        }

        private void FixedUpdate()
        {
            bool flag = false;
            if (fixedJoint || staticAttached)
            {
                if (fixedJoint && !fixedJoint.connectedBody)
                {
                    flag = true;
                }
                else if (!attachedCollider || !attachedCollider.gameObject.activeInHierarchy)
                {
                    flag = true;
                }
                else if (IsPropulsionCannonAmmo(attachedCollider.gameObject))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                Debug.Log("disconnect, connected body lost");
                if (fixedJoint)
                {
                    JointHelper.Disconnect(fixedJoint, true);
                    fixedJoint = null;
                }
                staticAttached = false;
            }
        }
        
        private void Update()
        {
            if (transform.parent != null && !resting)
            {
                transform.localPosition = UWE.Utils.LerpVector(transform.localPosition, Vector3.zero, Time.deltaTime * 10f);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 20f);
            }
        }

        private void OnDisable()
        {
            MainGameController instance = MainGameController.Instance;

            if (instance == null)
            {
                return;
            }

            instance.DeregisterHighFixedTimestepBehavior(this);
        }

        public void Release()
        {
            if (fixedJoint)
            {
                JointHelper.Disconnect(fixedJoint, true);
                fixedJoint = null;
            }

            fxControl.StopAndDestroy(0, 1.5f);

            staticAttached = false;
        }

        
        public void SetFlying(bool isFlying)
        {
            if (isFlying)
            {
                MainGameController instance = MainGameController.Instance;

                if (instance != null)
                {
                    instance.RegisterHighFixedTimestepBehavior(this);
                }
            }
            else
            {
                MainGameController instance2 = MainGameController.Instance;

                if (instance2 != null)
                {
                    instance2.DeregisterHighFixedTimestepBehavior(this);
                }
            }

            UWE.Utils.SetIsKinematicAndUpdateInterpolation(rb, !isFlying, false);

            collision.enabled = isFlying;
            fxControl.Play(0);
        }
    }
}
