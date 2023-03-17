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

using UnityEngine;

namespace UniRx.Async.Triggers
{
    [DisallowMultipleComponent]
    public class AsyncStartTrigger : MonoBehaviour
    {
        bool awakeCalled = false;
        bool called = false;
        UniTaskCompletionSource promise;

        void Awake()
        {
            awakeCalled = true;
        }

        void Start()
        {
            called = true;
            promise?.TrySetResult();
        }

        public UniTask StartAsync()
        {
            if (called) return UniTask.CompletedTask;
            if (!awakeCalled)
            {
                PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AwakeMonitor(this));
            }
            return new UniTask(promise ?? (promise = new UniTaskCompletionSource()));
        }

        private void OnDestroy()
        {
            promise?.TrySetCanceled();
        }

        class AwakeMonitor : IPlayerLoopItem
        {
            readonly AsyncStartTrigger trigger;

            public AwakeMonitor(AsyncStartTrigger trigger)
            {
                this.trigger = trigger;
            }

            public bool MoveNext()
            {
                if (trigger.awakeCalled) return false;
                if (trigger == null)
                {
                    trigger.OnDestroy();
                    return false;
                }
                return true;
            }
        }
    }
}

#endif