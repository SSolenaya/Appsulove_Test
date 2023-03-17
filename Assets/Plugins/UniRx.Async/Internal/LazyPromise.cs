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
using System.Threading;

namespace UniRx.Async.Internal
{
    internal sealed class LazyPromise : IAwaiter
    {
        Func<UniTask> factory;
        UniTask value;

        public LazyPromise(Func<UniTask> factory)
        {
            this.factory = factory;
        }

        void Create()
        {
            var f = Interlocked.Exchange(ref factory, null);
            if (f != null)
            {
                value = f();
            }
        }

        public bool IsCompleted
        {
            get
            {
                Create();
                return value.IsCompleted;
            }
        }

        public AwaiterStatus Status
        {
            get
            {
                Create();
                return value.Status;
            }
        }

        public void GetResult()
        {
            Create();
            value.GetResult();
        }

        void IAwaiter.GetResult()
        {
            GetResult();
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            Create();
            value.GetAwaiter().UnsafeOnCompleted(continuation);
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }
    }

    internal sealed class LazyPromise<T> : IAwaiter<T>
    {
        Func<UniTask<T>> factory;
        UniTask<T> value;

        public LazyPromise(Func<UniTask<T>> factory)
        {
            this.factory = factory;
        }

        void Create()
        {
            var f = Interlocked.Exchange(ref factory, null);
            if (f != null)
            {
                value = f();
            }
        }

        public bool IsCompleted
        {
            get
            {
                Create();
                return value.IsCompleted;
            }
        }

        public AwaiterStatus Status
        {
            get
            {
                    Create();
                    return value.Status;
            }
        }

        public T GetResult()
        {
            Create();
            return value.Result;
        }

        void IAwaiter.GetResult()
        {
            GetResult();
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            Create();
            value.GetAwaiter().UnsafeOnCompleted(continuation);
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }
    }
}

#endif