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
    public class PrefabInstantiatorCached : IPrefabInstantiator
    {
        readonly IPrefabInstantiator _subInstantiator;

        GameObject _gameObject;

        public PrefabInstantiatorCached(IPrefabInstantiator subInstantiator)
        {
            _subInstantiator = subInstantiator;
        }

        public List<TypeValuePair> ExtraArguments
        {
            get { return _subInstantiator.ExtraArguments; }
        }

        public Type ArgumentTarget
        {
            get { return _subInstantiator.ArgumentTarget; }
        }

        public GameObjectCreationParameters GameObjectCreationParameters
        {
            get { return _subInstantiator.GameObjectCreationParameters; }
        }

        public UnityEngine.Object GetPrefab()
        {
            return _subInstantiator.GetPrefab();
        }

        public GameObject Instantiate(InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            // We can't really support arguments if we are using the cached value since
            // the arguments might change when called after the first time
            Assert.IsEmpty(args);

            if (_gameObject != null)
            {
                injectAction = null;
                return _gameObject;
            }

            _gameObject = _subInstantiator.Instantiate(context, new List<TypeValuePair>(), out injectAction);
            return _gameObject;
        }
    }
}

#endif
