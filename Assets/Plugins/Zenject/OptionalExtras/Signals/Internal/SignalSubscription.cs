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

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class SignalSubscription : IDisposable, IPoolable<KeyValuePair<SignalSubscriptionId, Action<object>>, SignalDeclaration>
    {
        readonly Pool _pool;

        SignalSubscriptionId _subscriptionId;
        SignalDeclaration _declaration;
        Action<object> _callback;
        BindingId _signalId;

        public SignalSubscription(Pool pool)
        {
            _pool = pool;

            SetDefaults();
        }

        public SignalSubscriptionId SubscriptionId
        {
            get { return _subscriptionId; }
        }
        
        public BindingId SignalId
        {
            get { return _signalId; }
        }

        public void OnSpawned(KeyValuePair<SignalSubscriptionId, Action<object>> callback, SignalDeclaration declaration)
        {
            Assert.IsNull(_callback);
            _subscriptionId = callback.Key;
            _declaration = declaration;
            _callback = callback.Value;
            // Cache this in case OnDeclarationDespawned is called
            _signalId = declaration.BindingId;

            declaration.Add(this);
        }

        public void OnDespawned()
        {
            if (_declaration != null)
            {
                _declaration.Remove(this);
            }

            SetDefaults();
        }

        void SetDefaults()
        {
            _callback = null;
            _declaration = null;
            _signalId = new BindingId();
        }

        public void Dispose()
        {
            // Allow calling this twice since signals automatically unsubscribe in SignalBus.LateDispose
            // and so this causes issues if users also unsubscribe in a MonoBehaviour OnDestroy on a
            // root game object
            if (!_pool.InactiveItems.Contains(this))
            {
                _pool.Despawn(this);
            }
        }

        // See comment in SignalDeclaration for why this exists
        public void OnDeclarationDespawned()
        {
            _declaration = null;
        }

        public void Invoke(object signal)
        {
            _callback(signal);
        }

        public class Pool : PoolableMemoryPool<KeyValuePair<SignalSubscriptionId, Action<object>>, SignalDeclaration, SignalSubscription>
        {
        }
    }
}
