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
    // If you want to ensure that all items are always returned to the pool, include the following
    // in an installer
    // Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle()
    public class PoolCleanupChecker : ILateDisposable
    {
        readonly List<IMemoryPool> _poolFactories;
        readonly List<Type> _ignoredPools;

        public PoolCleanupChecker(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<IMemoryPool> poolFactories,
            [Inject(Source = InjectSources.Local)]
            List<Type> ignoredPools)
        {
            _poolFactories = poolFactories;
            _ignoredPools = ignoredPools;

            Assert.That(ignoredPools.All(x => x.DerivesFrom<IMemoryPool>()));
        }

        public void LateDispose()
        {
            foreach (var pool in _poolFactories)
            {
                if (!_ignoredPools.Contains(pool.GetType()))
                {
                    Assert.IsEqual(pool.NumActive, 0,
                        "Found active objects in pool '{0}' during dispose.  Did you forget to despawn an object of type '{1}'?".Fmt(pool.GetType(), pool.ItemType));
                }
            }
        }
    }
}
