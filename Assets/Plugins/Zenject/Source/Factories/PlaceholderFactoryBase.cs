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
using System.Linq;
using ModestTree;

namespace Zenject
{
    public interface IPlaceholderFactory : IValidatable
    {
    }

    // Placeholder factories can be used to choose a creation method in an installer, using FactoryBinder
    public abstract class PlaceholderFactoryBase<TValue> : IPlaceholderFactory
    {
        IProvider _provider;
        InjectContext _injectContext;

        [Inject]
        void Construct(IProvider provider, InjectContext injectContext)
        {
            Assert.IsNotNull(provider);
            Assert.IsNotNull(injectContext);

            _provider = provider;
            _injectContext = injectContext;
        }

        protected TValue CreateInternal(List<TypeValuePair> extraArgs)
        {
            try
            {
                var result = _provider.GetInstance(_injectContext, extraArgs);

                if (_injectContext.Container.IsValidating && result is ValidationMarker)
                {
                    return default(TValue);
                }

                Assert.That(result == null || result.GetType().DerivesFromOrEqual<TValue>());

                return (TValue) result;
            }
            catch (Exception e)
            {
                throw new ZenjectException(
                    "Error during construction of type '{0}' via {1}.Create method!".Fmt(typeof(TValue), GetType()), e);
            }
        }

        public virtual void Validate()
        {
            _provider.GetInstance(
                _injectContext, ValidationUtil.CreateDefaultArgs(ParamTypes.ToArray()));
        }

        protected abstract IEnumerable<Type> ParamTypes
        {
            get;
        }
    }
}
