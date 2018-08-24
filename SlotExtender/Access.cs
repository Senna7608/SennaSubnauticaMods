using UnityEngine;

namespace SlotExtender
{
    internal class Access : MonoBehaviour
    {
        private SlotExtender SlotExtenderMain;
        private bool isOpen = false;

        internal void Awake()
        {            
            SlotExtenderMain = SlotExtender.Instance;
        }

        internal void Update()
        {
            if (Player.main.inSeamoth && Player.main.GetVehicle() == SlotExtenderMain.seamoth)
            {
               if (Input.GetKeyDown(KeyCode.T) && !isOpen)
               {
                  SlotExtenderMain.SlotExtender_upgradesInput.OpenFromExternal();
                  isOpen = true;
                  return;
               }
               else if (Input.GetKeyDown(KeyCode.R) && !isOpen)
               {
                  SlotExtenderMain.seamoth.upgradesInput.OpenFromExternal();
                  isOpen = true;
                  return;
               }
               else if (isOpen && Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.R))
               {
                    PDA pda = Player.main.GetPDA();
                    pda.Close();
                    isOpen = false;
                    return;
               }

            }
        }
    }
}
