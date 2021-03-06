﻿using System;
#pragma warning disable CS1591

namespace Common.Helpers
{
    public static class ArrayUtils
    {
        public static void Add<T>(ref T[] array, T newItem)
        {
            int newSize = array.Length + 1;
            Array.Resize(ref array, newSize);
            array[newSize - 1] = newItem;
        }
    }
}
