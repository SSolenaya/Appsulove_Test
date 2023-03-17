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

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    [NoReflectionBaking]
    public class FromBinderNonGeneric : FromBinder
    {
        public FromBinderNonGeneric(
            DiContainer bindContainer, BindInfo bindInfo,
            BindStatement bindStatement)
            : base(bindContainer, bindInfo, bindStatement)
        {
        }

        // Shortcut for FromIFactory and also for backwards compatibility
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromFactory<TConcrete, TFactory>()
            where TFactory : IFactory<TConcrete>
        {
            return FromIFactory<TConcrete>(x => x.To<TFactory>().AsCached());
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactory<TContract>(
            Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            return FromIFactoryBase<TContract>(factoryBindGenerator);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            return FromMethodBase<TConcrete>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultiple<TConcrete>(Func<InjectContext, IEnumerable<TConcrete>> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), AllParentTypes);
            return FromMethodMultipleBase<TConcrete>(method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj, TContract>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, false);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj, TContract>(null, method);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveAllGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method, source, true);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstance(object instance)
        {
            return FromInstanceBase(instance);
        }

#if !NOT_UNITY3D

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(
            Func<Component, bool> predicate, bool includeInactive = true)
        {
            return FromComponentsInChildren(false, predicate, includeInactive);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(
            bool excludeSelf = false, Func<Component, bool> predicate = null, bool includeInactive = true)
        {
            return FromComponentsInChildrenBase(excludeSelf, predicate, includeInactive);
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInHierarchy(
            Func<Component, bool> predicate = null, bool includeInactive = true)
        {
            return FromComponentsInHierarchyBase(predicate, includeInactive);
        }
#endif
    }
}
