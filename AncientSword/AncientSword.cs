using UnityEngine;

namespace AncientSword
{
    public class AncientSword : PlayerTool
    {
        public override string animToolName
        {
            get
            {
                return "knife";
            }
        }
        
        public Animator animator;

        public FMODAsset attackSound;        
        public FMODAsset underwaterMissSound;
        public FMODAsset surfaceMissSound;
        
        public DamageType damageType = DamageType.Normal;

        public float damage = 50f;

        public float attackDist = 6f;

        public VFXEventTypes vfxEventType = VFXEventTypes.knife;
        
        public override void OnToolUseAnim(GUIHand hand)
        {
            Vector3 position = default(Vector3);
            GameObject closestObject = null;

            UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, attackDist, ref closestObject, ref position, true);

            if (closestObject == null)
            {
                InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();
                if (component != null && component.GetMostRecent() != null)
                {
                    closestObject = component.GetMostRecent().gameObject;
                }
            }

            if (closestObject)
            {
                LiveMixin liveMixin = closestObject.FindAncestor<LiveMixin>();

                if (IsValidTarget(liveMixin))
                {
                    if (liveMixin)
                    {
                        bool wasAlive = liveMixin.IsAlive();
                        liveMixin.TakeDamage(damage, position, damageType, null);
                        GiveResourceOnDamage(closestObject, liveMixin.IsAlive(), wasAlive);
                    }
                    Utils.PlayFMODAsset(attackSound, transform, 20f);
                    VFXSurface component2 = closestObject.GetComponent<VFXSurface>();
                    Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0f);
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), Player.main.transform);
                }
                else
                {
                    closestObject = null;
                }
            }
            
            if (closestObject == null && hand.GetActiveTarget() == null)
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