using UnityEngine.UI;
using Debuggers.UE._SealedClasses;

namespace Debuggers.UE._Object._Component._Behaviour._MonoBehaviour._UIBehaviour._Graphic._MaskableGraphic
{
    public static class _Text
    {        
        public static void DebugText(this Text text, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Text] ({text.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.alignByGeometry: {text.alignByGeometry}");
            DLog.Log($"{prefixString}.alignment: {text.alignment}");
            text.cachedTextGenerator.DebugTextGenerator($"{prefixString}.cachedTextGenerator");
            text.cachedTextGeneratorForLayout.DebugTextGenerator($"{prefixString}.cachedTextGeneratorForLayout");           

        }       


    }
}
