using UnityEngine;

namespace Crowbar
{
    public class Crowbar : PlayerTool
    {
        public override string animToolName
        {
            get
            {
                return "knife";
            }
        }
        
        [AssertNotNull]
        public FMODAsset attackSound;

        [AssertNotNull]
        public FMODAsset underwaterMissSound;

        [AssertNotNull]
        public FMODAsset surfaceMissSound;
        
        public DamageType damageType = DamageType.Normal;

        public float damage = 50f;

        public float attackDist = 5f;

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

        int num = 0;

        private static readonly string[] ROTATION = new string[3]
        {
            "X",
            "Y",
            "Z"            
        };

        public void Update()
        {
            Quaternion qt = gameObject.transform.localRotation;

            if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                switch (num)
                {
                    case 0:
                        qt.x += 0.1f;
                        break;
                    case 1:
                        qt.y += 0.1f;
                        break;
                    case 2:
                        qt.z += 0.1f;
                        break;
                }
                gameObject.transform.localRotation = new Quaternion(qt.x, qt.y, qt.z, qt.w);
                Debug.Log($"Rotation: {qt}");
            }

            if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                switch (num)
                {
                    case 0:
                        qt.x -= 0.1f;
                        break;
                    case 1:
                        qt.y -= 0.1f;
                        break;
                    case 2:
                        qt.z -= 0.1f;
                        break;                    
                }
                gameObject.transform.localRotation = new Quaternion(qt.x, qt.y, qt.z, qt.w);
                Debug.Log($"Rotation: {qt}");
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                num--;
                if (num < 0)
                {
                    num = 2;
                }
                Debug.Log($"Rotation now: {ROTATION[num]}");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                num++;
                if (num > 2)
                {
                    num = 0;
                }
                Debug.Log($"Rotation now: {ROTATION[num]}");
            }           
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