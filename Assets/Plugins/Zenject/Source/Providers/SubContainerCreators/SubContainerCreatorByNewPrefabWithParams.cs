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
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabWithParams : ISubContainerCreator
    {
        readonly DiContainer _container;
        readonly IPrefabProvider _prefabProvider;
        readonly Type _installerType;
        readonly GameObjectCreationParameters _gameObjectBindInfo;

        public SubContainerCreatorByNewPrefabWithParams(
            Type installerType, DiContainer container, IPrefabProvider prefabProvider,
            GameObjectCreationParameters gameObjectBindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _prefabProvider = prefabProvider;
            _container = container;
            _installerType = installerType;
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        IEnumerable<InjectableInfo> GetAllInjectableIncludingBaseTypes() 
        {
            var info = TypeAnalyzer.GetInfo(_installerType);

            while (info != null) 
            {
                foreach (var injectable in info.AllInjectables) 
                {
                    yield return injectable;
                }

                info = info.BaseTypeInfo;
            }
        }

        DiContainer CreateTempContainer(List<TypeValuePair> args)
        {
            var tempSubContainer = Container.CreateSubContainer();

            var allInjectables = GetAllInjectableIncludingBaseTypes();

            foreach (var argPair in args)
            {
                // We need to intelligently match on the exact parameters here to avoid the issue
                // brought up in github issue #217
                var match = allInjectables
                    .Where(x => argPair.Type.DerivesFromOrEqual(x.MemberType))
                    .OrderBy(x => ZenUtilInternal.GetInheritanceDelta(argPair.Type, x.MemberType)).FirstOrDefault();

                Assert.That(match != null,
                    "Could not find match for argument type '{0}' when injecting into sub container installer '{1}'",
                    argPair.Type, _installerType);

                tempSubContainer.Bind(match.MemberType)
                    .FromInstance(argPair.Value).WhenInjectedInto(_installerType);
            }

            return tempSubContainer;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext parentContext, out Action injectAction)
        {
            Assert.That(!args.IsEmpty());

            var prefab = _prefabProvider.GetPrefab();
            var tempContainer = CreateTempContainer(args);

            bool shouldMakeActive;
            var gameObject = tempContainer.CreateAndParentPrefab(
                prefab, _gameObjectBindInfo, null, out shouldMakeActive);

            var context = gameObject.GetComponent<GameObjectContext>();

            Assert.That(context != null,
                "Expected prefab with name '{0}' to container a component of type 'GameObjectContext'", prefab.name);

            context.Install(tempContainer);

            injectAction = () => 
            {
                // Note: We don't need to call ResolveRoots here because GameObjectContext does this for us
                tempContainer.Inject(context);

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

