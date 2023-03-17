// The MIT License (MIT)
//
// Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniRx.Async.Internal
{
    internal static class ArrayUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureCapacity<T>(ref T[] array, int index)
        {
            if (array.Length <= index)
            {
                EnsureCore(ref array, index);
            }
        }

        // rare case, no inlining.
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void EnsureCore<T>(ref T[] array, int index)
        {
            var newSize = array.Length * 2;
            var newArray = new T[(index < newSize) ? newSize : (index * 2)];
            Array.Copy(array, 0, newArray, 0, array.Length);

            array = newArray;
        }

        /// <summary>
        /// Optimizing utility to avoid .ToArray() that creates buffer copy(cut to just size).
        /// </summary>
        public static (T[] array, int length) Materialize<T>(IEnumerable<T> source)
        {
            if (source is T[] array)
            {
                return (array, array.Length);
            }

            var defaultCount = 4;
            if (source is ICollection<T> coll)
            {
                defaultCount = coll.Count;
                var buffer = new T[defaultCount];
                coll.CopyTo(buffer, 0);
                return (buffer, defaultCount);
            }
            else if (source is IReadOnlyCollection<T> rcoll)
            {
                defaultCount = rcoll.Count;
            }

            if (defaultCount == 0)
            {
                return (Array.Empty<T>(), 0);
            }

            {
                var index = 0;
                var buffer = new T[defaultCount];
                foreach (var item in source)
                {
                    EnsureCapacity(ref buffer, index);
                    buffer[index++] = item;
                }

                return (buffer, index);
            }
        }
    }
}

#endif