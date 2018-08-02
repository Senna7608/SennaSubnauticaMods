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

        //Default force values from Vehicle class in Assembly-CSharp.dll with dnSpy and Ingame debugged Exosuit force values without any speedhack:

        //private const float Default_forwardForce = 13f;     // 6f
        //private const float Default_backwardForce = 5f;     // 2f
        //private const float Default_sidewaysTorque = 8.5f;  // 25f  what is this?
        //private const float Default_sidewardForce = 11.5f;  // 3f
        //private const float Default_verticalForce = 11f;    // 2f

        //Calculated Exosuit penalties
        //private const float Exosuit_forwardForce_Penalty = -7f;
        //private const float Exosuit_backwardForce_Penalty = -3f;
        //private const float Exosuit_sidewaysTorque_Penalty = 16.5f;
        //private const float Exosuit_sidewardForce_Penalty = -8.5f;
        //private const float Exosuit_verticalForce_Penalty = -9f;

        private float Exosuit_forwardForce;
        private float Exosuit_backwardForce;
        //private float Exosuit_sidewaysTorque;
        private float Exosuit_sidewardForce;
        private float Exosuit_verticalForce;

#if DEBUG_EXOSUIT_OVERDRIVE
        private string exosuitID;
#endif
        public void Awake()
        {
            exosuit = gameObject.GetComponent<Exosuit>();

            if (exosuit == null)
            {
                Destroy(this);
            }
            else
            {
                Main = this;
            }
        }

        public void Start()
        {
            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);

#if DEBUG_EXOSUIT_OVERDRIVE
            exosuitID = gameObject.GetId();
            Logger.Log($"ExosuitOverDrive.Start(): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}\nSidewaysTorque: {exosuit.sidewaysTorque}");
#endif

            Exosuit_forwardForce = exosuit.forwardForce;
            Exosuit_backwardForce = exosuit.backwardForce;
            Exosuit_sidewardForce = exosuit.sidewardForce;
            Exosuit_verticalForce = exosuit.verticalForce;

            prev_multiplier = 1;

            exosuitEquipment = exosuit.modules;
            exosuitEquipment.onAddItem += SpeedModuleAddListener;
            exosuitEquipment.onRemoveItem += SpeedModuleRemoveListener;           
        }

        public void OnDestroy()
        {

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive.OnDestroy(): {exosuitID}\nGameObject Destroyed!");
#endif
            exosuitEquipment.onAddItem -= SpeedModuleAddListener;
            exosuitEquipment.onRemoveItem -= SpeedModuleRemoveListener;

            Destroy(this);
        }


        private void SpeedModuleAddListener(InventoryItem invItem)
        {
            SpeedModuleCount = exosuit.modules.GetCount(SpeedModule);

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive.Event(Add): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
            
        }

        private void SpeedModuleRemoveListener(InventoryItem invItem)
        {
            SpeedModuleCount = exosuit.modules.GetCount(SpeedModule);

#if DEBUG_EXOSUIT_OVERDRIVE
            Logger.Log($"[CheatManager]\nExosuitOverDrive.Event(Remove): {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
            
        }

        public void Update()
        {
            if (Player.main.inExosuit)
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

                        exosuit.forwardForce = Exosuit_forwardForce + boost;
                        exosuit.backwardForce = Exosuit_backwardForce;
                        exosuit.sidewardForce = Exosuit_sidewardForce;
                        exosuit.verticalForce = Exosuit_verticalForce;
                        prev_multiplier = multiplier;
                    }
                    else
                    {
                        float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);
                        exosuit.forwardForce = (Exosuit_forwardForce + boost) + (multiplier * ((overDrive - (Exosuit_forwardForce + boost)) / 5));
                        exosuit.backwardForce = (Exosuit_backwardForce + boost) + (multiplier * ((overDrive - (Exosuit_backwardForce + boost)) / 5));
                        exosuit.sidewardForce = (Exosuit_sidewardForce + boost) + (multiplier * ((overDrive - (Exosuit_sidewardForce + boost)) / 5));
                        exosuit.verticalForce = (Exosuit_verticalForce + boost) + (multiplier * ((overDrive - (Exosuit_verticalForce + boost)) / 5));
                        prev_multiplier = multiplier;
                    }

                    prevSpeedModuleCount = SpeedModuleCount;

#if DEBUG_EXOSUIT_OVERDRIVE
                Logger.Log($"[CheatManager]\nExosuitOverDrive.Update() {exosuitID}\nForwardForce: {exosuit.forwardForce}\nBackWardForce: {exosuit.backwardForce}\nVerticalForce: {exosuit.verticalForce}\nSidewardForce: {exosuit.sidewardForce}");
#endif
                }

            }
        }
    }
}
