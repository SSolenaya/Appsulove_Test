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

using UnityEngine;

namespace Zenject
{
    // Zero parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TValue> : MemoryPool<TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }

    // One parameter
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }

    // Two parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TParam1, TParam2, TValue>
        : MemoryPool<TParam1, TParam2, TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }

    // Three parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TParam1, TParam2, TParam3, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }

    // Four parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }

    // Five parameters
    // NOTE: For this to work, the given component must be at the root game object of the thing
    // you want to use in a pool
    public class MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : Component
    {
        Transform _originalParent;

        [Inject]
        public MonoMemoryPool()
        {
        }

        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            // Record the original parent which will be set to whatever is used in the UnderTransform method
            _originalParent = item.transform.parent;
        }

        protected override void OnDestroyed(TValue item)
        {
            GameObject.Destroy(item.gameObject);
        }

        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);

            if (item.transform.parent != _originalParent)
            {
                item.transform.SetParent(_originalParent, false);
            }
        }
    }
}
