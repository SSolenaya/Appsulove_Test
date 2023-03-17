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

namespace UniRx.Async
{
    public partial struct UniTask
    {
        /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
        public static async UniTask Run(Action action, bool configureAwait = true)
        {
            await UniTask.SwitchToThreadPool();

            if (configureAwait)
            {
                try
                {
                    action();
                }
                finally
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                action();
            }
        }

        /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
        public static async UniTask Run(Action<object> action, object state, bool configureAwait = true)
        {
            await UniTask.SwitchToThreadPool();

            if (configureAwait)
            {
                try
                {
                    action(state);
                }
                finally
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                action(state);
            }
        }

        /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
        public static async UniTask<T> Run<T>(Func<T> func, bool configureAwait = true)
        {
            await UniTask.SwitchToThreadPool();
            if (configureAwait)
            {
                try
                {
                    return func();
                }
                finally
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                return func();
            }
        }

        /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
        public static async UniTask<T> Run<T>(Func<object, T> func, object state, bool configureAwait = true)
        {
            await UniTask.SwitchToThreadPool();

            if (configureAwait)
            {
                try
                {
                    return func(state);
                }
                finally
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                return func(state);
            }
        }
    }
}

#endif