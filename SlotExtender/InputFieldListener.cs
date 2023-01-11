using UnityEngine;
using UWE;

namespace SlotExtender
{
    public class InputFieldListener : MonoBehaviour
    {
        public Utils.MonitoredValue<bool> onConsoleInputFieldActive = new Utils.MonitoredValue<bool>();

        public void Awake()
        {
            onConsoleInputFieldActive.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(OnConsoleInputFieldActive));
        }

        public void OnConsoleInputFieldActive(Utils.MonitoredValue<bool> isActive)
        {
            Main.isConsoleActive = isActive.value;
        }

        public void OnDestroy()
        {
            onConsoleInputFieldActive.changedEvent.RemoveHandler(this, OnConsoleInputFieldActive);
            Destroy(this);
        }
    }
}
