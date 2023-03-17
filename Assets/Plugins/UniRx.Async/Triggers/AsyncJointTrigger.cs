
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
    public class AsyncJointTrigger : AsyncTriggerBase
    {
        AsyncTriggerPromise<float> onJointBreak;
        AsyncTriggerPromiseDictionary<float> onJointBreaks;
        AsyncTriggerPromise<Joint2D> onJointBreak2D;
        AsyncTriggerPromiseDictionary<Joint2D> onJointBreak2Ds;


        protected override IEnumerable<ICancelablePromise> GetPromises()
        {
            return Concat(onJointBreak, onJointBreaks, onJointBreak2D, onJointBreak2Ds);
        }


        void OnJointBreak(float breakForce)
        {
            TrySetResult(onJointBreak, onJointBreaks, breakForce);
        }


        public UniTask<float> OnJointBreakAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onJointBreak, ref onJointBreaks, cancellationToken);
        }


        void OnJointBreak2D(Joint2D brokenJoint)
        {
            TrySetResult(onJointBreak2D, onJointBreak2Ds, brokenJoint);
        }


        public UniTask<Joint2D> OnJointBreak2DAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onJointBreak2D, ref onJointBreak2Ds, cancellationToken);
        }


    }
}

#endif

