using Common;
using UnityEngine;
using SeamothArms.ArmPrefabs;
using System.Collections;
using Common.Helpers;

namespace SeamothArms.ArmControls
{
    public class SeamothTorpedoArmControl : MonoBehaviour, ISeamothArm
    {
        public SeaMoth ThisSeamoth
        {
            get
            {
                return GetComponentInParent<SeaMoth>();
            }
        }
        
        private GenericHandTarget handTarget;
        public Animator animator;

        private const float cooldownTime = 5f;
        private const float cooldownInterval = 1f;
        
        public Transform siloFirst;        
        public Transform siloSecond;       
        public GameObject visualTorpedoFirst;        
        public GameObject visualTorpedoSecond;        
        public GameObject visualTorpedoReload;        
        public FMODAsset fireSound;
        public FMODAsset torpedoDisarmed;

        public ItemsContainer container;
        private float timeFirstShot = float.NegativeInfinity;
        private float timeSecondShot = float.NegativeInfinity;

        public ObjectHelper objectHelper { get; private set; }

        private void Awake()
        {     
            objectHelper = Main.graphics.objectHelper;

            animator = GetComponent<Animator>();

            siloFirst = objectHelper.FindDeepChild(gameObject, "TorpedoSiloFirst").transform;
            siloSecond = objectHelper.FindDeepChild(gameObject, "TorpedoSiloSecond").transform;

            visualTorpedoFirst = objectHelper.FindDeepChild(gameObject, "TorpedoFirst");
            visualTorpedoSecond = objectHelper.FindDeepChild(gameObject, "TorpedoSecond");
            visualTorpedoReload = objectHelper.FindDeepChild(gameObject, "TorpedoReload");

            handTarget = gameObject.GetComponentInChildren<GenericHandTarget>(true);
            handTarget.onHandHover.AddListener(OnHoverTorpedoStorage);
            handTarget.onHandClick.AddListener(OnOpenTorpedoStorage);

            fireSound = ScriptableObject.CreateInstance<FMODAsset>();
            fireSound.path = "event:/sub/seamoth/torpedo_fire";
            fireSound.name = "torpedo_fire";

            torpedoDisarmed = ScriptableObject.CreateInstance<FMODAsset>();
            torpedoDisarmed.path = "event:/sub/seamoth/torpedo_disarmed";
            torpedoDisarmed.name = "torpedo_disarmed";
        }        

        GameObject ISeamothArm.GetGameObject()
        {            
            return gameObject;
        }

        private IEnumerator GetItemsContainer(string slotName)
        {
            SNLogger.Log($"[SeamothArms] GetItemsContainer coroutine started for this Seamoth: {ThisSeamoth.GetInstanceID()}");

            while (container == null)
            {                                   
                container = ThisSeamoth.GetStorageInSlot(ThisSeamoth.GetSlotIndex(slotName), SeamothTorpedoArmPrefab.TechTypeID);                

                SNLogger.Log($"[SeamothArms] ItemsContainer is not ready for this Seamoth: {ThisSeamoth.GetInstanceID()}");
                yield return null;
            }

            SNLogger.Log($"[SeamothArms] ItemsContainer is ready for this Seamoth: {ThisSeamoth.GetInstanceID()}");
            SNLogger.Log($"[SeamothArms] GetItemsContainer coroutine stopped for this Seamoth: {ThisSeamoth.GetInstanceID()}");

            if (container != null)
            {
                container.SetAllowedTechTypes(GetTorpedoTypes());
                container.onAddItem += OnAddItem;
                container.onRemoveItem += OnRemoveItem;
                UpdateVisuals();
            }

            yield break;
        }

        void ISeamothArm.SetSide(Arm arm)
        {
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }

            if (arm == Arm.Right)
            {
                transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
                StartCoroutine(GetItemsContainer("SeamothArmRight"));

            }
            else
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                StartCoroutine(GetItemsContainer("SeamothArmLeft"));
            }            
        }

       
        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, true);
        }

        
        bool ISeamothArm.OnUseHeld(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, false);
        }

        
        bool ISeamothArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return true;
        }

        
        bool ISeamothArm.OnAltDown()
        {
            return false;
        }
                
        void ISeamothArm.Update(ref Quaternion aimDirection)
        {
        }

        
        void ISeamothArm.Reset()
        {
            animator.SetBool("use_tool", false);
        }

        
        private bool TryShoot(out float cooldownDuration, bool verbose)
        {            
            TorpedoType[] torpedoTypes = ThisSeamoth.torpedoTypes;
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

        
        private void OnAddItem(InventoryItem item)
        {
            UpdateVisuals();
        }

        
        private void OnRemoveItem(InventoryItem item)
        {
            UpdateVisuals();
        }

        
        private void UpdateVisuals()
        {            
            int num = 0;
            TorpedoType[] torpedoTypes = ThisSeamoth.torpedoTypes;

            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                num += container.GetCount(torpedoTypes[i].techType);
            }

            visualTorpedoReload.SetActive(num >= 3);
            visualTorpedoSecond.SetActive(num >= 2);
            visualTorpedoFirst.SetActive(num >= 1);
        }

        
        public void OnHoverTorpedoStorage(HandTargetEventData eventData)
        {            
            if (container == null)
            {
                return;
            }
            HandReticle.main.SetInteractText("SeamothTorpedoStorage");
            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
        }

        
        public void OnOpenTorpedoStorage(HandTargetEventData eventData)
        {
            OpenTorpedoStorageExternal(eventData.transform);
        }

        
        public void OpenTorpedoStorageExternal(Transform useTransform)
        {
            if (container == null)
            {
                return;
            }

            Inventory.main.SetUsedStorage(container, false);
            Player.main.GetPDA().Open(PDATab.Inventory, useTransform, null, -1f);
        }

        
        private void OnDestroy()
        {
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }

            handTarget.onHandHover.RemoveListener(OnHoverTorpedoStorage);
            handTarget.onHandClick.RemoveListener(OnOpenTorpedoStorage);
        }

        bool ISeamothArm.HasClaw()
        {
            return false;
        }

        bool ISeamothArm.HasDrill()
        {
            return false;
        }

        void ISeamothArm.SetRotation(Arm arm, bool isDocked)
        {
            if (ThisSeamoth == null)
            {
                return;
            }

            if (isDocked)
            {
                SubRoot subRoot = ThisSeamoth.GetComponentInParent<SubRoot>();

                if (subRoot.isCyclops)
                {
                    if (arm == Arm.Right)
                    {
                        transform.localRotation = Quaternion.Euler(32, 336, 310);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(32, 24, 50);
                    }
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(346, 0, 0);
                }
            }
            else
            {
                transform.localRotation = Quaternion.Euler(346, 0, 0);                
            }
        }

        float ISeamothArm.GetEnergyCost()
        {
            return 0;
        }

        private TechType[] GetTorpedoTypes()
        {
            TechType[] techTypes = new TechType[ThisSeamoth.torpedoTypes.Length];

            for (int i = 0; i < ThisSeamoth.torpedoTypes.Length; i++)
            {
                techTypes[i] = ThisSeamoth.torpedoTypes[i].techType;
            }

            return techTypes;
        }        
    }
}
