﻿using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {
        public static List<GUIContent> MainWindow = new List<GUIContent>
        {
            new GUIContent("Set", "Set the Base gameobject to currently selected game object."),
            new GUIContent("Refresh", "Refresh the selected Base gameobject child list."),
            new GUIContent("Get Roots", "Get the current active scenes root gameobjects and show in main window."),
            new GUIContent("On", "Set the selected gameobject state to active."),
            new GUIContent("Off", "Set the selected gameobject state to inactive."),
            new GUIContent("Copy", "Copy a selected gameobject to a temporary gameobject and set state to inactive."),
            new GUIContent("Paste", "Paste a previously copied gameobject to a child of current Base gameobject.\nName set to 'newPastedObject' and state set to active."),
            new GUIContent("Destroy", "Destroy the selected gameobject and all childs."),
            new GUIContent("Relative to: Local", "Change the selected gameobject transform information to world space."),
            new GUIContent("Relative to: World", "Change the selected gameobject transform information to parent local space."),
            new GUIContent("Transform to Default", "Set the selected gameobject transform values to Unity defaults."),
            new GUIContent("Set Position to Zero", "Set the selected gameobject position values to zero (dependant on local/world space)."),
            new GUIContent("Set Rotation to Zero", "Set the selected gameobject rotation values to zero (dependant on local/world space)."),
            new GUIContent("Set Scale to One", "Set the selected gameobject scale values to one."),
            new GUIContent("Reset Transform", "Set the selected gameobject transform values to original."),
            new GUIContent("Reset Collider", "Set the selected gameobject currently selected collider values to original."),
            new GUIContent("Exit Program", "Destroy this RuntimeHelperZero instance."),
            new GUIContent("Mark", "Marks the gameobject and adds it to the list."),
        };

        public static List<GUIContent> ObjectWindow = new List<GUIContent>
        {
            new GUIContent("Add new empty", "Add a new empty gameobject to the current Base gameobject."),
            new GUIContent("Add selected TechType", "Add a new copy of a TechType based gameobject to the current Base gameobject."),
        };

        public static List<GUIContent> ComponentWindow = new List<GUIContent>
        {
            new GUIContent("Remove Component", "Removes selected component from selected game object."),
            new GUIContent("Add Component", "Add component for selected game object.")
        };

        public static List<GUIContent> RendererWindow = new List<GUIContent>
        {
            new GUIContent("Reset", "Set this renderer materials to original.")
        };

        public static List<GUIContent> ComponentInfoWindow = new List<GUIContent>
        {
            new GUIContent("Add to LOG", "Add all information to logfile."),
            new GUIContent("Set", "Tries to change base to selected gameobject.")
        };

        public static List<GUIContent> MarkWindow = new List<GUIContent>
        {
            new GUIContent("Set", "Tries to change base to selected gameobject."),
            new GUIContent("Unmark", "Remove the selected gameobject from the list.")
        };

        public static List<GUIContent> FMODWindow = new List<GUIContent>
        {
            new GUIContent("Get", "Tries to gets all FMOD asset from resources"),
            new GUIContent("Play", "Play the selected FMOD asset from the list."),
            new GUIContent("Stop", "Stop the last played FMOD asset."),
            new GUIContent("Log", "Log current asset info to output window.")
        };

    }
}
