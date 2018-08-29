using UnityEngine;

namespace SlotExtender
{
    internal class SlotExtender : MonoBehaviour 
    {
        internal static SlotExtender Instance { get; private set; }        

        private bool isOpen = false;

        private static readonly string[] newSeamothSlotIDs = new string[]
        {
           "SeamothModule5",
           "SeamothModule6",
           "SeamothModule7",
           "SeamothModule8",
           "SeamothModule9"
        };

        private static readonly string[] newExosuitSlotIDs = new string[]
        {
           "ExosuitModule5",
           "ExosuitModule6",
           "ExosuitModule7",
           "ExosuitModule8"
        };

        internal void Awake()
        {
            Instance = this;

            SeaMoth seamoth = gameObject.GetComponent<SeaMoth>();
            Exosuit exosuit = gameObject.GetComponent<Exosuit>();

            if (seamoth != null)
            {
                seamoth.modules.AddSlots(newSeamothSlotIDs);                              
            }            

            if (exosuit != null)
            {
                exosuit.modules.AddSlots(newExosuitSlotIDs);                
            }            
        }

        internal void Update()
        {
            if (Player.main.inSeamoth || Player.main.inExosuit)
            {
                if (Input.GetKeyDown(KeyCode.R) && !isOpen)
                {
                    Player.main.GetVehicle().upgradesInput.OpenFromExternal();
                    isOpen = true;                    
                }
                else if (isOpen && Input.GetKeyDown(KeyCode.R))
                {
                    Player.main.GetPDA().Close();
                    isOpen = false;
                }

                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    Player.main.GetVehicle().SendMessage("SlotKeyDown", 5);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    Player.main.GetVehicle().SendMessage("SlotKeyDown", 6);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    Player.main.GetVehicle().SendMessage("SlotKeyDown", 7);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    Player.main.GetVehicle().SendMessage("SlotKeyDown", 8);
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
