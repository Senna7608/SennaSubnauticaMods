using UnityEngine;
using UWE;

namespace SlotExtender
{
    internal class SlotExtender : MonoBehaviour
    {
        public SlotExtender Instance { get; private set; }
        public Vehicle ThisVehicle { get; private set; }
        
        internal bool isActive = false;

        internal void Awake()
        {
            this.Instance = this;

            gameObject.AddComponent<Config.SlotExtenderConsoleCommand>();

            if (this.Instance.GetComponent<SeaMoth>())
            {
                this.ThisVehicle = this.Instance.GetComponent<SeaMoth>();

                foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                    this.ThisVehicle.modules.AddSlot(slotID);
            }
            else
            {
                this.ThisVehicle = this.Instance.GetComponent<Exosuit>();

                foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                    this.ThisVehicle.modules.AddSlot(slotID);
            }
        }

        internal void Start()
        {
            //forced triggering the Awake method in uGUI_Equipment for patching
            Player.main.GetPDA().Open();
            Player.main.GetPDA().Close();

            Utils.GetLocalPlayerComp().playerModeChanged.AddHandler(this.gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        internal void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (Player.main.GetVehicle() == this.ThisVehicle)
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

            if (!Player.main.inSeamoth && !Player.main.inExosuit)
                return; // Player not in vehicle. Exit method.

            if (Input.GetKeyDown(Config.Config.KEYBINDINGS[0]))
            {
                if (Player.main.GetPDA().isOpen)
                {
                    Player.main.GetPDA().Close();                    
                }
                else // Is Closed
                {
                    this.ThisVehicle.upgradesInput.OpenFromExternal();                    
                }
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[1]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 5);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[2]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 6);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[3]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 7);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[4]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 8);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[5]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 9);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[6]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 10);
            }
            else if (Input.GetKeyDown(Config.Config.KEYBINDINGS[7]))
            {
                this.ThisVehicle.SendMessage("SlotKeyDown", 11);
            }
        }

        internal static bool IsExtendedSeamothSlot(string slotName)
        {
            foreach (string slot in SlotHelper.NewSeamothSlotIDs)
            {
                if (slotName == slot)
                    return true;
            }

            return false;
        }
    }
}
