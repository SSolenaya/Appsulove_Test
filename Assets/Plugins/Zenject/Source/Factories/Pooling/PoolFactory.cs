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

namespace Zenject
{
    // Here we assume that each spawned object does the work of returning itself to the pool
    // in its own Dispose method
    public class PoolWrapperFactory<T> : IFactory<T>
        where T : IDisposable
    {
        readonly IMemoryPool<T> _pool;

        public PoolWrapperFactory(IMemoryPool<T> pool)
        {
            _pool = pool;
        }

        public T Create()
        {
            return _pool.Spawn();
        }
    }

    public class PoolWrapperFactory<TParam1, TValue> : IFactory<TParam1, TValue>
        where TValue : IDisposable
    {
        readonly IMemoryPool<TParam1, TValue> _pool;

        public PoolWrapperFactory(IMemoryPool<TParam1, TValue> pool)
        {
            _pool = pool;
        }

        public TValue Create(TParam1 arg)
        {
            return _pool.Spawn(arg);
        }
    }
}
