using Common;
using Common.GUIHelper;
using System.Collections.Generic;
using UnityEngine;
using static Common.GameHelper;

namespace RuntimeHelper
{
    public static class GuiHelper
    {
        public static void SetScrollViewItems(this List<GuiItem> targetList, List<string> itemNames, float width)
        {
            targetList.Clear();
           
            List<Rect> rects = new Rect(0, 0, width, itemNames.Count * 24).SetGridItemsRect(1, itemNames.Count, 22, 5, 2, false, false, true);
            targetList.CreateGuiItemsGroup(itemNames, rects, GuiItemType.TAB, new GuiItemColor(), null, GuiItemState.NORMAL, textAnchor: TextAnchor.MiddleLeft);            
        }

        public static List<string> InitTransformNamesList(this List<Transform> transformsList)
        {
            List<string> names = new List<string>();

            for (int i = 0; i < transformsList.Count; i++)
            {                
                if (i == 0 && !transformsList[0].IsRoot())
                {                   
                    names.Add(transformsList[0].name + " (Parent)");                    
                    continue;                   
                }

                if (i == 1 && !transformsList[0].IsRoot())
                {                    
                    names.Add(transformsList[1].name + " (Base)");
                    continue;
                }

                if (transformsList[i].IsRoot())
                {
                    names.Add(transformsList[i].name + " (Root)");                    
                }                
                else
                {
                    names.Add(transformsList[i].name + $" ({transformsList[i].childCount})");
                }                
            }            

            return names;
        }

        public static List<string> InitTechTypeNamesList(this List<TechTypeData> techTypeList)
        {
            List<string> names = new List<string>();

            foreach (TechTypeData ttData in techTypeList)
            {
                names.Add(ttData.Name);
            }          

            return names;
        }       
    }
}
