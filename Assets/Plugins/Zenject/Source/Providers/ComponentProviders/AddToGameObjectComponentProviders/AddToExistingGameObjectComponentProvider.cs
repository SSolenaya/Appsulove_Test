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
    public class AddToExistingGameObjectComponentProvider : AddToGameObjectComponentProviderBase
    {
        readonly GameObject _gameObject;

        public AddToExistingGameObjectComponentProvider(
            GameObject gameObject, DiContainer container, Type componentType,
            IEnumerable<TypeValuePair> extraArguments, object concreteIdentifier,
            Action<InjectContext, object> instantiateCallback)
            : base(container, componentType, extraArguments, concreteIdentifier, instantiateCallback)
        {
            _gameObject = gameObject;
        }

        // This will cause [Inject] to be triggered after awake / start
        // We could return true, but what if toggling active has other negative repercussions?
        // For now let's just not do anything
        protected override bool ShouldToggleActive
        {
            get { return false; }
        }

        protected override GameObject GetGameObject(InjectContext context)
        {
            return _gameObject;
        }
    }

    [NoReflectionBaking]
    public class AddToExistingGameObjectComponentProviderGetter : AddToGameObjectComponentProviderBase
    {
        readonly Func<InjectContext, GameObject> _gameObjectGetter;

        public AddToExistingGameObjectComponentProviderGetter(
            Func<InjectContext, GameObject> gameObjectGetter, DiContainer container, Type componentType,
            List<TypeValuePair> extraArguments, object concreteIdentifier,
            Action<InjectContext, object> instantiateCallback)
            : base(container, componentType, extraArguments, concreteIdentifier, instantiateCallback)
        {
            _gameObjectGetter = gameObjectGetter;
        }

        // This will cause [Inject] to be triggered after awake / start
        // We could return true, but what if toggling active has other negative repercussions?
        // For now let's just not do anything
        protected override bool ShouldToggleActive
        {
            get { return false; }
        }

        protected override GameObject GetGameObject(InjectContext context)
        {
            var gameObj = _gameObjectGetter(context);
            Assert.IsNotNull(gameObj, "Provided Func<InjectContext, GameObject> returned null value for game object when using FromComponentOn");
            return gameObj;
        }
    }
}

#endif
