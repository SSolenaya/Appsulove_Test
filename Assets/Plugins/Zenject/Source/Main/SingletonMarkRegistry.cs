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

namespace Zenject.Internal
{
    [NoReflectionBaking]
    public class SingletonMarkRegistry
    {
        readonly HashSet<Type> _boundSingletons = new HashSet<Type>();
        readonly HashSet<Type> _boundNonSingletons = new HashSet<Type>();

        public void MarkNonSingleton(Type type)
        {
            Assert.That(!_boundSingletons.Contains(type),
                "Found multiple creation bindings for type '{0}' in addition to AsSingle.  The AsSingle binding must be the definitive creation binding.  If this is intentional, use AsCached instead of AsSingle.", type);
            _boundNonSingletons.Add(type);
        }

        public void MarkSingleton(Type type)
        {
            bool added = _boundSingletons.Add(type);
            Assert.That(added, "Attempted to use AsSingle multiple times for type '{0}'.  As of Zenject 6+, AsSingle as can no longer be used for the same type across different bindings.  See the upgrade guide for details.", type);

            Assert.That(!_boundNonSingletons.Contains(type),
                "Found multiple creation bindings for type '{0}' in addition to AsSingle.  The AsSingle binding must be the definitive creation binding.  If this is intentional, use AsCached instead of AsSingle.", type);
        }

    }
}
