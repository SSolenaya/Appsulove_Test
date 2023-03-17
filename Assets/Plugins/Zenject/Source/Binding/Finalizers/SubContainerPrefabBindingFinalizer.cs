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
    public class SubContainerPrefabBindingFinalizer : ProviderBindingFinalizer
    {
        readonly object _subIdentifier;
        readonly bool _resolveAll;
        readonly Func<DiContainer, ISubContainerCreator> _subContainerCreatorFactory;

        public SubContainerPrefabBindingFinalizer(
            BindInfo bindInfo,
            object subIdentifier, bool resolveAll,
            Func<DiContainer, ISubContainerCreator> subContainerCreatorFactory)
            : base(bindInfo)
        {
            _subIdentifier = subIdentifier;
            _resolveAll = resolveAll;
            _subContainerCreatorFactory = subContainerCreatorFactory;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (BindInfo.ToChoice == ToChoices.Self)
            {
                Assert.IsEmpty(BindInfo.ToTypes);
                FinalizeBindingSelf(container);
            }
            else
            {
                FinalizeBindingConcrete(container, BindInfo.ToTypes);
            }
        }

        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) => new SubContainerDependencyProvider(
                            concreteType, _subIdentifier,
                            _subContainerCreatorFactory(container), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        _subContainerCreatorFactory(container));

                    RegisterProvidersForAllContractsPerConcreteType(
                        container,
                        concreteTypes,
                        (_, concreteType) =>
                        new SubContainerDependencyProvider(
                            concreteType, _subIdentifier, containerCreator, _resolveAll));
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }

        void FinalizeBindingSelf(DiContainer container)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                {
                    RegisterProviderPerContract(
                        container,
                        (_, contractType) => new SubContainerDependencyProvider(
                            contractType, _subIdentifier,
                            _subContainerCreatorFactory(container), _resolveAll));
                    break;
                }
                case ScopeTypes.Singleton:
                {
                    var containerCreator = new SubContainerCreatorCached(
                        _subContainerCreatorFactory(container));

                    RegisterProviderPerContract(
                        container,
                        (_, contractType) =>
                        new SubContainerDependencyProvider(
                            contractType, _subIdentifier, containerCreator, _resolveAll));
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }
    }
}

#endif
