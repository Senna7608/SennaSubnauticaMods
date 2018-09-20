using UnityEngine;

namespace AncientSword
{
    public class AncientSword : PlayerTool
    {
        [AssertNotNull]
        public FMODAsset attackSound;

        [AssertNotNull]
        public FMODAsset underwaterMissSound;

        [AssertNotNull]
        public FMODAsset surfaceMissSound;

        public DamageType damageType;

        public float damage = 50f;

        public float attackDist = 2f;

        public VFXEventTypes vfxEventType;

        public void Start()
        {


        }

        public override void OnToolUseAnim(GUIHand hand)
        {
            Vector3 position = default(Vector3);
            GameObject gameObject = null;

            UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, attackDist, ref gameObject, ref position, true);

            if (gameObject == null)
            {
                InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();
                if (component != null && component.GetMostRecent() != null)
                {
                    gameObject = component.GetMostRecent().gameObject;
                }
            }

            if (gameObject)
            {
                LiveMixin liveMixin = gameObject.FindAncestor<LiveMixin>();

                if (IsValidTarget(liveMixin))
                {
                    if (liveMixin)
                    {
                        bool wasAlive = liveMixin.IsAlive();
                        liveMixin.TakeDamage(damage, position, damageType, null);
                        GiveResourceOnDamage(gameObject, liveMixin.IsAlive(), wasAlive);
                    }
                    Utils.PlayFMODAsset(attackSound, transform, 20f);
                    VFXSurface component2 = gameObject.GetComponent<VFXSurface>();
                    Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0f);
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), Player.main.transform);
                }
                else
                {
                    gameObject = null;
                }
            }

            if (gameObject == null && hand.GetActiveTarget() == null)
            {
                if (Player.main.IsUnderwater())
                {
                    Utils.PlayFMODAsset(underwaterMissSound, transform, 20f);
                }
                else
                {
                    Utils.PlayFMODAsset(surfaceMissSound, transform, 20f);
                }
            }
        }
        
        private static bool IsValidTarget(LiveMixin liveMixin)
        {
            if (!liveMixin)
            {
                return true;
            }

            if (liveMixin.weldable)
            {
                return false;
            }

            if (!liveMixin.knifeable)
            {
                return false;
            }
            EscapePod component = liveMixin.GetComponent<EscapePod>();
            return !component;
        }
        
        protected virtual int GetUsesPerHit()
        {
            return 1;
        }
        
        private void GiveResourceOnDamage(GameObject target, bool isAlive, bool wasAlive)
        {
            TechType techType = CraftData.GetTechType(target);

            HarvestType harvestTypeFromTech = CraftData.GetHarvestTypeFromTech(techType);

            if (techType == TechType.Creepvine)
            {
                GoalManager.main.OnCustomGoalEvent("Cut_Creepvine");
            }

            if ((harvestTypeFromTech == HarvestType.DamageAlive && wasAlive) || (harvestTypeFromTech == HarvestType.DamageDead && !isAlive))
            {
                int num = 1;

                if (harvestTypeFromTech == HarvestType.DamageAlive && !isAlive)
                {
                    num += CraftData.GetHarvestFinalCutBonus(techType);
                }

                TechType harvestOutputData = CraftData.GetHarvestOutputData(techType);

                if (harvestOutputData != TechType.None)
                {
                    CraftData.AddToInventory(harvestOutputData, num, false, false);
                }
            }
        }       
        
    }
}