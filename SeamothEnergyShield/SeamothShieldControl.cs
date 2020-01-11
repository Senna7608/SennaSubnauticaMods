using UnityEngine;
using Common;

namespace SeamothEnergyShield
{
    public class SeamothShieldControl : MonoBehaviour
    {        
        private SeaMoth thisSeamoth;
        private LiveMixin liveMixin;
        private EnergyMixin energyMixin;
        private MeshRenderer shieldFX;
        private float shieldIntensity;
        private float shieldImpactIntensity;
        private float shieldGoToIntensity;
        private readonly float shieldPowerCost = 10f;
        private readonly float shieldRunningIteration = 5f;

        private FMODAsset shield_on_loop;
        private FMOD_CustomEmitter sfx;        

        private void Start()
        {            
            thisSeamoth = GetComponent<SeaMoth>();
            liveMixin = GetComponent<LiveMixin>();
            energyMixin = GetComponent<EnergyMixin>();

            shield_on_loop = ScriptableObject.CreateInstance<FMODAsset>();
            shield_on_loop.name = "shield_on_loop";
            shield_on_loop.path = "event:/sub/cyclops/shield_on_loop";
            sfx = gameObject.AddComponent<FMOD_CustomEmitter>();
            sfx.asset = shield_on_loop;
            sfx.followParent = true;

            GameObject CyclopsPrefab = GameHelper.GetRootGameObject("Cyclops", "Cyclops-MainPrefab");

            SubRoot subRoot = CyclopsPrefab.GetComponent<SubRoot>();

            shieldFX = Instantiate(subRoot.shieldFX, transform);
            
            shieldFX.gameObject.SetActive(false);

            Utils.ZeroTransform(shieldFX.transform);

            shieldFX.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            thisSeamoth.onToggle += OnToggle;
            thisSeamoth.modules.onAddItem += OnAddItem;
            thisSeamoth.modules.onRemoveItem += OnRemoveItem;                       
        }        

        public void StartSeamothShielded()
        {
            liveMixin.shielded = true;
            shieldFX.gameObject.SetActive(true);
            shieldGoToIntensity = 1f;

            LavaLarva[] componentsInChildren = gameObject.GetComponentsInChildren<LavaLarva>();

            foreach (LavaLarva lavaLarva in componentsInChildren)
            {
                lavaLarva.GetComponent<LiveMixin>().TakeDamage(1f, default(Vector3), DamageType.Electrical, null);
            }
        }

        public void EndSeamothShielded()
        {
            liveMixin.shielded = false;
            shieldGoToIntensity = 0f;
        }

        public void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == SeamothShieldPrefab.TechTypeID)
            {
                enabled = false;                
            }
        }

        public void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == SeamothShieldPrefab.TechTypeID)
            {               
               enabled = true;
            }
        }

        public void OnDestroy()
        {
            thisSeamoth.onToggle -= OnToggle;
            thisSeamoth.modules.onAddItem -= OnAddItem;
            thisSeamoth.modules.onRemoveItem -= OnRemoveItem;                     
            Destroy(this);
        }       

        private void OnToggle(int slotID, bool state)
        {
            if (thisSeamoth.GetSlotBinding(slotID) == SeamothShieldPrefab.TechTypeID)
            {
                if (state)
                {
                    StartShield();
                }
                else
                {
                    StopShield();
                }
            }
        }        

        private void Update()
        {            
            if (shieldFX != null && shieldFX.gameObject.activeSelf)
            {
                shieldImpactIntensity = Mathf.MoveTowards(shieldImpactIntensity, 0f, Time.deltaTime / 4f);
                shieldIntensity = Mathf.MoveTowards(shieldIntensity, shieldGoToIntensity, Time.deltaTime / 2f);
                shieldFX.material.SetFloat(ShaderPropertyID._Intensity, shieldIntensity);
                shieldFX.material.SetFloat(ShaderPropertyID._ImpactIntensity, shieldImpactIntensity);

                if (Mathf.Approximately(shieldIntensity, 0f) && shieldGoToIntensity == 0f)
                {
                    shieldFX.gameObject.SetActive(false);
                }
            }            
        }

        private void ShieldIteration()
        {            
            if (!energyMixin.ConsumeEnergy(shieldPowerCost))
            {
                StopShield();
            }
        }

        private void StartShield()
        {            
            sfx.Play();

            if (GameModeUtils.RequiresPower())
            {
                InvokeRepeating("ShieldIteration", 0f, shieldRunningIteration);
            }

            StartSeamothShielded();
        }

        private void StopShield()
        {           
            sfx.Stop();
            CancelInvoke("ShieldIteration");
            EndSeamothShielded();
        }
    }
}
