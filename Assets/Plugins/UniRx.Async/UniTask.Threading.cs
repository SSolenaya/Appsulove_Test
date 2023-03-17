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
using System.Threading;
using System.Threading.Tasks;
using UniRx.Async.Internal;

namespace UniRx.Async
{
    public partial struct UniTask
    {
        /// <summary>
        /// If running on mainthread, do nothing. Otherwise, same as UniTask.Yield(PlayerLoopTiming.Update).
        /// </summary>
        public static SwitchToMainThreadAwaitable SwitchToMainThread()
        {
            return new SwitchToMainThreadAwaitable();
        }

        public static SwitchToThreadPoolAwaitable SwitchToThreadPool()
        {
            return new SwitchToThreadPoolAwaitable();
        }

        public static SwitchToTaskPoolAwaitable SwitchToTaskPool()
        {
            return new SwitchToTaskPoolAwaitable();
        }

        public static SwitchToSynchronizationContextAwaitable SwitchToSynchronizationContext(SynchronizationContext syncContext)
        {
            Error.ThrowArgumentNullException(syncContext, nameof(syncContext));
            return new SwitchToSynchronizationContextAwaitable(syncContext);
        }
    }

    public struct SwitchToMainThreadAwaitable
    {
        public Awaiter GetAwaiter() => new Awaiter();

        public struct Awaiter : ICriticalNotifyCompletion
        {
            public bool IsCompleted
            {
                get
                {
                    var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    if (PlayerLoopHelper.MainThreadId == currentThreadId)
                    {
                        return true; // run immediate.
                    }
                    else
                    {
                        return false; // register continuation.
                    }
                }
            }

            public void GetResult() { }

            public void OnCompleted(Action continuation)
            {
                PlayerLoopHelper.AddContinuation(PlayerLoopTiming.Update, continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                PlayerLoopHelper.AddContinuation(PlayerLoopTiming.Update, continuation);
            }
        }
    }

    public struct SwitchToThreadPoolAwaitable
    {
        public Awaiter GetAwaiter() => new Awaiter();

        public struct Awaiter : ICriticalNotifyCompletion
        {
            static readonly WaitCallback switchToCallback = Callback;

            public bool IsCompleted => false;
            public void GetResult() { }

            public void OnCompleted(Action continuation)
            {
                ThreadPool.UnsafeQueueUserWorkItem(switchToCallback, continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                ThreadPool.UnsafeQueueUserWorkItem(switchToCallback, continuation);
            }

            static void Callback(object state)
            {
                var continuation = (Action)state;
                continuation();
            }
        }
    }

    public struct SwitchToTaskPoolAwaitable
    {
        public Awaiter GetAwaiter() => new Awaiter();

        public struct Awaiter : ICriticalNotifyCompletion
        {
            static readonly Action<object> switchToCallback = Callback;

            public bool IsCompleted => false;
            public void GetResult() { }

            public void OnCompleted(Action continuation)
            {
                Task.Factory.StartNew(switchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                Task.Factory.StartNew(switchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }

            static void Callback(object state)
            {
                var continuation = (Action)state;
                continuation();
            }
        }
    }

    public struct SwitchToSynchronizationContextAwaitable
    {
        readonly SynchronizationContext synchronizationContext;

        public SwitchToSynchronizationContextAwaitable(SynchronizationContext synchronizationContext)
        {
            this.synchronizationContext = synchronizationContext;
        }

        public Awaiter GetAwaiter() => new Awaiter(synchronizationContext);

        public struct Awaiter : ICriticalNotifyCompletion
        {
            static readonly SendOrPostCallback switchToCallback = Callback;
            readonly SynchronizationContext synchronizationContext;

            public Awaiter(SynchronizationContext synchronizationContext)
            {
                this.synchronizationContext = synchronizationContext;
            }

            public bool IsCompleted => false;
            public void GetResult() { }

            public void OnCompleted(Action continuation)
            {
                synchronizationContext.Post(switchToCallback, continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                synchronizationContext.Post(switchToCallback, continuation);
            }

            static void Callback(object state)
            {
                var continuation = (Action)state;
                continuation();
            }
        }
    }
}

#endif