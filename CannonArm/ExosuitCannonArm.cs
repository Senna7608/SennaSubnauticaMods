using UnityEngine;
using Common.DebugHelper;

namespace CannonArm
{
    public class ExosuitCannonArm : MonoBehaviour, IExosuitArm
    {
        private const float cooldownTime = 5f;
        private const float cooldownInterval = 1f;

        public Transform siloFirst;
        public Transform siloSecond;
        public GameObject visualTorpedoFirst;
        public GameObject visualTorpedoSecond;
        public GameObject visualTorpedoReload;
        public Animator animator;
        public FMODAsset fireSound;
        public FMODAsset torpedoDisarmed;
        public ItemsContainer container;

        private float timeFirstShot = float.NegativeInfinity;
        private float timeSecondShot = float.NegativeInfinity;

        private bool isPatched = false;
        
        private void Start()
        {    
            /*
            Debug.Log("[ExosuitCannonArm] Start() called");
            if (container == null)
            {
                container = new ItemsContainer(4,4, gameObject.transform, "ExosuitCannonContainer", torpedoDisarmed);
            }
            */
            if (isPatched)
                return;

            //DebugHelper.DebugComponents(gameObject);
            isPatched = true;
            /*
            try
            {
                
                var obj = gameObject.GetComponentInParent<Exosuit>();
                foreach (Exosuit.ExosuitArmPrefab prefabItem in obj.armPrefabs)
                {
                    if (prefabItem.techType == TechType.ExosuitTorpedoArmModule)
                    {
                        var torpedoArmPrefab = prefabItem.prefab.GetComponent<ExosuitTorpedoArm>();
                        //DebugHelper.DebugComponents(torpedoArmPrefab.gameObject);
                        animator = Instantiate(torpedoArmPrefab.animator, gameObject.transform.parent);
                        animator.name = "ExosuitCannonArm";

                        //siloFirst = Instantiate(torpedoArmPrefab.siloFirst, gameObject.transform);
                        //siloSecond = Instantiate(torpedoArmPrefab.siloSecond, gameObject.transform);
                        visualTorpedoFirst = Instantiate(torpedoArmPrefab.visualTorpedoFirst, gameObject.transform.parent);
                        visualTorpedoSecond = Instantiate(torpedoArmPrefab.visualTorpedoSecond, gameObject.transform.parent);
                        visualTorpedoReload = Instantiate(torpedoArmPrefab.visualTorpedoReload, gameObject.transform.parent);
                        fireSound = Instantiate(torpedoArmPrefab.fireSound, gameObject.transform.parent);
                        torpedoDisarmed = Instantiate(torpedoArmPrefab.torpedoDisarmed, gameObject.transform.parent);

                        //Debug.Log($"[ExosuitCannonArm] animator.name: {animator.name}");
                        isPatched = true;
                    }
                }
                Debug.Log($"[ExosuitCannonArm] obj.name: {obj.name}");
            }
            catch
            {
                Debug.Log($"[ExosuitCannonArm] obj: NULL!");
            }*/            
        }
        

        GameObject IExosuitArm.GetGameObject()
        {            
            return gameObject;            
        }

        private void OnAddItem(InventoryItem item)
        {
            Debug.Log("OnAddItem called");
            UpdateVisuals();
        }
        
        private void OnRemoveItem(InventoryItem item)
        {
            Debug.Log("OnRemoveItem called");
            UpdateVisuals();
        }

        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            Debug.Log("SetSide called");

            Exosuit componentInParent = GetComponentInParent<Exosuit>();
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }
            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                container = componentInParent.GetStorageInSlot(componentInParent.GetSlotIndex("ExosuitArmRight"), TechType.ExosuitTorpedoArmModule);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                container = componentInParent.GetStorageInSlot(componentInParent.GetSlotIndex("ExosuitArmLeft"), TechType.ExosuitTorpedoArmModule);
            }
            if (container != null)
            {
                container.onAddItem += OnAddItem;
                container.onRemoveItem += OnRemoveItem;
            }
            UpdateVisuals();
        }

        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            Debug.Log("OnUseDown called");
            return TryShoot(out cooldownDuration, true);
        }

        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            Debug.Log("OnUseHeld called");
            return TryShoot(out cooldownDuration, false);
        }

        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            Debug.Log("OnUseUp called");
            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return true;
        }

        bool IExosuitArm.OnAltDown()
        {
            Debug.Log("OnAltDown called");
            return false;
        }

        void IExosuitArm.Update(ref Quaternion aimDirection)
        {            
        }

        void IExosuitArm.Reset()
        {
            Debug.Log("Reset called");
            animator.SetBool("use_tool", false);
        }

        private bool TryShoot(out float cooldownDuration, bool verbose)
        {
            Exosuit componentInParent = GetComponentInParent<Exosuit>();
            TorpedoType[] torpedoTypes = componentInParent.torpedoTypes;
            TorpedoType torpedoType = null;
            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                if (container.Contains(torpedoTypes[i].techType))
                {
                    torpedoType = torpedoTypes[i];
                    break;
                }
            }
            float num = Mathf.Clamp(Time.time - timeFirstShot, 0f, 5f);
            float num2 = Mathf.Clamp(Time.time - timeSecondShot, 0f, 5f);
            float b = 5f - num;
            float b2 = 5f - num2;
            float num3 = Mathf.Min(num, num2);
            if (num3 < 1f)
            {
                cooldownDuration = 0f;
                return false;
            }
            if (num >= 5f)
            {
                if (Shoot(torpedoType, siloFirst, verbose))
                {
                    timeFirstShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b2);
                    return true;
                }
            }
            else
            {
                if (num2 < 5f)
                {
                    cooldownDuration = 0f;
                    return false;
                }
                if (Shoot(torpedoType, siloSecond, verbose))
                {
                    timeSecondShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b);
                    return true;
                }
            }
            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return false;
        }

        private bool Shoot(TorpedoType torpedoType, Transform siloTransform, bool verbose)
        {
            if (Vehicle.TorpedoShot(container, torpedoType, siloTransform))
            {
                Utils.PlayFMODAsset(fireSound, siloTransform, 20f);
                animator.SetBool("use_tool", true);
                if (container.count == 0)
                {
                    Utils.PlayFMODAsset(torpedoDisarmed, transform, 1f);
                }
                return true;
            }
            if (verbose)
            {
                ErrorMessage.AddError(Language.main.Get("VehicleTorpedoNoAmmo"));
            }
            return false;
        }

        private void UpdateVisuals()
        {
            Debug.Log("UpdateVisuals called");
            
            Exosuit componentInParent = gameObject.GetComponentInParent<Exosuit>();

            int num = 2;
            
            TorpedoType[] torpedoTypes = componentInParent.torpedoTypes;
            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                num += container.GetCount(torpedoTypes[i].techType);
            }
            visualTorpedoReload.SetActive(num >= 3);
            visualTorpedoSecond.SetActive(num >= 2);
            visualTorpedoFirst.SetActive(num >= 1);
            
        }
    }
}