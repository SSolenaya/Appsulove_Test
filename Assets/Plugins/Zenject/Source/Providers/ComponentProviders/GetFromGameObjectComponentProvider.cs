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

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    [NoReflectionBaking]
    public class GetFromGameObjectComponentProvider : IProvider
    {
        readonly GameObject _gameObject;
        readonly Type _componentType;
        readonly bool _matchSingle;

        // if concreteType is null we use the contract type from inject context
        public GetFromGameObjectComponentProvider(
            Type componentType, GameObject gameObject, bool matchSingle)
        {
            _componentType = componentType;
            _matchSingle = matchSingle;
            _gameObject = gameObject;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _componentType;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsNotNull(context);

            injectAction = null;

            if (_matchSingle)
            {
                var match = _gameObject.GetComponent(_componentType);

                Assert.IsNotNull(match, "Could not find component with type '{0}' on prefab '{1}'",
                _componentType, _gameObject.name);

                buffer.Add(match);
                return;
            }

            var allComponents = _gameObject.GetComponents(_componentType);

            Assert.That(allComponents.Length >= 1,
            "Expected to find at least one component with type '{0}' on prefab '{1}'",
            _componentType, _gameObject.name);

            buffer.AllocFreeAddRange(allComponents);
        }
    }
}

#endif

