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

        private float Def_backwardForce;
        private float Def_forwardForce;
        private float Def_sidewardForce;
        private float Def_verticalForce;

#if DEBUG_SEAMOTH_OVERDRIVE
        private string seamothID;
#endif
        public void Awake()
        {
            Main = this;           
        }

        public void Start()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();

            TechTypeExtensions.FromString("SpeedModule", out SpeedModule, true);

#if DEBUG_SEAMOTH_OVERDRIVE
            seamothID = gameObject.GetId();
            Logger.Log($"[CheatManager]\nSeamothOverDrive().Start(): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
            Def_forwardForce = seamoth.forwardForce;
            Def_backwardForce = seamoth.backwardForce;
            Def_sidewardForce = seamoth.sidewardForce;
            Def_verticalForce = seamoth.verticalForce;            

            seamothEquipment = seamoth.modules;
            seamothEquipment.onAddItem += SpeedModuleAddListener;
            seamothEquipment.onRemoveItem += SpeedModuleRemoveListener;            
        }

        public void OnDestroy()
        {

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive().OnDestroy(): {seamothID}\nGameObject Destroyed!");
#endif
            seamothEquipment.onAddItem -= SpeedModuleAddListener;
            seamothEquipment.onRemoveItem -= SpeedModuleRemoveListener;

            Destroy(this);
        }


        private void SpeedModuleAddListener(InventoryItem invItem)
        {
           SpeedModuleCount = seamoth.modules.GetCount(SpeedModule);

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive().Event(Add): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
        }

        private void SpeedModuleRemoveListener(InventoryItem invItem)
        {
            SpeedModuleCount = seamoth.modules.GetCount(SpeedModule);

#if DEBUG_SEAMOTH_OVERDRIVE
            Logger.Log($"[CheatManager]\nSeamothOverDrive().Event(Remove): {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
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
                        seamoth.forwardForce = Def_forwardForce + boost;
                        seamoth.backwardForce = Def_backwardForce;
                        seamoth.sidewardForce = Def_sidewardForce;
                        seamoth.verticalForce = Def_verticalForce;
                        prev_multiplier = CheatManager.seamothSpeedMultiplier;
                    }
                    else
                    {
                        seamoth.forwardForce = (Def_forwardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((MaxSpeed - (Def_forwardForce + boost)) / 5));
                        seamoth.backwardForce = (Def_backwardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((MaxSpeed - (Def_backwardForce + boost)) / 5));
                        seamoth.sidewardForce = (Def_sidewardForce + boost) + (CheatManager.seamothSpeedMultiplier * ((MaxSpeed - (Def_sidewardForce + boost)) / 5));
                        seamoth.verticalForce = (Def_verticalForce + boost) + (CheatManager.seamothSpeedMultiplier * ((MaxSpeed - (Def_verticalForce + boost)) / 5));
                        prev_multiplier = CheatManager.seamothSpeedMultiplier;
                    }

                    prevSpeedModuleCount = SpeedModuleCount;

#if DEBUG_SEAMOTH_OVERDRIVE
                Logger.Log($"[CheatManager]\nSeamothOverDrive().Update() {seamothID}\nForwardForce: {seamoth.forwardForce}\nBackWardForce: {seamoth.backwardForce}\nVerticalForce: {seamoth.verticalForce}\nSidewardForce: {seamoth.sidewardForce}");
#endif
                    seamoth.moveOnLand = CheatManager.seamothCanFly;
                    prev_seamothCanFly = CheatManager.seamothCanFly;
                }
            }
        }
    }
}
