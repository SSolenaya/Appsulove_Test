
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

using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Async.Triggers
{
    [DisallowMultipleComponent]
    public class AsyncCollisionTrigger : AsyncTriggerBase
    {
        AsyncTriggerPromise<Collision> onCollisionEnter;
        AsyncTriggerPromiseDictionary<Collision> onCollisionEnters;
        AsyncTriggerPromise<Collision> onCollisionExit;
        AsyncTriggerPromiseDictionary<Collision> onCollisionExits;
        AsyncTriggerPromise<Collision> onCollisionStay;
        AsyncTriggerPromiseDictionary<Collision> onCollisionStays;


        protected override IEnumerable<ICancelablePromise> GetPromises()
        {
            return Concat(onCollisionEnter, onCollisionEnters, onCollisionExit, onCollisionExits, onCollisionStay, onCollisionStays);
        }


        void OnCollisionEnter(Collision collision)
        {
            TrySetResult(onCollisionEnter, onCollisionEnters, collision);
        }


        public UniTask<Collision> OnCollisionEnterAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onCollisionEnter, ref onCollisionEnters, cancellationToken);
        }


        void OnCollisionExit(Collision collisionInfo)
        {
            TrySetResult(onCollisionExit, onCollisionExits, collisionInfo);
        }


        public UniTask<Collision> OnCollisionExitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onCollisionExit, ref onCollisionExits, cancellationToken);
        }


        void OnCollisionStay(Collision collisionInfo)
        {
            TrySetResult(onCollisionStay, onCollisionStays, collisionInfo);
        }


        public UniTask<Collision> OnCollisionStayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onCollisionStay, ref onCollisionStays, cancellationToken);
        }


    }
}

#endif

