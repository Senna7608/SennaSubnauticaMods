using UnityEngine;
using UWE;
using SlotExtender.Configuration;
using Common;
using System.Collections;

namespace SlotExtender
{
    public class SlotExtender : MonoBehaviour
    {
        public SlotExtender Instance { get; private set; }
        private Vehicle ThisVehicle;        
        private PDA PdaMain;
        internal bool isActive = false;        

        internal void Awake()
        {
            // set this SlotExtender instance
            Instance = this;

            if (gameObject.GetComponent<SeaMoth>())
            {
                // set this Vehicle to SeaMoth
                ThisVehicle = GetComponent<SeaMoth>();
            }
            else if (gameObject.GetComponent<Exosuit>())
            {
                // set this Vehicle to Exosuit
                ThisVehicle = Instance.GetComponent<Exosuit>();
            }                     
        }

        internal void Start()
        {
            // get and set PDA
            PdaMain = Player.main.GetPDA();

            // forced triggering the uGUI_Equipment constructor for start patching
            PdaMain.Open();
            PdaMain.Close();

            // adding and start a handler to check the player mode if changed
            Player.main.playerModeChanged.AddHandler(this, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));

            isActive = Player.main.currentMountedVehicle == ThisVehicle ? true : false;

            SNLogger.Log("SlotExtender", $"Broadcasting message: 'WakeUp', Vehicle Name: {ThisVehicle.GetName()}, Instance ID: {ThisVehicle.GetInstanceID()}");

            gameObject.BroadcastMessage("WakeUp", SendMessageOptions.DontRequireReceiver);
        }        

        internal void OnPlayerModeChanged(Player.Mode newMode)
        {
            if (newMode == Player.Mode.LockedPiloting)
            {
                // Player is in one of the vehicles but at this point Player.main.currentMountedVehicle is null.
                // therefore starting a coroutine while currentMountedVehicle is not null.
                StartCoroutine(WaitForPlayerModeChangeFinished(newMode));
            }
            else
            {
                // Player not in any Vehicle: this Slot Extender now disabled
                isActive = false;
            }                        
        }

        private IEnumerator WaitForPlayerModeChangeFinished(Player.Mode newMode)
        {            
            while (Player.main.currentMountedVehicle == null)
            {                
                yield return null;
            }
                                     
            if (Player.main.currentMountedVehicle == ThisVehicle)
            {
                // Player in this Vehicle: this Slot Extender now enabled
                isActive = true;
            }
            else
            {
                // Player not in this Vehicle: this Slot Extender now disabled
                isActive = false;
            }

            yield break;
        }

        internal void Update()
        {
            if (!isActive)
                return; // Slot Extender not active. Exit method.

            if (Main.isConsoleActive)
                return; // Input console active. Exit method.
            
            if (Main.isKeyBindigsUpdate)
                return; // Keybindings changed and updating in progress. Exit method.

            // Safety check
            if (!IsPlayerInVehicle())
                return; // Player not in any vehicle. Exit method.

            if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Upgrade"]))
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
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Storage"]))
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
                TryOpenSeamothStorage(0);
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot2))
            {
                TryOpenSeamothStorage(1);
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot3))
            {
                TryOpenSeamothStorage(2);
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot4))
            {
                TryOpenSeamothStorage(3);
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_6"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 5);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_7"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 6);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_8"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 7);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_9"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 8);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_10"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 9);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_11"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 10);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_12"]))
            {
                ThisVehicle.SendMessage("SlotKeyDown", 11);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["SeamothArmLeft"]))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;

                int slotID = SlotHelper.GetSeamothSlotInt(SlotConfigID.SeamothArmLeft);

                ThisVehicle.SendMessage("SlotKeyDown", slotID);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["SeamothArmRight"]))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;

                int slotID = SlotHelper.GetSeamothSlotInt(SlotConfigID.SeamothArmRight);

                ThisVehicle.SendMessage("SlotKeyDown", slotID);
                
                return;
            }
        }        

        public void OnDestroy()
        {
            // removing unused handler from memory
            Player.main.playerModeChanged.RemoveHandler(this, OnPlayerModeChanged);

            // destroying this SlotExtender instance
            Destroy(Instance);
        }        

        public void TryOpenSeamothStorage(int slotID)
        {
            if (ThisVehicle.GetType() != typeof(SeaMoth))
                return;
            if (ThisVehicle.GetSlotBinding(slotID) != TechType.VehicleStorageModule)
                return;
            if (PdaMain.isOpen)
            {
                PdaMain.Close();
                return;
            }
            else
            {
                ThisVehicle.GetComponent<SeaMoth>().storageInputs[slotID].OpenFromExternal();
                return;
            }
        }

        public bool IsPlayerInVehicle()
        {
            if (!Player.main)
                return false;

            return Player.main.inSeamoth || Player.main.inExosuit ? true : false;
        }
    }
}
