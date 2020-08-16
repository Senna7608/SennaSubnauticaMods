using System.Collections.Generic;

namespace RuntimeHelper
{
    public enum MESSAGES
    {
        PROGRAM_STARTED,
        BASE_OBJECT_CHANGED,
        ACTIVE_STATE_CHANGE,
        OBJECT_COPIED,
        OBJECT_PASTED,
        TRANSFORM_TO_LOCAL,
        TRANSFORM_TO_WORLD,
        TRANSFORM_TO_ORIGINAL,
        LOCALS_TO_ZERO,
        WORLD_TO_ZERO,
        LOCAL_POS_TO_ZERO,
        WORLD_POS_TO_ZERO,
        LOCAL_ROT_TO_ZERO,
        WORLD_ROT_TO_ZERO,
        LOCAL_SCALE_TO_ONE,
        COLLIDER_TO_ORIGINAL,
        RAYCAST_STATE,
        NEW_EMPTY_OBJECT_ADDED,
        NEW_TECH_OBJECT_ADDED,
        TRY_TO_ADD_A_TECH_OBJECT,
        RENDERER_WINDOW_INFOTEXT,
        SELECTED_FILE,
        MATERIALS_TO_ORIGINAL,
        NEW_MATERIAL_CREATED,
        OBJECT_DESTROY,
        CHILD_OBJECTS_LIST_REFRESHED,
        COMPONENT_REMOVED,
        COMPONENT_CANNOT_REMOVED,
        GET_ROOTS,
        COMPONENT_STATE_MODIFIED,
        COMPONENT_ADDED,
        OBJECT_MARKED,
        OBJECT_UNMARKED,
        COMPONENT_INFORMATION_LOGGED
    }


    public enum WARNINGS
    {
        OBJECT_STATE_CANNOT_MODIFIED,
        OBJECT_CANNOT_COPIED,
        PROGRAM_OBJECT_CANNOT_DESTROY,
        ROOT_OBJECT_CANNOT_DESTROY,
        UNABLE_TO_RAYCAST,
        EVENT_CURRENT_NOT_READY,
        COMPONENT_IN_BLACKLIST,
        OBJECT_CANNOT_INSTANTIATE,
        EMERGENCY_METHOD_STARTED,
        TEMP_OBJECT_EMPTY,
        CANNOT_MARK,
        CANNOT_SET_MARKED,
        CANNOT_REMOVE_MARKED
    }


    public enum ERRORS
    {
        BASE_OBJECT_DESTROYED,
        SELECTED_OBJECT_DESTROYED,
        GET_OR_ADD_COMPONENT_ERROR,
        CANNOT_SWITCH_TO_OBJECT
    }

