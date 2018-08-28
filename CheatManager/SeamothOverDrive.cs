//#define DEBUG_SEAMOTH_OVERDRIVE

using UnityEngine;

namespace CheatManager
{
    public class SeamothOverDrive : MonoBehaviour
    {
        public static SeamothOverDrive Main { get; private set; }        

        private SeaMoth seamoth;
        private Equipment seamothEquipment;

        private TechType SpeedModule;

        private const float MaxSpeed = 50f;
        private const float SpeedModuleBoost = 4.025f;
        private int SpeedModuleCount = 0;
        
        private int prevSpeedModuleCount = 0;
        
        private float prev_multiplier = 1;
        
        private bool prev_seamothCanFly = false;

        //Default force values from Vehicle class in Assembly-CSharp.dll with dnSpy and Ingame debugged Seamoth force values without any speedhack:

        //private const float Default_forwardForce = 13f;         //   11.5f
        //private const float Default_backwardForce = 5f;         //   5f
        //private const float Default_sidewaysTorque = 8.5f;      //   8.5f
        //private const float Default_sidewardForce = 11.5f;      //   11.5
        //private const float Default_verticalForce = 11f;        //   11

        //Calculated Seamoth penalty:
        //private const float Seamoth_forwardForce_Penalty = -1.5f;

        private float Seamoth_forwardForce;
        private float Seamoth_backwardForce;
        //private float Seamoth_sidewaysTorque;
        private float Seamoth_sidewardForce;
        private float Seamoth_verticalForce;

#if DEBUG_SEAMOTH_OVERDRIVE
        private string seamothID;        
#endif
        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();

            if (seamoth == null)
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

#if DEBUG_SEAMOTH_OVERDRIVE
            seamothID = gameObject.GetId();
            Logger.Log($"[CheatManager]\nSeamothOverDrive.Start(): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}\nSidewaysTorque: {seamoth.sidewaysTorque}");
#endif
            Seamoth_forwardForce = seamoth.forwardForce;
            Seamoth_backwardForce = seamoth.backwardForce;
            Seamoth_sidewardForce = seamoth.sidewardForce;
            Seamoth_verticalForce = seamoth.verticalForce;            

            seamothEquipment = seamoth.modules;
            seamothEquipment.onAddItem += SpeedModuleAddListener;
            seamothEquipment.onRemoveItem += SpeedModuleRemoveListener;            
        }
        
        public void OnDestroy()
        {

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive.OnDestroy(): {seamothID}\nGameObject Destroyed!");
#endif
            seamothEquipment.onAddItem -= SpeedModuleAddListener;
            seamothEquipment.onRemoveItem -= SpeedModuleRemoveListener;            
            Destroy(this);
        }


        private void SpeedModuleAddListener(InventoryItem invItem)
        {
           SpeedModuleCount = seamoth.modules.GetCount(SpeedModule);

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive.Event(Add): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
        }

        private void SpeedModuleRemoveListener(InventoryItem invItem)
        {
            SpeedModuleCount = seamoth.modules.GetCount(SpeedModule);

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive.Event(Remove): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
        }
                
        public void Update()
        {
            if (Player.main.inSeamoth)
            {
                if (prev_multiplier != CheatManager.seamothSpeedMultiplier || prev_seamothCanFly != CheatManager.seamothCanFly || prevSpeedModuleCount != SpeedModuleCount)
                {
                    float boost = 0f;

                    if (SpeedModuleCount > 0)
                    {
                        boost = SpeedModuleCount * SpeedModuleBoost;
                    }

                    if (CheatManager.seamothSpeedMultiplier == 1)
                    {
                        seamoth.forwardForce = Seamoth_forwardForce + boost;
                        seamoth.backwardForce = Seamoth_backwardForce;
                        seamoth.sidewardForce = Seamoth_sidewardForce;
                        seamoth.verticalForce = Seamoth_verticalForce;
                        prev_multiplier = CheatManager.seamothSpeedMultiplier;
                    }
                    else
                    {   float overDrive = MaxSpeed * (((float)SpeedModuleCount + 10) / 10);                     
                        seamoth.forwardForce = (Seamoth_forwardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((overDrive - (Seamoth_forwardForce + boost)) / 5));
                        seamoth.backwardForce = (Seamoth_backwardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((overDrive - (Seamoth_backwardForce + boost)) / 5));
                        seamoth.sidewardForce = (Seamoth_sidewardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((overDrive - (Seamoth_sidewardForce + boost)) / 5));
                        seamoth.verticalForce = (Seamoth_verticalForce + boost) + (CheatManager.seamothSpeedMultiplier * ((overDrive - (Seamoth_verticalForce + boost)) / 5));
                        prev_multiplier = CheatManager.seamothSpeedMultiplier;
                    }

                    prevSpeedModuleCount = SpeedModuleCount;

#if DEBUG_SEAMOTH_OVERDRIVE
                Logger.Log($"[CheatManager]\nSeamothOverDrive.Update() {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
                    seamoth.moveOnLand = CheatManager.seamothCanFly;
                    prev_seamothCanFly = CheatManager.seamothCanFly;
                }
            }
        }
    }
}
