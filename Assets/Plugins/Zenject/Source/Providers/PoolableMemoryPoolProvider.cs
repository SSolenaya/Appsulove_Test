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

namespace Zenject
{
    public abstract class PoolableMemoryPoolProviderBase<TContract> : IProvider
    {
        public PoolableMemoryPoolProviderBase(
            DiContainer container, Guid poolId)
        {
            Container = container;
            PoolId = poolId;
        }

        public bool IsCached
        {
            get { return false; }
        }

        protected Guid PoolId
        {
            get;
            private set;
        }

        protected DiContainer Container
        {
            get;
            private set;
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TContract);
        }

        public abstract void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer);
    }

    // Zero parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<IMemoryPool>
        where TMemoryPool : MemoryPool<IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.That(args.IsEmpty());

            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(_pool));
        }
    }

    // One parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 1);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn((TParam1)args[0].Value, _pool));
        }
    }

    // Two parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, TParam2, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, TParam2, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 2);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(
                (TParam1)args[0].Value,
                (TParam2)args[1].Value,
                _pool));
        }
    }

    // Three parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 3);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(
                (TParam1)args[0].Value,
                (TParam2)args[1].Value,
                (TParam3)args[2].Value,
                _pool));
        }
    }

    // Four parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 4);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(
                (TParam1)args[0].Value,
                (TParam2)args[1].Value,
                (TParam3)args[2].Value,
                (TParam4)args[3].Value,
                _pool));
        }
    }

    // Five parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 5);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(
                (TParam1)args[0].Value,
                (TParam2)args[1].Value,
                (TParam3)args[2].Value,
                (TParam4)args[3].Value,
                (TParam5)args[4].Value,
                _pool));
        }
    }

    // Six parameters

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable
        where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool>
        where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool, TContract>
    {
        TMemoryPool _pool;

        public PoolableMemoryPoolProvider(
            DiContainer container, Guid poolId)
            : base(container, poolId)
        {
        }

        public void Validate()
        {
            Container.ResolveId<TMemoryPool>(PoolId);
        }

        public override void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEqual(args.Count, 6);
            Assert.IsNotNull(context);

            Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());

            injectAction = null;

            if (_pool == null)
            {
                _pool = Container.ResolveId<TMemoryPool>(PoolId);
            }

            buffer.Add(_pool.Spawn(
                (TParam1)args[0].Value,
                (TParam2)args[1].Value,
                (TParam3)args[2].Value,
                (TParam4)args[3].Value,
                (TParam5)args[4].Value,
                (TParam6)args[5].Value,
                _pool));
        }
    }
}

