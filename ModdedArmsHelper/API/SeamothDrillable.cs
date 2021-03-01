using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModdedArmsHelper.API
{
    public class SeamothDrillable : MonoBehaviour, IManagedUpdateBehaviour, IManagedBehaviour
    {
        private Drillable drillable;

        public event Drillable.OnDrilled onDrilled;

        public int managedUpdateIndex { get; set; }

        private MeshRenderer[] renderers;
        private const float drillDamage = 5f;
        private const float maxHealth = 200f;
        private float timeLastDrilled;
        private List<GameObject> lootPinataObjects = new List<GameObject>();
        private SeaMoth drillingMoth;

        private void OnEnable()
        {
            BehaviourUpdateUtils.RegisterForUpdate(this);
        }
        
        private void OnDisable()
        {
            BehaviourUpdateUtils.Deregister(this);
        }
        
        private void OnDestroy()
        {
            BehaviourUpdateUtils.Deregister(this);
        }
                
        public void ManagedFixedUpdate()
        {
        }
                
        public void ManagedLateUpdate()
        {
        }
        
        private void Awake()
        {
            drillable = GetComponent<Drillable>();
            renderers = GetComponentsInChildren<MeshRenderer>();            
        }
        
        public void HoverDrillable()
        {
            SeamothArmManager control = Player.main.GetComponentInParent<SeamothArmManager>();

            if (control && control.HasDrill())
            {                
                HandReticle.main.SetInteractText(Language.main.GetFormat("DrillResource", Language.main.Get(drillable.primaryTooltip)), drillable.secondaryTooltip, false, true, HandReticle.Hand.Left);
                HandReticle.main.SetIcon(HandReticle.IconType.Drill, 1f);
            }
            else
            {
                HandReticle.main.SetInteractText(drillable.primaryTooltip, "NeedExoToMine");
            }
        }
               
        public void OnDrill(Vector3 position, SeaMoth seamoth, out GameObject hitObject)
        {
            float num = 0f;

            for (int i = 0; i < drillable.health.Length; i++)
            {
                num += drillable.health[i];
            }

            drillingMoth = seamoth;

            Vector3 zero = Vector3.zero;

            int num2 = FindClosestMesh(position, out zero);

            hitObject = renderers[num2].gameObject;

            timeLastDrilled = Time.time;

            if (num > 0f)
            {
                float num3 = drillable.health[num2];

                drillable.health[num2] = Mathf.Max(0f, drillable.health[num2] - 5f);

                num -= num3 - drillable.health[num2];

                if (num3 > 0f && drillable.health[num2] <= 0f)
                {
                    renderers[num2].gameObject.SetActive(false);

                    SpawnFX(drillable.breakFX, zero);

                    if (Random.value < drillable.kChanceToSpawnResources)
                    {
                        SpawnLoot(zero);
                    }
                }

                if (num <= 0f)
                {
                    SpawnFX(drillable.breakAllFX, zero);

                    onDrilled?.Invoke(drillable);

                    if (drillable.deleteWhenDrilled)
                    {
                        float time = (!drillable.lootPinataOnSpawn) ? 0f : 6f;
                        drillable.Invoke("DestroySelf", time);
                    }
                }
            }
        }
                        
        private void ClipWithTerrain(ref Vector3 position)
        {
            Vector3 origin = position;

            origin.y = transform.position.y + 5f;

            Ray ray = new Ray(origin, Vector3.down);

            int num = UWE.Utils.RaycastIntoSharedBuffer(ray, 10f, -5, QueryTriggerInteraction.UseGlobal);

            for (int i = 0; i < num; i++)
            {
                RaycastHit raycastHit = UWE.Utils.sharedHitBuffer[i];

                if (raycastHit.collider.gameObject.GetComponentInParent<VoxelandChunk>() != null)
                {
                    position.y = Mathf.Max(position.y, raycastHit.point.y + 0.3f);
                    break;
                }
            }
        }       

        private void SpawnLoot(Vector3 position)
        {
            if (drillable.resources.Length > 0)
            {
                int num = Random.Range(drillable.minResourcesToSpawn, drillable.maxResourcesToSpawn);

                for (int i = 0; i < num; i++)
                {
                    GameObject _resource = ChooseRandomResource();

                    if (_resource)
                    {
                        GameObject resource = Instantiate(_resource);
                        Vector3 position2 = position;
                        float num2 = 1f;
                        position2.x += Random.Range(-num2, num2);
                        position2.z += Random.Range(-num2, num2);
                        position2.y += Random.Range(-num2, num2);
                        ClipWithTerrain(ref position2);
                        resource.transform.position = position2;
                        Vector3 vector = Random.onUnitSphere;
                        vector.y = 0f;
                        vector = Vector3.Normalize(vector);
                        vector.y = 1f;
                        resource.GetComponent<Rigidbody>().isKinematic = false;
                        resource.GetComponent<Rigidbody>().AddForce(vector);
                        resource.GetComponent<Rigidbody>().AddTorque(Vector3.right * Random.Range(3f, 6f));

                        if (drillable.lootPinataOnSpawn)
                        {
                            StartCoroutine(AddResourceToPinata(resource));
                        }
                    }
                }
            }
        }
        
        private IEnumerator AddResourceToPinata(GameObject resource)
        {
            yield return new WaitForSeconds(1.5f);
            lootPinataObjects.Add(resource);
            yield break;
        }
        
        private int FindClosestMesh(Vector3 position, out Vector3 center)
        {
            int result = 0;
            float num = float.PositiveInfinity;
            center = Vector3.zero;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].gameObject.activeInHierarchy)
                {
                    Bounds encapsulatedAABB = UWE.Utils.GetEncapsulatedAABB(renderers[i].gameObject, -1);
                    float sqrMagnitude = (encapsulatedAABB.center - position).sqrMagnitude;
                    if (sqrMagnitude < num)
                    {
                        num = sqrMagnitude;
                        result = i;
                        center = encapsulatedAABB.center;
                        if (sqrMagnitude <= 0.5f)
                        {
                            break;
                        }
                    }
                }
            }
            return result;
        }
        
        private GameObject ChooseRandomResource()
        {
            GameObject result = null;

            for (int i = 0; i < drillable.resources.Length; i++)
            {
                Drillable.ResourceType resourceType = drillable.resources[i];

                if (resourceType.chance >= 1f)
                {
                    result = CraftData.GetPrefabForTechType(resourceType.techType, true);
                    break;
                }

                PlayerEntropy component = Player.main.gameObject.GetComponent<PlayerEntropy>();                

                if (component.CheckChance(resourceType.techType, resourceType.chance))
                {
                    result = CraftData.GetPrefabForTechType(resourceType.techType, true);
                    break;
                }
            }
            return result;
        }
        
        private void SpawnFX(GameObject fx, Vector3 position)
        {
            GameObject gameObject = Instantiate(fx);
            gameObject.transform.position = position;
        }
        
        public void ManagedUpdate()
        {
            if (timeLastDrilled + 0.5f > Time.time)
            {
                drillable.modelRoot.transform.position = transform.position + new Vector3(Mathf.Sin(Time.time * 60f), Mathf.Cos(Time.time * 58f + 0.5f), Mathf.Cos(Time.time * 64f + 2f)) * 0.011f;
            }
            if (lootPinataObjects.Count > 0 && drillingMoth)
            {
                List<GameObject> list = new List<GameObject>();

                foreach (GameObject gameObject in lootPinataObjects)
                {
                    if (gameObject == null)
                    {
                        list.Add(gameObject);
                    }
                    else
                    {
                        Vector3 b = drillingMoth.transform.position + new Vector3(0f, 0.8f, 0f);

                        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, b, Time.deltaTime * 5f);

                        float num = Vector3.Distance(gameObject.transform.position, b);

                        if (num < 3f)
                        {
                            Pickupable pickupable = gameObject.GetComponentInChildren<Pickupable>();

                            if (pickupable)
                            {
                                SeamothArmManager control = drillingMoth.GetComponent<SeamothArmManager>();

                                if (!control.HasRoomForItem(pickupable))
                                {
                                    if (Player.main.GetVehicle() == drillingMoth)
                                    {
                                        ErrorMessage.AddMessage(Language.main.Get("ContainerCantFit"));
                                    }
                                }
                                else
                                {
                                    string arg = Language.main.Get(pickupable.GetTechName());
                                    ErrorMessage.AddMessage(Language.main.GetFormat("VehicleAddedToStorage", arg));
                                    uGUI_IconNotifier.main.Play(pickupable.GetTechType(), uGUI_IconNotifier.AnimationType.From, null);
                                    pickupable = pickupable.Initialize();
                                    InventoryItem item = new InventoryItem(pickupable);
                                    control.GetRoomForItem(pickupable).UnsafeAdd(item);
                                    pickupable.PlayPickupSound();
                                }
                                list.Add(gameObject);
                            }
                        }
                    }
                }

                if (list.Count > 0)
                {
                    foreach (GameObject item2 in list)
                    {
                        lootPinataObjects.Remove(item2);
                    }
                }
            }
        }

        public string GetProfileTag()
        {
            return "Drillable";
        }
    }

}

