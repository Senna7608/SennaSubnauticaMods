using UnityEngine;
using UWE;
using SlotExtender.Configuration;

namespace SlotExtender
{
    public class SlotExtender : MonoBehaviour
    {
        public SlotExtender Instance { get; private set; }
        private Vehicle ThisVehicle { get; set; }
        private Player PlayerMain { get; set; }        
        private PDA PdaMain { get; set; }
        internal bool isActive = false;        

        internal void Awake()
        {
            //get this SlotExtender instance
            Instance = gameObject.GetComponent<SlotExtender>();

            if (Instance.GetComponent<SeaMoth>())
            {
                //this Vehicle type is SeaMoth
                ThisVehicle = Instance.GetComponent<SeaMoth>();

                //add extra slots
                foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
            else if (Instance.GetComponent<Exosuit>())
            {
                //this Vehicle type is Exosuit
                ThisVehicle = Instance.GetComponent<Exosuit>();

                //add extra slots
                foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
            else
            {
                Logger.Log("Unknown Vehicle type error! Instance destroyed!");
                Destroy(Instance);
            }            
        }

        internal void Start()
        {    
            //get player instance
            PlayerMain = Player.main;
            PdaMain = PlayerMain.GetPDA();
            //forced triggering the Awake method in uGUI_Equipment for patching
            PdaMain.Open();
            PdaMain.Close();
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

            if (Input.GetKeyDown(Config.KEYBINDINGS["Upgrade"]))
            {
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }                    
                else // Is Closed
                {
                    ThisVehicle.upgradesInput.OpenFromExternal();
                    return;
                }
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Storage"]))
            {
                if (ThisVehicle.GetType() != typeof(Exosuit))
                    return;
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }
                else
                {
                    ThisVehicle.GetComponent<Exosuit>().storageContainer.Open();
                    return;
                }
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot1))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;
                if (ThisVehicle.GetSlotBinding(0) != TechType.VehicleStorageModule)
                    return;
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }
                else
                {
                    ThisVehicle.GetComponent<SeaMoth>().storageInputs[0].OpenFromExternal();
                    return;
                }
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot2))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;
                if (ThisVehicle.GetSlotBinding(1) != TechType.VehicleStorageModule)
                    return;
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }
                else
                {
                    ThisVehicle.GetComponent<SeaMoth>().storageInputs[1].OpenFromExternal();
                    return;
                }
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot3))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;
                if (ThisVehicle.GetSlotBinding(2) != TechType.VehicleStorageModule)
                    return;
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }
                else
                {
                    ThisVehicle.GetComponent<SeaMoth>().storageInputs[2].OpenFromExternal();
                    return;
                }
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot4))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;
                if (ThisVehicle.GetSlotBinding(3) != TechType.VehicleStorageModule)
                    return;
                if (PdaMain.isOpen)
                {
                    PdaMain.Close();
                    return;
                }
                else
                {
                    ThisVehicle.GetComponent<SeaMoth>().storageInputs[3].OpenFromExternal();
                    return;
                }
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_6"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 5);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_7"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 6);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_8"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 7);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_9"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 8);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_10"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 9);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_11"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 10);
                return;
            }
            else if (Input.GetKeyDown(Config.KEYBINDINGS["Slot_12"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 11);
                return;
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
