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
using System.Runtime.CompilerServices;

namespace UniRx.Async.Internal
{
    internal static class Error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowArgumentNullException<T>(T value, string paramName)
          where T : class
        {
            if (value == null) ThrowArgumentNullExceptionCore(paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowArgumentNullExceptionCore(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException<T>(string message)
        {
            throw new ArgumentException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowNotYetCompleted()
        {
            throw new InvalidOperationException("Not yet completed.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T ThrowNotYetCompleted<T>()
        {
            throw new InvalidOperationException("Not yet completed.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowWhenContinuationIsAlreadyRegistered<T>(T continuationField)
          where T : class
        {
            if (continuationField != null) ThrowInvalidOperationExceptionCore("continuation is already registered.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowInvalidOperationExceptionCore(string message)
        {
            throw new InvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowOperationCanceledException()
        {
            throw new OperationCanceledException();
        }
    }
}

#endif