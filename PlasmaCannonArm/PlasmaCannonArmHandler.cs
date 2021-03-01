using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.ArmHandlers;

namespace PlasmaCannonArm
{
    public class PlasmaCannonArmHandler : ArmHandler, IExosuitArm
    {
        public GameObject cannon_tube_right, cannon_tube_left;
        public AudioSource audioSource;       
        public GameObject glowLeft, glowRight;
                
        private float timeFirstShot = float.NegativeInfinity;
        private float timeSecondShot = float.NegativeInfinity;
        private const float coolDown = 0.5f;
        
        private const float energyCosumption = 0.5f;       
        
        public override void Awake()
        {
            GameObject plasmaCannon = ArmServices.main.objectHelper.FindDeepChild(gameObject, "plasmaCannon");
            GameObject PlasmaArm = plasmaCannon.transform.Find("PlasmaArm").gameObject;

            audioSource = PlasmaArm.GetComponentInChildren<AudioSource>();
            audioSource.volume = 0.08f;

            cannon_tube_left = plasmaCannon.transform.Find("PlasmaTubeLeft").gameObject;
            cannon_tube_right = plasmaCannon.transform.Find("PlasmaTubeRight").gameObject;            
            glowLeft = plasmaCannon.transform.Find("PlasmaArm/GlowTubeLeft/glowLeft").gameObject;
            glowRight = plasmaCannon.transform.Find("PlasmaArm/GlowTubeRight/glowRight").gameObject;                    
        }

        public override void Start()
        {
        }

        GameObject IExosuitArm.GetGameObject()
        {           
            return gameObject;            
        }
        
        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);                                
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);                              
            }
        }
       
        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, true);
        }        
        
        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, false);
        }
        
        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            Animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return true;
        }        

        void IExosuitArm.Update(ref Quaternion aimDirection)
        {            
        }        

        void IExosuitArm.Reset()
        {
            Animator.SetBool("use_tool", false);
        }

        public bool OnAltDown()
        {
            return false;
        }
   

        private GameObject CreatePlasmaBullet(Transform siloTransform)
        {
            GameObject pBullet = new GameObject("PlasmaBullet");

            pBullet.transform.position = siloTransform.position;

            pBullet.AddComponent<PlasmaBullet>();

            return pBullet;
        }
                
        public bool PlasmaShoot(GameObject bullet, Transform siloTransform)
        {
            PlasmaBullet plasmaBullet = bullet.GetComponent<PlasmaBullet>();            

            Transform aimingTransform = Player.main.camRoot.GetAimingTransform();            

            Rigidbody rb = gameObject.GetComponentInParent<Rigidbody>();           

            Vector3 rhs = (!(rb != null)) ? Vector3.zero : rb.velocity;

            float speed = Vector3.Dot(aimingTransform.forward, rhs);

            plasmaBullet.Shoot(siloTransform.position, aimingTransform.rotation, speed, -1f);
            
            audioSource.Play();

            Animator.SetBool("use_tool", true);

            Exosuit.GetComponent<EnergyInterface>().ConsumeEnergy(energyCosumption);

            return true;
        }  

        private bool TryShoot(out float cooldownDuration, bool verbose)
        {            
            float num = Mathf.Clamp(Time.time - timeFirstShot, 0f, coolDown);
            float num2 = Mathf.Clamp(Time.time - timeSecondShot, 0f, coolDown);
            float b = coolDown - num;
            float b2 = coolDown - num2;
            float num3 = Mathf.Min(num, num2);
            if (num3 < coolDown)
            {
                cooldownDuration = 0f;
                return false;
            }
            if (num >= coolDown)
            {
                GameObject bullet1 = CreatePlasmaBullet(cannon_tube_left.transform);

                if (PlasmaShoot(bullet1, cannon_tube_left.transform))
                {
                    timeFirstShot = Time.time;
                    cooldownDuration = Mathf.Max(coolDown, b2);
                    return true;
                }
            }
            else
            {
                if (num2 < coolDown)
                {
                    cooldownDuration = 0f;
                    return false;
                }

                GameObject bullet2 = CreatePlasmaBullet(cannon_tube_right.transform);

                if (PlasmaShoot(bullet2, cannon_tube_right.transform))
                {
                    timeSecondShot = Time.time;
                    cooldownDuration = Mathf.Max(coolDown, b);
                    return true;
                }
            }

            Animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return false;
        }

        public void ResetColors()
        {
            MeshRenderer glowLeftRenderer = glowLeft.GetComponent<MeshRenderer>();
            MeshRenderer glowRightRenderer = glowRight.GetComponent<MeshRenderer>();

            glowLeftRenderer.materials[1].color = new Color(0.330f, 0.901f, 0.431f, 1f);
            glowRightRenderer.materials[1].color = new Color(0.330f, 0.901f, 0.431f, 1f);
        }
    }
}

