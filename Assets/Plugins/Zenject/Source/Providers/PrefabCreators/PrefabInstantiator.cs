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
using Zenject.Internal;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    [NoReflectionBaking]
    public class PrefabInstantiator : IPrefabInstantiator
    {
        readonly IPrefabProvider _prefabProvider;
        readonly DiContainer _container;
        readonly List<TypeValuePair> _extraArguments;
        readonly GameObjectCreationParameters _gameObjectBindInfo;
        readonly Type _argumentTarget;
        readonly List<Type> _instantiateCallbackTypes;
        readonly Action<InjectContext, object> _instantiateCallback;

        public PrefabInstantiator(
            DiContainer container,
            GameObjectCreationParameters gameObjectBindInfo,
            Type argumentTarget,
            IEnumerable<Type> instantiateCallbackTypes,
            IEnumerable<TypeValuePair> extraArguments,
            IPrefabProvider prefabProvider,
            Action<InjectContext, object> instantiateCallback)
        {
            _prefabProvider = prefabProvider;
            _extraArguments = extraArguments.ToList();
            _container = container;
            _gameObjectBindInfo = gameObjectBindInfo;
            _argumentTarget = argumentTarget;
            _instantiateCallbackTypes = instantiateCallbackTypes.ToList();
            _instantiateCallback = instantiateCallback;
        }

        public GameObjectCreationParameters GameObjectCreationParameters
        {
            get { return _gameObjectBindInfo; }
        }

        public Type ArgumentTarget
        {
            get { return _argumentTarget; }
        }

        public List<TypeValuePair> ExtraArguments
        {
            get { return _extraArguments; }
        }

        public UnityEngine.Object GetPrefab()
        {
            return _prefabProvider.GetPrefab();
        }

        public GameObject Instantiate(InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            Assert.That(_argumentTarget == null || _argumentTarget.DerivesFromOrEqual(context.MemberType));

            bool shouldMakeActive;
            var gameObject = _container.CreateAndParentPrefab(
                GetPrefab(), _gameObjectBindInfo, context, out shouldMakeActive);
            Assert.IsNotNull(gameObject);

            injectAction = () =>
            {
                var allArgs = ZenPools.SpawnList<TypeValuePair>();

                allArgs.AllocFreeAddRange(_extraArguments);
                allArgs.AllocFreeAddRange(args);

                if (_argumentTarget == null)
                {
                    Assert.That(
                        allArgs.IsEmpty(),
                        "Unexpected arguments provided to prefab instantiator.  Arguments are not allowed if binding multiple components in the same binding");
                }

                if (_argumentTarget == null || allArgs.IsEmpty())
                {
                    _container.InjectGameObject(gameObject);
                }
                else
                {
                    _container.InjectGameObjectForComponentExplicit(
                        gameObject, _argumentTarget, allArgs, context, null);

                    Assert.That(allArgs.Count == 0);
                }

                ZenPools.DespawnList<TypeValuePair>(allArgs);

                if (shouldMakeActive && !_container.IsValidating)
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                    {
                        gameObject.SetActive(true);
                    }
                }

                if (_instantiateCallback != null)
                {
                    var callbackObjects = ZenPools.SpawnHashSet<object>();

                    foreach (var type in _instantiateCallbackTypes)
                    {
                        var obj = gameObject.GetComponentInChildren(type);

                        if (obj != null)
                        {
                            callbackObjects.Add(obj);
                        }
                    }

                    foreach (var obj in callbackObjects)
                    {
                        _instantiateCallback(context, obj);
                    }

                    ZenPools.DespawnHashSet(callbackObjects);
                }
            };

            return gameObject;
        }
    }
}

#endif
