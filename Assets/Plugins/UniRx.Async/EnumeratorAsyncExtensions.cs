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
using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using UniRx.Async.Internal;
using UnityEngine;

namespace UniRx.Async
{
    public static class EnumeratorAsyncExtensions
    {
        public static IAwaiter GetAwaiter(this IEnumerator enumerator)
        {
            var awaiter = new EnumeratorAwaiter(enumerator, CancellationToken.None);
            if (!awaiter.IsCompleted)
            {
                PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, awaiter);
            }
            return awaiter;
        }

        public static UniTask ToUniTask(this IEnumerator enumerator)
        {
            var awaiter = new EnumeratorAwaiter(enumerator, CancellationToken.None);
            if (!awaiter.IsCompleted)
            {
                PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, awaiter);
            }
            return new UniTask(awaiter);
        }

        public static UniTask ConfigureAwait(this IEnumerator enumerator, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken))
        {
            var awaiter = new EnumeratorAwaiter(enumerator, cancellationToken);
            if (!awaiter.IsCompleted)
            {
                PlayerLoopHelper.AddAction(timing, awaiter);
            }
            return new UniTask(awaiter);
        }

        class EnumeratorAwaiter : IAwaiter, IPlayerLoopItem
        {
            IEnumerator innerEnumerator;
            CancellationToken cancellationToken;
            Action continuation;
            AwaiterStatus status;
            ExceptionDispatchInfo exception;

            public EnumeratorAwaiter(IEnumerator innerEnumerator, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    status = AwaiterStatus.Canceled;
                    return;
                }

                this.innerEnumerator = ConsumeEnumerator(innerEnumerator);
                this.status = AwaiterStatus.Pending;
                this.cancellationToken = cancellationToken;
                this.continuation = null;

                TaskTracker.TrackActiveTask(this, 2);
            }

            public bool IsCompleted => status.IsCompleted();

            public AwaiterStatus Status => status;

            public void GetResult()
            {
                switch (status)
                {
                    case AwaiterStatus.Succeeded:
                        break;
                    case AwaiterStatus.Pending:
                        Error.ThrowNotYetCompleted();
                        break;
                    case AwaiterStatus.Faulted:
                        exception.Throw();
                        break;
                    case AwaiterStatus.Canceled:
                        Error.ThrowOperationCanceledException();
                        break;
                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    InvokeContinuation(AwaiterStatus.Canceled);
                    return false;
                }

                var success = false;
                try
                {
                    if (innerEnumerator.MoveNext())
                    {
                        return true;
                    }
                    else
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    exception = ExceptionDispatchInfo.Capture(ex);
                }

                InvokeContinuation(success ? AwaiterStatus.Succeeded : AwaiterStatus.Faulted);
                return false;
            }

            void InvokeContinuation(AwaiterStatus status)
            {
                this.status = status;
                var cont = this.continuation;

                // cleanup
                TaskTracker.RemoveTracking(this);
                this.continuation = null;
                this.cancellationToken = CancellationToken.None;
                this.innerEnumerator = null;

                if (cont != null) cont.Invoke();
            }

            public void OnCompleted(Action continuation)
            {
                UnsafeOnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                Error.ThrowWhenContinuationIsAlreadyRegistered(this.continuation);
                this.continuation = continuation;
            }

            // Unwrap YieldInstructions

            static IEnumerator ConsumeEnumerator(IEnumerator enumerator)
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    if (current == null)
                    {
                        yield return null;
                    }
                    else if (current is CustomYieldInstruction)
                    {
                        // WWW, WaitForSecondsRealtime
                        var e2 = UnwrapWaitCustomYieldInstruction((CustomYieldInstruction)current);
                        while (e2.MoveNext())
                        {
                            yield return null;
                        }
                    }
                    else if (current is YieldInstruction)
                    {
                        IEnumerator innerCoroutine = null;
                        switch (current)
                        {
                            case AsyncOperation ao:
                                innerCoroutine = UnwrapWaitAsyncOperation(ao);
                                break;
                            case WaitForSeconds wfs:
                                innerCoroutine = UnwrapWaitForSeconds(wfs);
                                break;
                        }
                        if (innerCoroutine != null)
                        {
                            while (innerCoroutine.MoveNext())
                            {
                                yield return null;
                            }
                        }
                        else
                        {
                            yield return null;
                        }
                    }
                    else if (current is IEnumerator e3)
                    {
                        var e4 = ConsumeEnumerator(e3);
                        while (e4.MoveNext())
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        // WaitForEndOfFrame, WaitForFixedUpdate, others.
                        yield return null;
                    }
                }
            }

            // WWW and others as CustomYieldInstruction.
            static IEnumerator UnwrapWaitCustomYieldInstruction(CustomYieldInstruction yieldInstruction)
            {
                while (yieldInstruction.keepWaiting)
                {
                    yield return null;
                }
            }

            static readonly FieldInfo waitForSeconds_Seconds = typeof(WaitForSeconds).GetField("m_Seconds", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            static IEnumerator UnwrapWaitForSeconds(WaitForSeconds waitForSeconds)
            {
                var second = (float)waitForSeconds_Seconds.GetValue(waitForSeconds);
                var startTime = DateTimeOffset.UtcNow;
                while (true)
                {
                    yield return null;

                    var elapsed = (DateTimeOffset.UtcNow - startTime).TotalSeconds;
                    if (elapsed >= second)
                    {
                        break;
                    }
                };
            }

            static IEnumerator UnwrapWaitAsyncOperation(AsyncOperation asyncOperation)
            {
                while (!asyncOperation.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}

#endif