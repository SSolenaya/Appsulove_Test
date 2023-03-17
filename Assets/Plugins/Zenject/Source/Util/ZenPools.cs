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

namespace Zenject.Internal
{
    public static class ZenPools
    {
#if ZEN_INTERNAL_NO_POOLS
        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            return new InjectContext(container, memberType);
        }

        public static void DespawnInjectContext(InjectContext context)
        {
        }

        public static List<T> SpawnList<T>()
        {
            return new List<T>();
        }

        public static void DespawnList<T>(List<T> list)
        {
        }

        public static void DespawnArray<T>(T[] arr)
        {
        }

        public static T[] SpawnArray<T>(int length)
        {
            return new T[length];
        }

        public static HashSet<T> SpawnHashSet<T>()
        {
            return new HashSet<T>();
        }

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
        }

        public static void DespawnHashSet<T>(HashSet<T> set)
        {
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            return new LookupId(provider, bindingId);
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
        }

        public static BindInfo SpawnBindInfo()
        {
            return new BindInfo();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
        }

        public static BindStatement SpawnStatement()
        {
            return new BindStatement();
        }

        public static void DespawnStatement(BindStatement statement)
        {
        }
#else
        static readonly StaticMemoryPool<InjectContext> _contextPool = new StaticMemoryPool<InjectContext>();
        static readonly StaticMemoryPool<LookupId> _lookupIdPool = new StaticMemoryPool<LookupId>();
        static readonly StaticMemoryPool<BindInfo> _bindInfoPool = new StaticMemoryPool<BindInfo>();
        static readonly StaticMemoryPool<BindStatement> _bindStatementPool = new StaticMemoryPool<BindStatement>();

        public static HashSet<T> SpawnHashSet<T>()
        {
            return HashSetPool<T>.Instance.Spawn();
        }

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return DictionaryPool<TKey, TValue>.Instance.Spawn();
        }

        public static BindStatement SpawnStatement()
        {
            return _bindStatementPool.Spawn();
        }

        public static void DespawnStatement(BindStatement statement)
        {
            statement.Reset();
            _bindStatementPool.Despawn(statement);
        }

        public static BindInfo SpawnBindInfo()
        {
            return _bindInfoPool.Spawn();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
            bindInfo.Reset();
            _bindInfoPool.Despawn(bindInfo);
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            DictionaryPool<TKey, TValue>.Instance.Despawn(dictionary);
        }

        public static void DespawnHashSet<T>(HashSet<T> set)
        {
            HashSetPool<T>.Instance.Despawn(set);
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            var lookupId = _lookupIdPool.Spawn();

            lookupId.Provider = provider;
            lookupId.BindingId = bindingId;

            return lookupId;
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
            _lookupIdPool.Despawn(lookupId);
        }

        public static List<T> SpawnList<T>()
        {
            return ListPool<T>.Instance.Spawn();
        }

        public static void DespawnList<T>(List<T> list)
        {
            ListPool<T>.Instance.Despawn(list);
        }

        public static void DespawnArray<T>(T[] arr)
        {
            ArrayPool<T>.GetPool(arr.Length).Despawn(arr);
        }

        public static T[] SpawnArray<T>(int length)
        {
            return ArrayPool<T>.GetPool(length).Spawn();
        }

        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            var context = _contextPool.Spawn();

            context.Container = container;
            context.MemberType = memberType;

            return context;
        }

        public static void DespawnInjectContext(InjectContext context)
        {
            context.Reset();
            _contextPool.Despawn(context);
        }
#endif

        public static InjectContext SpawnInjectContext(
            DiContainer container, InjectableInfo injectableInfo, InjectContext currentContext,
            object targetInstance, Type targetType, object concreteIdentifier)
        {
            var context = SpawnInjectContext(container, injectableInfo.MemberType);

            context.ObjectType = targetType;
            context.ParentContext = currentContext;
            context.ObjectInstance = targetInstance;
            context.Identifier = injectableInfo.Identifier;
            context.MemberName = injectableInfo.MemberName;
            context.Optional = injectableInfo.Optional;
            context.SourceType = injectableInfo.SourceType;
            context.FallBackValue = injectableInfo.DefaultValue;
            context.ConcreteIdentifier = concreteIdentifier;

            return context;
        }
    }
}