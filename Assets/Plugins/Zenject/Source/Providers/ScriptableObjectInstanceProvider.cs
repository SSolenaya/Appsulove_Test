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
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class ScriptableObjectInstanceProvider : IProvider
    {
        readonly DiContainer _container;
        readonly Type _resourceType;
        readonly List<TypeValuePair> _extraArguments;
        readonly bool _createNew;
        readonly object _concreteIdentifier;
        readonly Action<InjectContext, object> _instantiateCallback;
        readonly UnityEngine.Object _resource;

        public ScriptableObjectInstanceProvider(
            UnityEngine.Object resource, Type resourceType,
            DiContainer container, IEnumerable<TypeValuePair> extraArguments,
            bool createNew, object concreteIdentifier,
            Action<InjectContext, object> instantiateCallback)
        {
            _container = container;
            Assert.DerivesFromOrEqual<ScriptableObject>(resourceType);

            _resource = resource;
            _extraArguments = extraArguments.ToList();
            _resourceType = resourceType;
            _createNew = createNew;
            _concreteIdentifier = concreteIdentifier;
            _instantiateCallback = instantiateCallback;
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
            return _resourceType;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsNotNull(context);

            if (_createNew)
            {
                buffer.Add(UnityEngine.ScriptableObject.Instantiate(_resource));
            }
            else
            {
                buffer.Add(_resource);
            }

            injectAction = () =>
            {
                for (int i = 0; i < buffer.Count; i++)
                {
                    var obj = buffer[i];

                    var extraArgs = ZenPools.SpawnList<TypeValuePair>();

                    extraArgs.AllocFreeAddRange(_extraArguments);
                    extraArgs.AllocFreeAddRange(args);

                    _container.InjectExplicit(
                        obj, _resourceType, extraArgs, context, _concreteIdentifier);

                    ZenPools.DespawnList(extraArgs);

                    if (_instantiateCallback != null)
                    {
                        _instantiateCallback(context, obj);
                    }
                }
            };
        }
    }
}

#endif

