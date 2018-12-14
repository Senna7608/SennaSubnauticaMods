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
            Instance = gameObject.GetComponent<SlotExtender>();

            gameObject.AddComponent<Config.ConsoleCommand>();

            if (Instance.GetComponent<SeaMoth>())
            {
                ThisVehicle = Instance.GetComponent<SeaMoth>();

                foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
            else
            {
                ThisVehicle = Instance.GetComponent<Exosuit>();

                foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                    ThisVehicle.modules.AddSlot(slotID);
            }
        }

        internal void Start()
        {            
            PlayerMain = Player.main;
            //forced triggering the Awake method in uGUI_Equipment for patching
            PlayerMain.GetPDA().Open();
            PlayerMain.GetPDA().Close();
            PlayerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (PlayerMain.GetVehicle() == ThisVehicle)
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
            PlayerMain.playerModeChanged.RemoveHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
            Destroy(Instance);
        }
    }
}
