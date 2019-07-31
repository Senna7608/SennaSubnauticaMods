using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        public static List<GUIContent> MainWindow = new List<GUIContent>
        {
            new GUIContent("Set", "Set the Base game object to currently selected game object."),
            new GUIContent("Refresh", "Refresh the selected Base game object child list."),
            new GUIContent("Get Roots", "Get the current active scenes root game objects and show in main window."),
            new GUIContent("On", "Set the selected game object state to active."),
            new GUIContent("Off", "Set the selected game object state to inactive."),
            new GUIContent("Copy", "Copy a selected game object to a temporary game object and set state to inactive."),
            new GUIContent("Paste", "Paste a previously copied game object to a child of current Base game object.\nName set to 'newPastedObject' and state set to active."),
            new GUIContent("Destroy", "Destroy the selected game object and all childs."),
            new GUIContent("Relative to: Local", "Change the selected game object transform information to world space."),
            new GUIContent("Relative to: World", "Change the selected game object transform information to parent local space."),
            new GUIContent("Transform to Default", "Set the selected game object transform values to Unity defaults."),
            new GUIContent("Set Position to Zero", "Set the selected game object position values to zero (dependant on local/world space)."),
            new GUIContent("Set Rotation to Zero", "Set the selected game object rotation values to zero (dependant on local/world space)."),
            new GUIContent("Set Scale to One", "Set the selected game object scale values to one."),
            new GUIContent("Reset Transform", "Set the selected game object transform values to original."),
            new GUIContent("Reset Collider", "Set the selected game object currently selected collider values to original."),
            new GUIContent("Exit Program", "Destroy this RuntimeHelper instance."),
        };

        public static List<GUIContent> ObjectWindow = new List<GUIContent>
        {
            new GUIContent("Add new empty", "Add a new empty game object to the current Base game object."),
            new GUIContent("Add selected TechType", "Add a new copy of a TechType based game object to the current Base game object."),
        };

        public static List<GUIContent> ComponentWindow = new List<GUIContent>
        {
            new GUIContent("Remove Component", "Remove selected component from selected game object.")            
        };

        public static List<GUIContent> RendererWindow = new List<GUIContent>
        {
            new GUIContent("Reset", "Set this renderer materials to original.")
        };

    }
}
