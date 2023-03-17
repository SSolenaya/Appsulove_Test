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

namespace Zenject.Internal
{
    public interface IDecoratorProvider
    {
        void GetAllInstances(
            IProvider provider, InjectContext context, List<object> buffer);
    }

    [NoReflectionBaking]
    public class DecoratorProvider<TContract> : IDecoratorProvider
    {
        readonly Dictionary<IProvider, List<object>> _cachedInstances =
            new Dictionary<IProvider, List<object>>();

        readonly DiContainer _container;
        readonly List<Guid> _factoryBindIds = new List<Guid>();

        List<IFactory<TContract, TContract>> _decoratorFactories;

#if ZEN_MULTITHREADING
        readonly object _locker = new object();
#endif

        public DecoratorProvider(DiContainer container)
        {
            _container = container;
        }

        public void AddFactoryId(Guid factoryBindId)
        {
            _factoryBindIds.Add(factoryBindId);
        }

        void LazyInitializeDecoratorFactories()
        {
            if (_decoratorFactories == null)
            {
                _decoratorFactories = new List<IFactory<TContract, TContract>>();

                for (int i = 0; i < _factoryBindIds.Count; i++)
                {
                    var bindId = _factoryBindIds[i];
                    var factory = _container.ResolveId<IFactory<TContract, TContract>>(bindId);
                    _decoratorFactories.Add(factory);
                }
            }
        }

        public void GetAllInstances(
            IProvider provider, InjectContext context, List<object> buffer)
        {
            if (provider.IsCached)
            {
                List<object> instances;

#if ZEN_MULTITHREADING
                lock (_locker)
#endif
                {
                    if (!_cachedInstances.TryGetValue(provider, out instances))
                    {
                        instances = new List<object>();
                        WrapProviderInstances(provider, context, instances);
                        _cachedInstances.Add(provider, instances);
                    }
                }

                buffer.AllocFreeAddRange(instances);
            }
            else
            {
                WrapProviderInstances(provider, context, buffer);
            }
        }

        void WrapProviderInstances(IProvider provider, InjectContext context, List<object> buffer)
        {
            LazyInitializeDecoratorFactories();

            provider.GetAllInstances(context, buffer);

            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i] = DecorateInstance(buffer[i], context);
            }
        }

        object DecorateInstance(object instance, InjectContext context)
        {
            for (int i = 0; i < _decoratorFactories.Count; i++)
            {
                instance = _decoratorFactories[i].Create(
                    context.Container.IsValidating ? default(TContract) : (TContract)instance);
            }

            return instance;
        }
    }
}
