using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common;

namespace CyclopsLaserCannonModule
{
    public class CannonButton : MonoBehaviour
    {
        public CannonControl control_instance;
        private bool mouseHover;        
        public Image image;
        public EventTrigger eventTrigger;        

        public void Awake()
        {
            SNLogger.Debug("CannonButton: Awake started...");

            image = GetComponent<Image>();

            image.sprite = Main.buttonSprite;                       

            eventTrigger = GetComponent<EventTrigger>();

            foreach (EventTrigger.Entry entry in eventTrigger.triggers)
            {
                entry.callback.RemoveAllListeners();
            }

            eventTrigger.triggers.Clear();

            EventTrigger.Entry newEntry_1 = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            newEntry_1.callback.AddListener((data) => { OnPointerEnter(); });
            eventTrigger.triggers.Add(newEntry_1);

            EventTrigger.Entry newEntry_2 = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            newEntry_2.callback.AddListener((data) => { OnPointerExit(); });
            eventTrigger.triggers.Add(newEntry_2);

            EventTrigger.Entry newEntry_3 = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            newEntry_3.callback.AddListener((data) => { OnPointerClick(); });
            eventTrigger.triggers.Add(newEntry_3);            
        }

        
        public void OnPointerEnter()
        {
            if (Player.main.currentSub == control_instance.subroot)
            {
                mouseHover = true;
            }            
        }

        public void OnPointerExit()
        {            
            if (Player.main.currentSub == control_instance.subroot)
            {
                HandReticle main = HandReticle.main;
                main.SetIcon(HandReticle.IconType.Default, 1f);
                mouseHover = false;
            }            
        }

        public void OnPointerClick()
        {
            if(control_instance.isPiloting)
            {
                control_instance.Cannon_Camera.GetComponent<CannonCamera>().EnterCamera();
            }
        }

        private void Update()
        {
            if (mouseHover)
            {                
                HandReticle main = HandReticle.main;
                main.SetText(HandReticle.TextType.Hand, CannonConfig.language_settings["Item_Name"], false);
            }
        }        
    }
}
