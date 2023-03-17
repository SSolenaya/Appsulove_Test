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

namespace Zenject
{
    // Zero parameters
    public class PoolableMemoryPool<TValue>
        : MemoryPool<TValue>
        where TValue : IPoolable
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TValue item)
        {
            item.OnSpawned();
        }
    }

    // One parameters
    public class PoolableMemoryPool<TParam1, TValue>
        : MemoryPool<TParam1, TValue>
        where TValue : IPoolable<TParam1>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TValue item)
        {
            item.OnSpawned(p1);
        }
    }

    // Two parameters
    public class PoolableMemoryPool<TParam1, TParam2, TValue>
        : MemoryPool<TParam1, TParam2, TValue>
        where TValue : IPoolable<TParam1, TParam2>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TValue item)
        {
            item.OnSpawned(p1, p2);
        }
    }

    // Three parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TValue item)
        {
            item.OnSpawned(p1, p2, p3);
        }
    }

    // Four parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4);
        }
    }

    // Five parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5);
        }
    }

    // Six parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6);
        }
    }

    // Seven parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7);
        }
    }

    // Eight parameters
    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>
        : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7, p8);
        }
    }
}

