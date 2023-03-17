
#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_METRO)

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
    public class AsyncMouseTrigger : AsyncTriggerBase
    {
        AsyncTriggerPromise<AsyncUnit> onMouseDown;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseDowns;
        AsyncTriggerPromise<AsyncUnit> onMouseDrag;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseDrags;
        AsyncTriggerPromise<AsyncUnit> onMouseEnter;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseEnters;
        AsyncTriggerPromise<AsyncUnit> onMouseExit;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseExits;
        AsyncTriggerPromise<AsyncUnit> onMouseOver;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseOvers;
        AsyncTriggerPromise<AsyncUnit> onMouseUp;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseUps;
        AsyncTriggerPromise<AsyncUnit> onMouseUpAsButton;
        AsyncTriggerPromiseDictionary<AsyncUnit> onMouseUpAsButtons;


        protected override IEnumerable<ICancelablePromise> GetPromises()
        {
            return Concat(onMouseDown, onMouseDowns, onMouseDrag, onMouseDrags, onMouseEnter, onMouseEnters, onMouseExit, onMouseExits, onMouseOver, onMouseOvers, onMouseUp, onMouseUps, onMouseUpAsButton, onMouseUpAsButtons);
        }


        void OnMouseDown()
        {
            TrySetResult(onMouseDown, onMouseDowns, AsyncUnit.Default);
        }


        public UniTask OnMouseDownAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseDown, ref onMouseDowns, cancellationToken);
        }


        void OnMouseDrag()
        {
            TrySetResult(onMouseDrag, onMouseDrags, AsyncUnit.Default);
        }


        public UniTask OnMouseDragAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseDrag, ref onMouseDrags, cancellationToken);
        }


        void OnMouseEnter()
        {
            TrySetResult(onMouseEnter, onMouseEnters, AsyncUnit.Default);
        }


        public UniTask OnMouseEnterAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseEnter, ref onMouseEnters, cancellationToken);
        }


        void OnMouseExit()
        {
            TrySetResult(onMouseExit, onMouseExits, AsyncUnit.Default);
        }


        public UniTask OnMouseExitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseExit, ref onMouseExits, cancellationToken);
        }


        void OnMouseOver()
        {
            TrySetResult(onMouseOver, onMouseOvers, AsyncUnit.Default);
        }


        public UniTask OnMouseOverAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseOver, ref onMouseOvers, cancellationToken);
        }


        void OnMouseUp()
        {
            TrySetResult(onMouseUp, onMouseUps, AsyncUnit.Default);
        }


        public UniTask OnMouseUpAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseUp, ref onMouseUps, cancellationToken);
        }


        void OnMouseUpAsButton()
        {
            TrySetResult(onMouseUpAsButton, onMouseUpAsButtons, AsyncUnit.Default);
        }


        public UniTask OnMouseUpAsButtonAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetOrAddPromise(ref onMouseUpAsButton, ref onMouseUpAsButtons, cancellationToken);
        }


    }
}

#endif


#endif
