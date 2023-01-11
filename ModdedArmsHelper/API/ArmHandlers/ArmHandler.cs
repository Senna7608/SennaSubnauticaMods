using UnityEngine;
#pragma warning disable IDE1006 //Naming styles

namespace ModdedArmsHelper.API.ArmHandlers
{
    /// <summary>
    /// The abstract component to inherit for handling the arm.
    /// </summary>
    public abstract class ArmHandler : MonoBehaviour
    {
        /// <summary>
        /// Implement this method in your arm handler.
        /// </summary>
        public abstract void Awake();
        /// <summary>
        /// Implement this method in your arm handler.
        /// </summary>
        public abstract void Start();

        private Vehicle _vehicle = null;

        /// <summary>
        /// This property is returning the <see cref="Vehicle"/> component.
        /// </summary>
        public Vehicle vehicle
        {
            get
            {
                if (_vehicle == null)
                {
                    _vehicle = GetComponentInParent<Vehicle>();
                }

                return _vehicle;
            }
        }

        private SeaMoth _seamoth = null;
        /// <summary>
        /// This property is returning the <see cref="SeaMoth"/> component. If exists.
        /// </summary> 
        public SeaMoth seamoth
        {
            get
            {
                if (_seamoth == null)
                {
                    _seamoth = GetComponentInParent<SeaMoth>();
                }

                return _seamoth;
            }
        }

        private Exosuit _exosuit = null;
        /// <summary>
        /// This property is returning the <see cref="Exosuit"/> component. If exists.
        /// </summary>
        public Exosuit exosuit
        {
            get
            {
                if (_exosuit == null)
                {
                    _exosuit = GetComponentInParent<Exosuit>();
                }

                return _exosuit;
            }
        }       

        private Animator _animator = null;
        /// <summary>
        /// This property is returning the <see cref="Animator"/> component. If exists in the arm gameobject.
        /// </summary>
        public Animator animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }

                return _animator;
            }
        }

        private EnergyInterface _energyInterface = null;
        /// <summary>
        /// This property is returning the <see cref="EnergyInterface"/> component. If exists in the arm parent gameobject.
        /// </summary>
        public EnergyInterface energyInterface
        {
            get
            {
                if (_energyInterface == null)
                {
                    _energyInterface = GetComponentInParent<EnergyInterface>();
                }

                return _energyInterface;
            }
        }

        private ArmTag _armTag = null;

        /// <summary>
        /// This property is returning the <see cref="ArmTag"/> component.
        /// </summary>
        public ArmTag armTag
        {
            get
            {
                if (_armTag == null)
                {
                    _armTag = GetComponent<ArmTag>();
                }

                return _armTag;
            }
        }

        private GameObject _lowerArm = null;
        /// <summary>
        /// This property is returning the lower arm gameobject container. Pivot point is the upper arm elbow.
        /// </summary>
        public GameObject lowerArm
        {
            get
            {
                if (_lowerArm == null)
                {
                    _lowerArm = ArmServices.main.objectHelper.FindDeepChild(transform, "ModdedLowerArmContainer");
                }

                return _lowerArm;
            }
        }
    }
}
