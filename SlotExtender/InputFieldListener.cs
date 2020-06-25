using Common;
using UnityEngine;
using UWE;

namespace SlotExtender
{
    public class InputFieldListener : MonoBehaviour
    {
        public Utils.MonitoredValue<bool> onConsoleInputFieldActive = new Utils.MonitoredValue<bool>();

        public void Awake()
        {
#if DEBUG
            SNLogger.Debug("SlotExtender", "Method call: InputFieldListener.Awake()");
#endif 

            onConsoleInputFieldActive.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(OnConsoleInputFieldActive));
        }

        public void OnConsoleInputFieldActive(Utils.MonitoredValue<bool> isActive)
        {
#if DEBUG
            SNLogger.Debug("SlotExtender", $"Method call: InputFieldListener.OnConsoleInputFieldActive({isActive.value})");
#endif 
            Main.isConsoleActive = isActive.value;
        }

        public void OnDestroy()
        {
#if DEBUG
            SNLogger.Debug("SlotExtender", "Method call: InputFieldListener.OnDestroy()");
#endif 
            onConsoleInputFieldActive.changedEvent.RemoveHandler(this, OnConsoleInputFieldActive);
            Destroy(this);
        }
    }
}
