using UnityEngine;
using UWE;

namespace SlotExtender
{
    public class SlotExtender : MonoBehaviour
    {
        public SlotExtender Instance { get; private set; }
        public Vehicle ThisVehicle { get; private set; }
        public Player PlayerMain { get; private set; }

        internal bool isActive = false;

        internal void Awake()
        {
            //get this SlotExtender instance
            Instance = gameObject.GetComponent<SlotExtender>();

            //add console commad for configuration window
            gameObject.AddComponent<Config.SxConfig>();

            if (Instance.GetComponent<SeaMoth>())
            {
                //this Vehicle type is SeaMoth
                ThisVehicle = Instance.GetComponent<SeaMoth>();

                //add extra slots
                foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
            else
            {
                //this Vehicle type is Exosuit
                ThisVehicle = Instance.GetComponent<Exosuit>();

                //add extra slots
                foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
        }

        internal void Start()
        {    
            //get player instance
            PlayerMain = Player.main;
            //forced triggering the Awake method in uGUI_Equipment for patching
            PlayerMain.GetPDA().Open();
            PlayerMain.GetPDA().Close();
            //add and start a handler to check the player mode if changed
            PlayerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (PlayerMain.GetVehicle() == ThisVehicle)
                {
                    //player in this Vehicle: this SlotExtender now enabled
                    isActive = true;
                }
                else
                {
                    //player not in this Vehicle: this Slot Extender now disabled
                    isActive = false;
                }
            }
            else
            {
                //player not in any Vehicle: this Slot Extender now disabled
                isActive = false;
            }
        }

        internal void Update()
        {
            if (!isActive)
                return; // Slot Extender not active. Exit method.

            if (!PlayerMain.inSeamoth && !PlayerMain.inExosuit)
                return; // Player not in vehicle. Exit method.

            if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Show"]))
            {
                if (PlayerMain.GetPDA().isOpen)
                {
                    PlayerMain.GetPDA().Close();                    
                }
                else // Is Closed
                {
                    ThisVehicle.upgradesInput.OpenFromExternal();                    
                }
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_6"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 5);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_7"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 6);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_8"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 7);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_9"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 8);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_10"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 9);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_11"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 10);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS["Slot_12"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 11);
            }
        }

        internal bool IsExtendedSeamothSlot(string slotName)
        {
            foreach (string slot in SlotHelper.NewSeamothSlotIDs)
            {
                if (slotName == slot)
                    return true;
            }            

            return false;
        }

        public void OnDestroy()
        {
            //remove unused handler from memory
            PlayerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);            
            Destroy(Instance);
        }
    }
}
