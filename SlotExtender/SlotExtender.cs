using UnityEngine;
using UWE;

namespace SlotExtender
{
    internal class SlotExtender : MonoBehaviour 
    {
        public SlotExtender Instance { get; private set; }
        public Vehicle ThisVehicle { get; private set; }

        internal bool isOpen = false;
        internal bool isActive = false;                

        internal static readonly string[] newSeamothSlotIDs = new string[5]
        {
           "SeamothModule5",
           "SeamothModule6",
           "SeamothModule7",
           "SeamothModule8",
           "SeamothModule9"
        };

        internal static readonly string[] newExosuitSlotIDs = new string[4]
        {
           "ExosuitModule5",
           "ExosuitModule6",
           "ExosuitModule7",
           "ExosuitModule8"
        };        

        internal void Awake()
        {
            Instance = this;
            
            if (Instance.GetComponent<SeaMoth>())
            {
                ThisVehicle = Instance.GetComponent<SeaMoth>();
                ThisVehicle.modules.AddSlots(newSeamothSlotIDs);                
            }
            else
            {
                ThisVehicle = Instance.GetComponent<Exosuit>();
                ThisVehicle.modules.AddSlots(newExosuitSlotIDs);                
            }            
        }
        
        internal void Start()
        {  
            //forced triggering the Awake method in uGUI_Equipment for patching
            Player.main.GetPDA().Open();
            Player.main.GetPDA().Close();

            Utils.GetLocalPlayerComp().playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (Player.main.GetVehicle() == ThisVehicle)
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
            }
            else
            {
                isActive = false;
            }
        }

        internal void Update()
        {
            if (isActive)
            {
                if (Player.main.inSeamoth || Player.main.inExosuit)
                {
                    if (Input.GetKeyDown(KeyCode.R) && !isOpen)
                    {                       
                        ThisVehicle.upgradesInput.OpenFromExternal();
                        isOpen = true;
                    }
                    else if (isOpen && Input.GetKeyDown(KeyCode.R))
                    {
                        Player.main.GetPDA().Close();
                        isOpen = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        ThisVehicle.SendMessage("SlotKeyDown", 5);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        ThisVehicle.SendMessage("SlotKeyDown", 6);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        ThisVehicle.SendMessage("SlotKeyDown", 7);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        ThisVehicle.SendMessage("SlotKeyDown", 8);
                    }
                }
            }
        }

        internal static bool IsExtendedSeamothSlot(string slotName)
        {
            foreach (string slot in newSeamothSlotIDs)
            {
                if (slotName == slot)
                    return true;
            }

            return false;
        }        
    }
}
