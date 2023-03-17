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
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class FactorySubContainerBinderWithParams<TContract> : FactorySubContainerBinderBase<TContract>
    {
        public FactorySubContainerBinderWithParams(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
            : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

#if !NOT_UNITY3D

        [System.Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab(Type installerType, UnityEngine.Object prefab)
        {
            return ByNewContextPrefab(installerType, prefab);
        }

        [System.Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab<TInstaller>(UnityEngine.Object prefab)
            where TInstaller : IInstaller
        {
            return ByNewContextPrefab<TInstaller>(prefab);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab<TInstaller>(UnityEngine.Object prefab)
            where TInstaller : IInstaller
        {
            return ByNewContextPrefab(typeof(TInstaller), prefab);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab(Type installerType, UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            Assert.That(installerType.DerivesFrom<MonoInstaller>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'MonoInstaller'", installerType);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc = 
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabWithParams(
                        installerType,
                        container,
                        new PrefabProvider(prefab),
                        gameObjectInfo), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }

        [System.Obsolete("ByNewPrefabResource has been renamed to ByNewContextPrefabResource to avoid confusion with ByNewPrefabResourceInstaller and ByNewPrefabResourceMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource<TInstaller>(string resourcePath)
            where TInstaller : IInstaller
        {
            return ByNewContextPrefabResource<TInstaller>(resourcePath);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource(
            Type installerType, string resourcePath)
        {
            return ByNewContextPrefabResource(installerType, resourcePath);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource<TInstaller>(string resourcePath)
            where TInstaller : IInstaller
        {
            return ByNewContextPrefabResource(typeof(TInstaller), resourcePath);
        }

        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource(
            Type installerType, string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectCreationParameters();

            ProviderFunc = 
                (container) => new SubContainerDependencyProvider(
                    ContractType, SubIdentifier,
                    new SubContainerCreatorByNewPrefabWithParams(
                        installerType,
                        container,
                        new PrefabProviderResource(resourcePath),
                        gameObjectInfo), false);

            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo, gameObjectInfo);
        }
#endif
    }
}
