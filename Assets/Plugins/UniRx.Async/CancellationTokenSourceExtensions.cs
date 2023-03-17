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
using UnityEngine;
using UniRx.Async.Triggers;
using System;

namespace UniRx.Async
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAfterSlim(this CancellationTokenSource cts, int millisecondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
        {
            var delay = UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cts.Token);
            CancelAfterCore(cts, delay).Forget();
        }

        public static void CancelAfterSlim(this CancellationTokenSource cts, TimeSpan delayTimeSpan, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
        {
            var delay = UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, cts.Token);
            CancelAfterCore(cts, delay).Forget();
        }

        static async UniTaskVoid CancelAfterCore(CancellationTokenSource cts, UniTask delayTask)
        {
            var alreadyCanceled = await delayTask.SuppressCancellationThrow();
            if (!alreadyCanceled)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public static void RegisterRaiseCancelOnDestroy(this CancellationTokenSource cts, Component component)
        {
            RegisterRaiseCancelOnDestroy(cts, component.gameObject);
        }

        public static void RegisterRaiseCancelOnDestroy(this CancellationTokenSource cts, GameObject gameObject)
        {
            var trigger = gameObject.GetAsyncDestroyTrigger();
            trigger.AddCancellationTriggerOnDestory(cts);
        }
    }
}

#endif