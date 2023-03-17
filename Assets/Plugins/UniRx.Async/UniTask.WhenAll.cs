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
using System.Runtime.ExceptionServices;
using System.Threading;
using UniRx.Async.Internal;

namespace UniRx.Async
{
    public partial struct UniTask
    {
        // UniTask

        public static async UniTask<T[]> WhenAll<T>(params UniTask<T>[] tasks)
        {
            return await new WhenAllPromise<T>(tasks, tasks.Length);
        }

        public static async UniTask<T[]> WhenAll<T>(IEnumerable<UniTask<T>> tasks)
        {
            WhenAllPromise<T> promise;
            using (var span = ArrayPoolUtil.Materialize(tasks))
            {
                promise = new WhenAllPromise<T>(span.Array, span.Length);
            }

            return await promise;
        }

        public static async UniTask WhenAll(params UniTask[] tasks)
        {
            await new WhenAllPromise(tasks, tasks.Length);
        }

        public static async UniTask WhenAll(IEnumerable<UniTask> tasks)
        {
            WhenAllPromise promise;
            using (var span = ArrayPoolUtil.Materialize(tasks))
            {
                promise = new WhenAllPromise(span.Array, span.Length);
            }

            await promise;
        }

        class WhenAllPromise<T>
        {
            readonly T[] result;
            int completeCount;
            Action whenComplete;
            ExceptionDispatchInfo exception;

            public WhenAllPromise(UniTask<T>[] tasks, int tasksLength)
            {
                this.completeCount = 0;
                this.whenComplete = null;
                this.exception = null;
                this.result = new T[tasksLength];

                for (int i = 0; i < tasksLength; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        T value = default(T);
                        try
                        {
                            value = tasks[i].Result;
                        }
                        catch (Exception ex)
                        {
                            exception = ExceptionDispatchInfo.Capture(ex);
                            TryCallContinuation();
                            continue;
                        }

                        result[i] = value;
                        var count = Interlocked.Increment(ref completeCount);
                        if (count == result.Length)
                        {
                            TryCallContinuation();
                        }
                    }
                    else
                    {
                        RunTask(tasks[i], i).Forget();
                    }
                }
            }

            void TryCallContinuation()
            {
                var action = Interlocked.Exchange(ref whenComplete, null);
                if (action != null)
                {
                    action.Invoke();
                }
            }

            async UniTaskVoid RunTask(UniTask<T> task, int index)
            {
                T value = default(T);
                try
                {
                    value = await task;
                }
                catch (Exception ex)
                {
                    exception = ExceptionDispatchInfo.Capture(ex);
                    TryCallContinuation();
                    return;
                }

                result[index] = value;
                var count = Interlocked.Increment(ref completeCount);
                if (count == result.Length)
                {
                    TryCallContinuation();
                }
            }

            public Awaiter GetAwaiter()
            {
                return new Awaiter(this);
            }

            public struct Awaiter : ICriticalNotifyCompletion
            {
                WhenAllPromise<T> parent;

                public Awaiter(WhenAllPromise<T> parent)
                {
                    this.parent = parent;
                }

                public bool IsCompleted
                {
                    get
                    {
                        return parent.exception != null || parent.result.Length == parent.completeCount;
                    }
                }

                public T[] GetResult()
                {
                    if (parent.exception != null)
                    {
                        parent.exception.Throw();
                    }

                    return parent.result;
                }

                public void OnCompleted(Action continuation)
                {
                    UnsafeOnCompleted(continuation);
                }

                public void UnsafeOnCompleted(Action continuation)
                {
                    parent.whenComplete = continuation;
                    if (IsCompleted)
                    {
                        var action = Interlocked.Exchange(ref parent.whenComplete, null);
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
            }
        }

        class WhenAllPromise
        {
            int completeCount;
            int resultLength;
            Action whenComplete;
            ExceptionDispatchInfo exception;

            public WhenAllPromise(UniTask[] tasks, int tasksLength)
            {
                this.completeCount = 0;
                this.whenComplete = null;
                this.exception = null;
                this.resultLength = tasksLength;

                for (int i = 0; i < tasksLength; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        try
                        {
                            tasks[i].GetResult();
                        }
                        catch (Exception ex)
                        {
                            exception = ExceptionDispatchInfo.Capture(ex);
                            TryCallContinuation();
                            continue;
                        }

                        var count = Interlocked.Increment(ref completeCount);
                        if (count == resultLength)
                        {
                            TryCallContinuation();
                        }
                    }
                    else
                    {
                        RunTask(tasks[i], i).Forget();
                    }
                }
            }

            void TryCallContinuation()
            {
                var action = Interlocked.Exchange(ref whenComplete, null);
                if (action != null)
                {
                    action.Invoke();
                }
            }

            async UniTaskVoid RunTask(UniTask task, int index)
            {
                try
                {
                    await task;
                }
                catch (Exception ex)
                {
                    exception = ExceptionDispatchInfo.Capture(ex);
                    TryCallContinuation();
                    return;
                }

                var count = Interlocked.Increment(ref completeCount);
                if (count == resultLength)
                {
                    TryCallContinuation();
                }
            }

            public Awaiter GetAwaiter()
            {
                return new Awaiter(this);
            }

            public struct Awaiter : ICriticalNotifyCompletion
            {
                WhenAllPromise parent;

                public Awaiter(WhenAllPromise parent)
                {
                    this.parent = parent;
                }

                public bool IsCompleted
                {
                    get
                    {
                        return parent.exception != null || parent.resultLength == parent.completeCount;
                    }
                }

                public void GetResult()
                {
                    if (parent.exception != null)
                    {
                        parent.exception.Throw();
                    }
                }

                public void OnCompleted(Action continuation)
                {
                    UnsafeOnCompleted(continuation);
                }

                public void UnsafeOnCompleted(Action continuation)
                {
                    parent.whenComplete = continuation;
                    if (IsCompleted)
                    {
                        var action = Interlocked.Exchange(ref parent.whenComplete, null);
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
            }
        }
    }
}
#endif