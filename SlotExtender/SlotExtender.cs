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

            SNLogger.Log($"Broadcasting message: 'WakeUp', Vehicle Name: {ThisVehicle.GetName()}, Instance ID: {ThisVehicle.GetInstanceID()}");

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
                TryUseSlotItem(0);
                return;
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot2))
            {
                TryUseSlotItem(1);
                return;
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot3))
            {
                TryUseSlotItem(2);
                return;
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot4))
            {
                TryUseSlotItem(3);
                return;
            }
            else if (GameInput.GetButtonDown(GameInput.Button.Slot5))
            {
                TryUseSlotItem(4);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_6"]))
            {
                TryUseSlotItem(5);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_7"]))
            {
                TryUseSlotItem(6);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_8"]))
            {
                TryUseSlotItem(7);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_9"]))
            {
                TryUseSlotItem(8);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_10"]))
            {
                TryUseSlotItem(9);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_11"]))
            {
                TryUseSlotItem(10);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["Slot_12"]))
            {
                TryUseSlotItem(11);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS[SlotHelper.slotStringCache[SlotName.SeamothArmLeft]]))
            {
                TryUseSeamothArm(SlotHelper.slotStringCache[SlotName.SeamothArmLeft]);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS[SlotHelper.slotStringCache[SlotName.SeamothArmRight]]))
            {
                TryUseSeamothArm(SlotHelper.slotStringCache[SlotName.SeamothArmRight]);
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

        public void TryUseSlotItem(int slotID)
        {
            if (PdaMain.isOpen)
            {
                PdaMain.Close();
                return;
            }

            if (ThisVehicle.GetType() == typeof(SeaMoth))
            {
                if (ThisVehicle.GetSlotBinding(slotID) == TechType.VehicleStorageModule)
                {
                    int slot = slotID > 3 ? slotID - SEConfig.STORAGE_SLOTS_OFFSET : slotID;
                    ThisVehicle.GetComponent<SeaMoth>().storageInputs[slot].OpenFromExternal();
                    return;
                }
            }

            if (slotID > 5)
            {
                ThisVehicle.SendMessage("SlotKeyDown", slotID);
            }
        }

        public void TryUseSeamothArm(string seamothArmID)
        {
            if (ThisVehicle.GetType() == typeof(SeaMoth))
            {
                ThisVehicle.SlotKeyDown(ThisVehicle.GetSlotIndex(seamothArmID));
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
