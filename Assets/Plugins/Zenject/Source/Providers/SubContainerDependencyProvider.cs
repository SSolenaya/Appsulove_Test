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
    public class SubContainerDependencyProvider : IProvider
    {
        readonly ISubContainerCreator _subContainerCreator;
        readonly Type _dependencyType;
        readonly object _identifier;
        readonly bool _resolveAll;

        // if concreteType is null we use the contract type from inject context
        public SubContainerDependencyProvider(
            Type dependencyType,
            object identifier,
            ISubContainerCreator subContainerCreator, bool resolveAll)
        {
            _subContainerCreator = subContainerCreator;
            _dependencyType = dependencyType;
            _identifier = identifier;
            _resolveAll = resolveAll;
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
            return _dependencyType;
        }

        InjectContext CreateSubContext(
            InjectContext parent, DiContainer subContainer)
        {
            var subContext = parent.CreateSubContext(_dependencyType, _identifier);

            subContext.Container = subContainer;

            // This is important to avoid infinite loops
            subContext.SourceType = InjectSources.Local;

            return subContext;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsNotNull(context);

            var subContainer = _subContainerCreator.CreateSubContainer(args, context, out injectAction);

            var subContext = CreateSubContext(context, subContainer);

            if (_resolveAll)
            {
                subContainer.ResolveAll(subContext, buffer);
                return;
            }

            buffer.Add(subContainer.Resolve(subContext));
        }
    }
}
