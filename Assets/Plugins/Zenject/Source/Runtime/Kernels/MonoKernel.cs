// The MIT License (MIT)
//
// Copyright (c) 2010-2015 Modest Tree Media  http://www.modesttree.com
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

#if !NOT_UNITY3D

using ModestTree;
using UnityEngine;

namespace Zenject
{
    public abstract class MonoKernel : MonoBehaviour
    {
        [InjectLocal]
        TickableManager _tickableManager = null;

        [InjectLocal]
        InitializableManager _initializableManager = null;

        [InjectLocal]
        DisposableManager _disposablesManager = null;

        bool _hasInitialized;
        bool _isDestroyed;

        protected bool IsDestroyed
        {
            get { return _isDestroyed; }
        }

        public virtual void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            // We don't put this in start in case Start is overridden
            if (!_hasInitialized)
            {
                _hasInitialized = true;
                _initializableManager.Initialize();
            }
        }

        public virtual void Update()
        {
            // Don't spam the log every frame if initialization fails and leaves it as null
            if (_tickableManager != null)
            {
                _tickableManager.Update();
            }
        }

        public virtual void FixedUpdate()
        {
            // Don't spam the log every frame if initialization fails and leaves it as null
            if (_tickableManager != null)
            {
                _tickableManager.FixedUpdate();
            }
        }

        public virtual void LateUpdate()
        {
            // Don't spam the log every frame if initialization fails and leaves it as null
            if (_tickableManager != null)
            {
                _tickableManager.LateUpdate();
            }
        }

        public virtual void OnDestroy()
        {
            // _disposablesManager can be null if we get destroyed before the Start event
            if (_disposablesManager != null)
            {
                Assert.That(!_isDestroyed);
                _isDestroyed = true;

                _disposablesManager.Dispose();
                _disposablesManager.LateDispose();
            }
        }
    }
}

#endif
