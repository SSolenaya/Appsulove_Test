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
    [NoReflectionBaking]
    public class CachedOpenTypeProvider : IProvider
    {
        readonly IProvider _creator;
        readonly Dictionary<Type, CachedProvider> _providerMap = new Dictionary<Type, CachedProvider>();

#if ZEN_MULTITHREADING
        readonly object _locker = new object();
#endif

        public CachedOpenTypeProvider(IProvider creator)
        {
            Assert.That(creator.TypeVariesBasedOnMemberType);
            _creator = creator;
        }

        public bool IsCached
        {
            get { return true; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                // Should not call this
                throw Assert.CreateException();
            }
        }

        public int NumInstances
        {
            get
            {
#if ZEN_MULTITHREADING
                lock (_locker)
#endif
                {
                    return _providerMap.Values.Select(x => x.NumInstances).Sum();
                }
            }
        }

        // This method can be called if you want to clear the memory for an AsSingle instance,
        // See isssue https://github.com/svermeulen/Zenject/issues/441
        public void ClearCache()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                _providerMap.Clear();
            }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _creator.GetInstanceType(context);
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsNotNull(context);

            CachedProvider provider;

#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                if (!_providerMap.TryGetValue(context.MemberType, out provider))
                {
                    provider = new CachedProvider(_creator);
                    _providerMap.Add(context.MemberType, provider);
                }
            }

            provider.GetAllInstancesWithInjectSplit(
                context, args, out injectAction, buffer);
        }
    }
}

