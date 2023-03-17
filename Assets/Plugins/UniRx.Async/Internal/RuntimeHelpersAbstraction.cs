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
using UnityEngine;

namespace UniRx.Async.Internal
{
    internal static class RuntimeHelpersAbstraction
    {
        // If we can use RuntimeHelpers.IsReferenceOrContainsReferences(.NET Core 2.0), use it.
        public static bool IsWellKnownNoReferenceContainsType<T>()
        {
            return WellKnownNoReferenceContainsType<T>.IsWellKnownType;
        }

        static bool WellKnownNoReferenceContainsTypeInitialize(Type t)
        {
            // The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
            if (t.IsPrimitive) return true;

            if (t.IsEnum) return true;
            if (t == typeof(DateTime)) return true;
            if (t == typeof(DateTimeOffset)) return true;
            if (t == typeof(Guid)) return true;
            if (t == typeof(decimal)) return true;

            // unwrap nullable
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return WellKnownNoReferenceContainsTypeInitialize(t.GetGenericArguments()[0]);
            }

            // or add other wellknown types(Vector, etc...) here
            if (t == typeof(Vector2)) return true;
            if (t == typeof(Vector3)) return true;
            if (t == typeof(Vector4)) return true;
            if (t == typeof(Color)) return true;
            if (t == typeof(Rect)) return true;
            if (t == typeof(Bounds)) return true;
            if (t == typeof(Quaternion)) return true;
            if (t == typeof(Vector2Int)) return true;
            if (t == typeof(Vector3Int)) return true;

            return false;
        }

        static class WellKnownNoReferenceContainsType<T>
        {
            public static readonly bool IsWellKnownType;

            static WellKnownNoReferenceContainsType()
            {
                IsWellKnownType = WellKnownNoReferenceContainsTypeInitialize(typeof(T));
            }
        }
    }
}

#endif