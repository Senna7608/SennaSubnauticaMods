using UnityEngine;
using UWE;
using SlotExtender.Configuration;
using Common;
using static Common.GameHelper;
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
            //get this SlotExtender instance
            Instance = GetComponent<SlotExtender>();

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
                SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] Unknown Vehicle type error! Instance destroyed!");
                Destroy(Instance);
            }            
        }

        internal void Start()
        {               
            PdaMain = Player.main.GetPDA();
            //forced triggering the Awake method in uGUI_Equipment for patching
            PdaMain.Open();
            PdaMain.Close();

            //add and start a handler to check the player mode if changed
            Player.main.playerModeChanged.AddHandler(this, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));

            isActive = Player.main.currentMountedVehicle == ThisVehicle ? true : false;

            SNLogger.Log($"[SlotExtender] Broadcasting message: 'WakeUp', Vehicle Name: {ThisVehicle.GetName()}, Instance ID: {ThisVehicle.GetInstanceID()}");

            gameObject.BroadcastMessage("WakeUp", SendMessageOptions.DontRequireReceiver);
        }        

        internal void OnPlayerModeChanged(Player.Mode newMode)
        {
            if (newMode == Player.Mode.LockedPiloting)
            {
                //the player is in one of the vehicles but at this point Player.main.currentMountedVehicle is null.
                //therefore starting a coroutine while currentMountedVehicle is not null.
                StartCoroutine(WaitForPlayerModeChangeFinished(newMode));
            }
            else
            {
                //player not in any Vehicle: this Slot Extender now disabled
                isActive = false;
            }                        
        }

        private IEnumerator WaitForPlayerModeChangeFinished(Player.Mode newMode)
        {
            //print($"[{SEConfig.PROGRAM_NAME}] WaitForPlayerModeChangeFinished coroutine started for this Vehicle: {ThisVehicle.GetInstanceID()}");

            while (Player.main.currentMountedVehicle == null)
            {
                //print($"[{SEConfig.PROGRAM_NAME}] Player.main.currentMountedVehicle is NULL!");
                yield return null;
            }

            //print($"[{SEConfig.PROGRAM_NAME}] Player.main.currentMountedVehicle is {Player.main.currentMountedVehicle.GetInstanceID()}");
            //print($"[{SEConfig.PROGRAM_NAME}] WaitForPlayerModeChangeFinished coroutine stopped for this Vehicle: {ThisVehicle.GetInstanceID()}");
                         
            if (Player.main.currentMountedVehicle == ThisVehicle)
            {
                //player in this Vehicle: this Slot Extender now enabled
                isActive = true;
            }
            else
            {
                //player not in this Vehicle: this Slot Extender now disabled
                isActive = false;
            }            

            //print($"[{SEConfig.PROGRAM_NAME}] isActive now {isActive}, Player mode now {newMode}");

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
                ThisVehicle.SendMessage("SlotKeyDown", 12);
                return;
            }
            else if (Input.GetKeyDown(SEConfig.KEYBINDINGS["SeamothArmRight"]))
            {
                if (ThisVehicle.GetType() != typeof(SeaMoth))
                    return;
                ThisVehicle.SendMessage("SlotKeyDown", 13);
                return;
            }
        }        

        public void OnDestroy()
        {
            //remove unused handler from memory
            Player.main.playerModeChanged.RemoveHandler(this, OnPlayerModeChanged);            
            Destroy(Instance);
        }        
    }
}
