//#define DEBUG_EXOSUIT_OVERDRIVE

using UnityEngine;

namespace CheatManager
{
    public class ExosuitOverDrive : MonoBehaviour
    {
        public static ExosuitOverDrive Main { get; private set; }

        private Exosuit exosuit;

        Equipment exosuitEquipment;

        private TechType SpeedModule;
        private const float MaxSpeed = 50f;
        private const float SpeedModuleBoost = 4.025f;
        private int SpeedModuleCount = 0;
        private int prevSpeedModuleCount = 0;
        private float prev_multiplier;
        private float Def_backwardForce;
        private float Def_forwardForce;
        private float Def_sidewardForce;
        private float Def_verticalForce;

#if DEBUG_EXOSUIT_OVERDRIVE
        private string exosuitID;
#endif
        public void Awake()
        {
            Main = this; 
        }

        public void Start()
        {
            exosuit = gameObject.GetComponent<Exosuit>();

            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);

#if DEBUG_EXOSUIT_OVERDRIVE
            exosuitID = gameObject.GetId();
            Logger.Log($"ExosuitOverDrive().Start(): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif

            Def_forwardForce = exosuit.forwardForce;
            Def_backwardForce = exosuit.backwardForce;
            Def_sidewardForce = exosuit.sidewardForce;
            Def_verticalForce = exosuit.verticalForce;

            prev_multiplier = 1;

            exosuitEquipment = exosuit.modules;
            exosuitEquipment.onAddItem += SpeedModuleAddListener;
            exosuitEquipment.onRemoveItem += SpeedModuleRemoveListener;

            CheatManager.exosuitSpeedMultiplier = 1;
        }

        public void OnDestroy()
        {
            
            CheatManager.exosuitSpeedMultiplier = 1;

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive().OnDestroy(): {exosuitID}\nGameObject Destroyed!");
#endif
            exosuitEquipment.onAddItem -= SpeedModuleAddListener;
            exosuitEquipment.onRemoveItem -= SpeedModuleRemoveListener;

            Destroy(this);
        }


        private void SpeedModuleAddListener(InventoryItem invItem)
        {
            SpeedModuleCount = exosuit.modules.GetCount(SpeedModule);

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive().Event(Add): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
            
        }

        private void SpeedModuleRemoveListener(InventoryItem invItem)
        {
            SpeedModuleCount = exosuit.modules.GetCount(SpeedModule);

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive().Event(Remove): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
            
        }

        public void Update()
        {
            if (prev_multiplier != CheatManager.exosuitSpeedMultiplier || prevSpeedModuleCount != SpeedModuleCount)
            {
                float boost = 0;
                float multiplier = CheatManager.exosuitSpeedMultiplier;

                if (SpeedModuleCount > 0)
                {
                    boost = SpeedModuleCount * SpeedModuleBoost;
                }

                if (CheatManager.exosuitSpeedMultiplier == 1)
                {

                    exosuit.forwardForce = Def_forwardForce + boost;
                    exosuit.backwardForce = Def_backwardForce;
                    exosuit.sidewardForce = Def_sidewardForce;
                    exosuit.verticalForce = Def_verticalForce;
                    prev_multiplier = multiplier;
                }
                else
                {
                    exosuit.forwardForce = (Def_forwardForce + boost) + (multiplier * ((MaxSpeed - (Def_forwardForce + boost)) / 5));
                    exosuit.backwardForce = (Def_backwardForce + boost) + (multiplier * ((MaxSpeed - (Def_backwardForce + boost)) / 5));
                    exosuit.sidewardForce = (Def_sidewardForce + boost) + (multiplier * ((MaxSpeed - (Def_sidewardForce + boost)) / 5));
                    exosuit.verticalForce = (Def_verticalForce + boost) + (multiplier * ((MaxSpeed - (Def_verticalForce + boost)) / 5));
                    prev_multiplier = multiplier;
                }

                prevSpeedModuleCount = SpeedModuleCount;

#if DEBUG_EXOSUIT_OVERDRIVE
                Logger.Log($"[CheatManager]\nExosuitOverDrive().Update() {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
            }
            
        }      
    }
}
