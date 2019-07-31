using UnityEngine;
using Common.GUIHelper;
using RuntimeHelper.Visuals;
using RuntimeHelper.Components;
using static RuntimeHelper.SceneHelper.SceneHelper;
using Common;
using RuntimeHelper.Logger;
using RuntimeHelper.Objects;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        public void UpdateVisuals()
        {
            RefreshTransformIndex();

            isExistsCollider = selectedObject.IsExistsCollider();            

            RefreshEditModeList();

            SetObjectDrawing(true);

            RefreshComponentsList();            
        }

        private void OnBaseObjectChange()
        {            
            SetDirty(true);

            ReleaseObjectDrawing();

            baseObject = selectedObject;

            RefreshTransformsList();

            UpdateVisuals();

            OutputWindow_Log($"Base Object changed to [{baseObject.name}]");

            SetDirty(false);            
        }

        public void OnObjectChange(GameObject newObject)
        {            
            SetDirty(true);

            ReleaseObjectDrawing();

            selectedObject = newObject;

            UpdateVisuals();

            SetDirty(false);
        }

        private void OnObjectDestroy(bool immediate)
        {
            SetDirty(true);

            ReleaseObjectDrawing();

            if (selectedObject.IsNotNull())
            {
                OutputWindow_Log($"Object [{selectedObject.name}] now destroy.\nTransforms: parent [{selectedObject.transform.parent.name}], root [{selectedObject.transform.root.name}]");

                if (immediate)
                    DestroyImmediate(selectedObject);
                else
                    Destroy(selectedObject);                
            }

            try
            {
                selectedObject = TRANSFORMS[0].gameObject;
            }
            catch
            {
                selectedObject = gameObject;
            }

            if (baseObject.IsNull())
            {
                baseObject = selectedObject;
                OutputWindow_Log($"Base Object changed to [{baseObject.name}]");
            }

            RefreshTransformsList();

            UpdateVisuals();

            SetDirty(false);
        }
                
        private void OnRefresHBase()
        {            
            SetDirty(true);

            ReleaseObjectDrawing();

            if (baseObject.IsNull())
                return;

            selectedObject = baseObject;

            RefreshTransformsList();
            
            UpdateVisuals();

            SetScroll();

            OutputWindow_Log("Base transform list refreshed.");

            SetDirty(false);           
        }

        private void OnPasteObject()
        {
            SetDirty(true);            

            if (tempObject.IsNotNull())
            {
                tempObject.PasteObject(baseObject.transform);
                OutputWindow_Log($"Object [{tempObject.name}] pasted.\nName set to [newPastedObject]. Parent set to [{baseObject.transform.name}]");
            }

            RefreshTransformsList();

            UpdateVisuals();

            SetDirty(false);
        }

        private void ReleaseObjectDrawing()
        {
            if (selectedObject.IsNull())
                return;

            SetObjectDrawing(false);
            
            if (isExistsCollider)
                SetColliderDrawing(false, null);
                
        }


        private void GetRoots()
        {
            SetDirty(true);

            ReleaseObjectDrawing();

            TRANSFORMS.Clear();            

            GetRootTransforms(ref TRANSFORMS);

            TRANSFORMS.Sort(TransformsHelper.SortByName);

            guiItems_transforms.SetScrollViewItems(TRANSFORMS.InitTransformNamesList(), 278f);           

            RefreshTransformIndex();

            UpdateVisuals();

            OutputWindow_Log("Getting Root transforms.");

            isRootList = true;

            SetDirty(false);
        }
        
        private void RefreshTransformIndex()
        {
            current_transform_index = TRANSFORMS.FindIndex(item => item.Equals(selectedObject.transform));

            if (current_transform_index == -1)
            {
                current_transform_index = 0;
                selectedObject = TRANSFORMS[0].gameObject;                               
            }

            guiItems_transforms.SetStateInverseTAB(current_transform_index);

            SetScroll();
        }

        private void SetScroll()
        {
            SetScrollPos(ref scrollpos_transforms, current_transform_index);
        }

        private void SetObjectDrawing(bool value)
        {
            try
            {
                GameObject containerBase = selectedObject.GetOrAddVisualBase(BaseType.Object);
                DrawObjectBounds dob = containerBase.GetOrAddComponent<DrawObjectBounds>();
                dob.IsDraw(value);
            }
            catch
            {
                OutputWindow_Log($"GetOrAddComponent [DrawObjectBounds] to object [{selectedObject.name}] has failed!", LogType.Exception);
            }
        }

        private void SetColliderDrawing(bool value, Collider collider)
        {
            try
            {
                GameObject containerBase = selectedObject.GetOrAddVisualBase(BaseType.Collider);
                DrawColliderBounds dcb = containerBase.GetOrAddComponent<DrawColliderBounds>();

                if (collider.IsNotNull())
                {
                    dcb.SetColliderBase(collider);
                }

                dcb.IsDraw(value);
            }
            catch
            {
                OutputWindow_Log($"GetOrAddComponent [DrawColliderBounds] to object [{selectedObject.name}] has failed!", LogType.Exception);
            }
        }
    }
}
