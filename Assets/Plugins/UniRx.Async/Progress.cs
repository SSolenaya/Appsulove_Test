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

#if NET_4_6 || NET_STANDARD_2_0 || CSHARP_7_OR_LATER
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using UniRx.Async.Internal;

namespace UniRx.Async
{
    /// <summary>
    /// Lightweight IProgress[T] factory.
    /// </summary>
    public static class Progress
    {
        public static IProgress<T> Create<T>(Action<T> handler)
        {
            if (handler == null) return NullProgress<T>.Instance;
            return new AnonymousProgress<T>(handler);
        }

        public static IProgress<T> CreateOnlyValueChanged<T>(Action<T> handler, IEqualityComparer<T> comparer = null)
        {
            if (handler == null) return NullProgress<T>.Instance;
            return new OnlyValueChangedProgress<T>(handler, comparer ?? UnityEqualityComparer.GetDefault<T>());
        }

        sealed class NullProgress<T> : IProgress<T>
        {
            public static readonly IProgress<T> Instance = new NullProgress<T>();

            NullProgress()
            {

            }

            public void Report(T value)
            {
            }
        }

        sealed class AnonymousProgress<T> : IProgress<T>
        {
            readonly Action<T> action;

            public AnonymousProgress(Action<T> action)
            {
                this.action = action;
            }

            public void Report(T value)
            {
                action(value);
            }
        }

        sealed class OnlyValueChangedProgress<T> : IProgress<T>
        {
            readonly Action<T> action;
            readonly IEqualityComparer<T> comparer;
            bool isFirstCall;
            T latestValue;

            public OnlyValueChangedProgress(Action<T> action, IEqualityComparer<T> comparer)
            {
                this.action = action;
                this.comparer = comparer;
                this.isFirstCall = true;
            }

            public void Report(T value)
            {
                if (isFirstCall)
                {
                    isFirstCall = false;
                }
                else if (comparer.Equals(value, latestValue))
                {
                    return;
                }

                latestValue = value;
                action(value);
            }
        }
    }
}

#endif