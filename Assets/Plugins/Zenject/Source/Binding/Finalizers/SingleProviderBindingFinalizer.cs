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
    public class SingleProviderBindingFinalizer : ProviderBindingFinalizer
    {
        readonly Func<DiContainer, Type, IProvider> _providerFactory;

        public SingleProviderBindingFinalizer(
            BindInfo bindInfo, Func<DiContainer, Type, IProvider> providerFactory)
            : base(bindInfo)
        {
            _providerFactory = providerFactory;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (BindInfo.ToChoice == ToChoices.Self)
            {
                Assert.IsEmpty(BindInfo.ToTypes);

                RegisterProviderPerContract(container, _providerFactory);
            }
            else
            {
                // Empty sometimes when using convention based bindings
                if (!BindInfo.ToTypes.IsEmpty())
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container, BindInfo.ToTypes, _providerFactory);
                }
            }
        }
    }
}
