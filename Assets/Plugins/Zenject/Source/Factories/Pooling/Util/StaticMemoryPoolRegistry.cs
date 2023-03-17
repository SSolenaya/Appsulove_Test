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
using ModestTree;

namespace Zenject
{
#if UNITY_EDITOR
    public static class StaticMemoryPoolRegistry
    {
        public static event Action<IMemoryPool> PoolAdded = delegate {};
        public static event Action<IMemoryPool> PoolRemoved = delegate {};

        readonly static List<IMemoryPool> _pools = new List<IMemoryPool>();

        public static IEnumerable<IMemoryPool> Pools
        {
            get { return _pools; }
        }

        public static void Add(IMemoryPool memoryPool)
        {
            _pools.Add(memoryPool);
            PoolAdded(memoryPool);
        }

        public static void Remove(IMemoryPool memoryPool)
        {
            _pools.RemoveWithConfirm(memoryPool);
            PoolRemoved(memoryPool);
        }
    }
#endif
}
