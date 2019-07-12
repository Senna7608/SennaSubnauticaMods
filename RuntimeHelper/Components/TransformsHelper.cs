﻿using Common;
using RuntimeHelper.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Components
{
    public static class TransformsHelper
    {
        public static void ScanObjectTransforms(this GameObject gameObject, ref List<Transform> transforms)
        {
            if (transforms == null)
            {
                transforms = new List<Transform>();
            }

            transforms.Clear();

            int childCount = gameObject.transform.childCount;

            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);

                    if (transform.name.StartsWith("RH_"))
                    {
                        continue;
                    }

                    transforms.Add(transform);
                }
            }

            transforms.Sort(SortByName);

            if (gameObject.transform.IsRoot())
            {
                transforms.Insert(0, gameObject.transform);
            }
            else
            {
                transforms.Insert(0, gameObject.transform.parent);
            }
        }

        public static int SortByName(Transform transform_1, Transform transform_2)
        {
            return transform_1.name.CompareTo(transform_2.name);            
        }

        private static bool CompareByTransform(Transform transform_1, Transform transform_2)
        {
            return transform_1.Equals(transform_2);
        }

        private static int SortByParentName(Transform transform_1, Transform transform_2)
        {
            return transform_1.parent.name.CompareTo(transform_2.parent.name);

        }

        public static void SetLocalPositionToZero(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void SetLocalRotationToZero(this Transform transform)
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public static void SetLocalScaleToOne(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }        

        public static void SetAllToZero(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }        

        public static void GetTransformLocals(this Transform source, ref TransformInfo transformInfo)
        {
            transformInfo.LocalPosition = source.localPosition;
            transformInfo.LocalRotation = source.localRotation;
            transformInfo.LocalScale = source.localScale;
        }

        public static void SetTransformLocals(this Transform destination, ref TransformInfo transformInfo)
        {
            destination.localPosition = transformInfo.LocalPosition;
            destination.localRotation = transformInfo.LocalRotation;
            destination.localScale = transformInfo.LocalScale;
        }        
    }
}