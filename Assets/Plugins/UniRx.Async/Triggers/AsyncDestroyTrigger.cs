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

using System.Threading;
using UniRx.Async.Internal;
using UnityEngine;

namespace UniRx.Async.Triggers
{
    [DisallowMultipleComponent]
    public class AsyncDestroyTrigger : MonoBehaviour
    {
        bool called = false;
        UniTaskCompletionSource promise;
        CancellationTokenSource cancellationTokenSource; // main cancellation
        object canellationTokenSourceOrQueue;            // external from AddCancellationTriggerOnDestory

        public CancellationToken CancellationToken
        {
            get
            {
                if (cancellationTokenSource == null)
                {
                    cancellationTokenSource = new CancellationTokenSource();
                }
                return cancellationTokenSource.Token;
            }
        }

        /// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
        void OnDestroy()
        {
            called = true;
            promise?.TrySetResult();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            if (canellationTokenSourceOrQueue != null)
            {
                if (canellationTokenSourceOrQueue is CancellationTokenSource cts)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                else
                {
                    var q = (MinimumQueue<CancellationTokenSource>)canellationTokenSourceOrQueue;
                    while (q.Count != 0)
                    {
                        var c = q.Dequeue();
                        c.Cancel();
                        c.Dispose();
                    }
                }
                canellationTokenSourceOrQueue = null;
            }
        }

        /// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
        public UniTask OnDestroyAsync()
        {
            if (called) return UniTask.CompletedTask;
            return new UniTask(promise ?? (promise = new UniTaskCompletionSource()));
        }

        /// <summary>Add Cancellation Triggers on destroy</summary>
        public void AddCancellationTriggerOnDestory(CancellationTokenSource cts)
        {
            if (called)
            {
                cts.Cancel();
                cts.Dispose();
            }

            if (canellationTokenSourceOrQueue == null)
            {
                canellationTokenSourceOrQueue = cts;
            }
            else if (canellationTokenSourceOrQueue is CancellationTokenSource c)
            {
                var q = new MinimumQueue<CancellationTokenSource>(4);
                q.Enqueue(c);
                q.Enqueue(cts);
                canellationTokenSourceOrQueue = q;
            }
            else
            {
                ((MinimumQueue<CancellationTokenSource>)canellationTokenSourceOrQueue).Enqueue(cts);
            }
        }
    }
}

#endif