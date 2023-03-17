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

using System.Linq;
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class PlaceholderFactoryBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        readonly FactoryBindInfo _factoryBindInfo;

        public PlaceholderFactoryBindingFinalizer(
            BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindInfo)
        {
            // Note that it doesn't derive from PlaceholderFactory<TContract>
            // when used with To<>, so we can only check IPlaceholderFactory
            Assert.That(factoryBindInfo.FactoryType.DerivesFrom<IPlaceholderFactory>());

            _factoryBindInfo = factoryBindInfo;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            var provider = _factoryBindInfo.ProviderFunc(container);

            var transientProvider = new TransientProvider(
                _factoryBindInfo.FactoryType,
                container,
                _factoryBindInfo.Arguments.Concat(
                    InjectUtil.CreateArgListExplicit(
                        provider,
                        new InjectContext(container, typeof(TContract)))).ToList(),
                BindInfo.ContextInfo, BindInfo.ConcreteIdentifier, null);

            IProvider mainProvider;

            if (BindInfo.Scope == ScopeTypes.Unset || BindInfo.Scope == ScopeTypes.Singleton)
            {
                mainProvider = BindingUtil.CreateCachedProvider(transientProvider);
            }
            else
            {
                Assert.IsEqual(BindInfo.Scope, ScopeTypes.Transient);
                mainProvider = transientProvider;
            }

            RegisterProviderForAllContracts(container, mainProvider);
        }
    }
}
