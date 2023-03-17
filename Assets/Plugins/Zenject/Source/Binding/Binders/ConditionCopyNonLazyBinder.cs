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
using System.Linq;
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class ConditionCopyNonLazyBinder : CopyNonLazyBinder
    {
        public ConditionCopyNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public CopyNonLazyBinder When(BindingCondition condition)
        {
            BindInfo.Condition = condition;
            return this;
        }

        public CopyNonLazyBinder WhenInjectedIntoInstance(object instance)
        {
            return When(r => ReferenceEquals(r.ObjectInstance, instance));
        }

        public CopyNonLazyBinder WhenInjectedInto(params Type[] targets)
        {
            return When(r => targets.Where(x => r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(x)).Any());
        }

        public CopyNonLazyBinder WhenInjectedInto<T>()
        {
            return When(r => r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(typeof(T)));
        }

        public CopyNonLazyBinder WhenNotInjectedInto<T>()
        {
            return When(r => r.ObjectType == null || !r.ObjectType.DerivesFromOrEqual(typeof(T)));
        }
    }
}
