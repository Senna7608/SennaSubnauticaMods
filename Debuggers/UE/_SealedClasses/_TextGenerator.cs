using UnityEngine;
using Debuggers.UE._Structures;

namespace Debuggers.UE._SealedClasses
{
    public static class _TextGenerator
    {
        public static void DebugTextGenerator(this TextGenerator textGenerator, string prefixString)
        {
            DLog.Log($"{prefixString}.characterCount: {textGenerator.characterCount}");
            DLog.Log($"{prefixString}.characterCountVisible: {textGenerator.characterCountVisible}");

            foreach (UICharInfo uICharInfo in textGenerator.characters)
            {
                uICharInfo.DebugUICharInfo($"{prefixString}.characters");
            }
            
            DLog.Log($"{prefixString}.fontSizeUsedForBestFit: {textGenerator.fontSizeUsedForBestFit}");
            DLog.Log($"{prefixString}.lineCount: {textGenerator.lineCount}");

            foreach (UILineInfo uILineInfo in textGenerator.lines)
            {
                uILineInfo.DebugUILineInfo($"{prefixString}.lines");
            }

            DLog.Log($"{prefixString}.rectExtents: {textGenerator.rectExtents}");
            DLog.Log($"{prefixString}.vertexCount: {textGenerator.vertexCount}");

            foreach (UIVertex uIVertex in textGenerator.verts)
            {
                uIVertex.DebugUIVertex($"{prefixString}.verts");
            }            
        }
    }
}
