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

using System.Collections.Generic;

namespace Zenject
{
    public class ArrayPool<T> : StaticMemoryPoolBaseBase<T[]>
    {
        readonly int _length;

        public ArrayPool(int length)
            : base(OnDespawned)
        {
            _length = length;
        }

        static void OnDespawned(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = default(T);
            }
        }

        public T[] Spawn()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                return SpawnInternal();
            }
        }

        protected override T[] Alloc()
        {
            return new T[_length];
        }

        static readonly Dictionary<int, ArrayPool<T>> _pools =
            new Dictionary<int, ArrayPool<T>>();

        public static ArrayPool<T> GetPool(int length)
        {
            ArrayPool<T> pool;

            if (!_pools.TryGetValue(length, out pool))
            {
                pool = new ArrayPool<T>(length);
                _pools.Add(length, pool);
            }

            return pool;
        }
    }
}
