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

namespace Zenject
{
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefab : ISubContainerCreator
    {
        readonly GameObjectCreationParameters _gameObjectBindInfo;
        readonly IPrefabProvider _prefabProvider;
        readonly DiContainer _container;

        public SubContainerCreatorByNewPrefab(
            DiContainer container, IPrefabProvider prefabProvider,
            GameObjectCreationParameters gameObjectBindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _prefabProvider = prefabProvider;
            _container = container;
        }

        public DiContainer CreateSubContainer(
            List<TypeValuePair> args, InjectContext parentContext, out Action injectAction)
        {
            Assert.That(args.IsEmpty());

            var prefab = _prefabProvider.GetPrefab();

            bool shouldMakeActive;
            var gameObject = _container.CreateAndParentPrefab(
                prefab, _gameObjectBindInfo, null, out shouldMakeActive);

            var context = gameObject.GetComponent<GameObjectContext>();

            Assert.That(context != null,
                "Expected prefab with name '{0}' to contain a component of type 'GameObjectContext' on the root", prefab.name);

            context.Install(_container);

            injectAction = () =>
            {
                // Note: We don't need to call ResolveRoots here because GameObjectContext does this for us
                _container.Inject(context);

                if (shouldMakeActive && !_container.IsValidating)
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                    {
                        gameObject.SetActive(true);
                    }
                }
            };

            return context.Container;
        }
    }
}

#endif
