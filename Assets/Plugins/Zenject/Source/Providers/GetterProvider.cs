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
    [NoReflectionBaking]
    public class GetterProvider<TObj, TResult> : IProvider
    {
        readonly DiContainer _container;
        readonly object _identifier;
        readonly Func<TObj, TResult> _method;
        readonly bool _matchAll;
        readonly InjectSources _sourceType;

        public GetterProvider(
            object identifier, Func<TObj, TResult> method,
            DiContainer container, InjectSources sourceType, bool matchAll)
        {
            _container = container;
            _identifier = identifier;
            _method = method;
            _matchAll = matchAll;
            _sourceType = sourceType;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TResult);
        }

        InjectContext GetSubContext(InjectContext parent)
        {
            var subContext = parent.CreateSubContext(
                typeof(TObj), _identifier);

            subContext.Optional = false;
            subContext.SourceType = _sourceType;

            return subContext;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(typeof(TResult).DerivesFromOrEqual(context.MemberType));

            injectAction = null;

            if (_container.IsValidating)
            {
                // All we can do is validate that the getter object can be resolved
                if (_matchAll)
                {
                    _container.ResolveAll(GetSubContext(context));
                }
                else
                {
                    _container.Resolve(GetSubContext(context));
                }

                buffer.Add(new ValidationMarker(typeof(TResult)));
                return;
            }

            if (_matchAll)
            {
                Assert.That(buffer.Count == 0);
                _container.ResolveAll(GetSubContext(context), buffer);

                for (int i = 0; i < buffer.Count; i++)
                {
                    buffer[i] = _method((TObj)buffer[i]);
                }
            }
            else
            {
                buffer.Add(_method(
                    (TObj)_container.Resolve(GetSubContext(context))));
            }
        }
    }
}