    public partial class RuntimeHelper
    {
        public static readonly Dictionary<MESSAGES, string> MESSAGE_TEXT = new Dictionary<MESSAGES, string>
        {
            { MESSAGES.PROGRAM_STARTED, "Runtime Helper started."},
            { MESSAGES.GET_ROOTS, "Getting active scenes root gameobjects."},
            { MESSAGES.ACTIVE_STATE_CHANGE, "Gameobject [{0}] active state now {1}."},
            { MESSAGES.OBJECT_COPIED, "Gameobject [{0}] is copied to a temporary gameobject and ready for paste."},
            { MESSAGES.TRANSFORM_TO_LOCAL, "Transform information now relative to parent local space."},
            { MESSAGES.TRANSFORM_TO_WORLD, "Transform information now relative to world space."},
            { MESSAGES.LOCALS_TO_ZERO, "Gameobject [{0}] local vectors set to: pos: (0,0,0); rot: (0,0,0); scale: (1,1,1)"},
            { MESSAGES.WORLD_TO_ZERO, "Gameobject [{0}] world vectors set to: pos: (0,0,0); rot: (0,0,0)"},
            { MESSAGES.LOCAL_POS_TO_ZERO, "Gameobject [{0}] local position set to zero."},
            { MESSAGES.WORLD_POS_TO_ZERO, "Gameobject [{0}] world position set to zero."},
            { MESSAGES.LOCAL_ROT_TO_ZERO, "Gameobject [{0}] local rotation set to zero."},
            { MESSAGES.WORLD_ROT_TO_ZERO, "Gameobject [{0}] world rotation set to zero."},
            { MESSAGES.LOCAL_SCALE_TO_ONE, "Gameobject [{0}] local scale set to one."},
            { MESSAGES.TRANSFORM_TO_ORIGINAL, "Gameobject [{0}] transform values set to original."},
            { MESSAGES.COLLIDER_TO_ORIGINAL, "Collider [{0}] values set to original."},
            { MESSAGES.RAYCAST_STATE, "Raycast mode now {0}."},
            { MESSAGES.NEW_EMPTY_OBJECT_ADDED, "New empty gameobject added, name: [{0}], parent: [{1}], root: [{2}]"},
            { MESSAGES.NEW_TECH_OBJECT_ADDED, "New TechType based gameobject added, name: [{0}], parent: [{1}], root: [{2}]"},
            { MESSAGES.TRY_TO_ADD_A_TECH_OBJECT, "Trying to instantiate selected TechType [{0}][{1}] from prefab."},
            { MESSAGES.RENDERER_WINDOW_INFOTEXT, "Information: []: Material index, S: Shader, P: Property keyword, V: Property value"},
            { MESSAGES.SELECTED_FILE, "Selected file with full path: {0}"},
            { MESSAGES.MATERIALS_TO_ORIGINAL, "Renderer materials set to original."},
            { MESSAGES.NEW_MATERIAL_CREATED, "New material created and set."},
            { MESSAGES.BASE_OBJECT_CHANGED, "Base gameobject changed to [{0}]."},
            { MESSAGES.OBJECT_DESTROY, "Gameobject [{0}] now destroy.Transforms: parent: [{1}], root: [{1}]"},
            { MESSAGES.CHILD_OBJECTS_LIST_REFRESHED, "Base child gameobjects list refreshed."},
            { MESSAGES.OBJECT_PASTED, "Gameobject [{0}] pasted.Name set to [newPastedObject]. Parent set to [{1}]."},
            { MESSAGES.COMPONENT_REMOVED, "Component [{0}] removed from this gameobject [{1}]."},
            { MESSAGES.COMPONENT_CANNOT_REMOVED, "Component [{0}] cannot be removed from this gameobject [{1}]."},
            { MESSAGES.COMPONENT_STATE_MODIFIED, "Component [{0}] active state now {1}."},
            { MESSAGES.COMPONENT_ADDED, "Component [{0}] added to gameobject [{1}]."},
            { MESSAGES.OBJECT_MARKED, "Gameobject [{0}] marked and added to list."},
            { MESSAGES.OBJECT_UNMARKED, "Gameobject [{0}] unmarked and removed from list."},
            { MESSAGES.COMPONENT_INFORMATION_LOGGED, "Component [{0}] information add to logfile."},
        };


        public static readonly Dictionary<WARNINGS, string> WARNING_TEXT = new Dictionary<WARNINGS, string>
        {
            { WARNINGS.OBJECT_STATE_CANNOT_MODIFIED, "Gameobject [{0}] active state cannot be modified!"},
            { WARNINGS.OBJECT_CANNOT_COPIED, "Gameobject [{0}] is cannot be copied!"},
            { WARNINGS.PROGRAM_OBJECT_CANNOT_DESTROY, "Gameobject [{0}] is cannot be destroyed! Use 'Exit Program' button!"},
            { WARNINGS.ROOT_OBJECT_CANNOT_DESTROY, "Root objects cannot be destroyed!"},
            { WARNINGS.UNABLE_TO_RAYCAST, "Unable to start raycast mode!"},
            { WARNINGS.EVENT_CURRENT_NOT_READY, "Event current is not ready!"},
            { WARNINGS.COMPONENT_IN_BLACKLIST, "Component [{0}] is in Blacklist and cannot be removed!"},
            { WARNINGS.OBJECT_CANNOT_INSTANTIATE, "The selected TechType [{0}][{1}] cannot be instantiated!"},
            { WARNINGS.EMERGENCY_METHOD_STARTED, "Emergency method started."},
            { WARNINGS.TEMP_OBJECT_EMPTY, "Cannot paste. Temp object is empty."},
            { WARNINGS.CANNOT_MARK, "Cannot mark. Gameobject is already in the list."},
            { WARNINGS.CANNOT_SET_MARKED, "Cannot set. List is empty."},
            { WARNINGS.CANNOT_REMOVE_MARKED, "Cannot remove. List is empty."},
        };


        public static readonly Dictionary<ERRORS, string> ERROR_TEXT = new Dictionary<ERRORS, string>
        {
            { ERRORS.BASE_OBJECT_DESTROYED, "*** Base gameobject unexpectedly destroyed!"},
            { ERRORS.SELECTED_OBJECT_DESTROYED, "*** Selected gameobject unexpectedly destroyed!"},
            { ERRORS.GET_OR_ADD_COMPONENT_ERROR, "*** Get or Add Component [{0}] to gameobject [{1}] has failed!"},
            { ERRORS.CANNOT_SWITCH_TO_OBJECT, "*** Cannot swittch base to this gameobject [{0}]!"},
        };

    }
}
