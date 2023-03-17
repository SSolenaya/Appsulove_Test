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
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    [NoReflectionBaking]
    public class FromBinderGeneric<TContract> : FromBinder
    {
        public FromBinderGeneric(
            DiContainer bindContainer,
            BindInfo bindInfo,
            BindStatement bindStatement)
            : base(bindContainer, bindInfo, bindStatement)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), BindInfo.ContractTypes);
        }

        // Shortcut for FromIFactory and also for backwards compatibility
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromFactory<TFactory>()
            where TFactory : IFactory<TContract>
        {
            return FromIFactory(x => x.To<TFactory>().AsCached());
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactory(
            Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            return FromIFactoryBase<TContract>(factoryBindGenerator);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod(Func<TContract> method)
        {
            return FromMethodBase<TContract>(ctx => method());
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod(Func<InjectContext, TContract> method)
        {
            return FromMethodBase<TContract>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultiple(Func<InjectContext, IEnumerable<TContract>> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), AllParentTypes);
            return FromMethodMultipleBase<TContract>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, false);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, true);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstance(TContract instance)
        {
            return FromInstanceBase(instance);
        }

#if !NOT_UNITY3D

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(
            Func<TContract, bool> predicate, bool includeInactive = true)
        {
            return FromComponentsInChildren(false, predicate, includeInactive);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(
            bool excludeSelf = false, Func<TContract, bool> predicate = null, bool includeInactive = true)
        {
            Func<Component, bool> subPredicate;

            if (predicate != null)
            {
                subPredicate = component => predicate((TContract)(object)component);
            }
            else
            {
                subPredicate = null;
            }

            return FromComponentsInChildrenBase(
                excludeSelf, subPredicate, includeInactive);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInHierarchy(
            Func<TContract, bool> predicate = null, bool includeInactive = true)
        {
            Func<Component, bool> subPredicate;

            if (predicate != null)
            {
                subPredicate = component => predicate((TContract)(object)component);
            }
            else
            {
                subPredicate = null;
            }

            return FromComponentsInHierarchyBase(subPredicate, includeInactive);
        }
#endif
    }
}
