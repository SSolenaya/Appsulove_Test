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

using System.Collections.Generic;

namespace Zenject
{
    [NoReflectionBaking]
    public class CopyNonLazyBinder : NonLazyBinder
    {
        List<BindInfo> _secondaryBindInfos;

        public CopyNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        // This is used in cases where you have multiple bindings that depend on each other so should
        // be inherited together (eg. FromIFactory)
        internal void AddSecondaryCopyBindInfo(BindInfo bindInfo)
        {
            if (_secondaryBindInfos == null)
            {
                _secondaryBindInfos = new List<BindInfo>();
            }
            _secondaryBindInfos.Add(bindInfo);
        }

        public NonLazyBinder CopyIntoAllSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.CopyIntoAll);
            return this;
        }

        // Only copy the binding into children and not grandchildren
        public NonLazyBinder CopyIntoDirectSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.CopyDirectOnly);
            return this;
        }

        // Do not apply the binding on the current container
        public NonLazyBinder MoveIntoAllSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.MoveIntoAll);
            return this;
        }

        // Do not apply the binding on the current container
        public NonLazyBinder MoveIntoDirectSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.MoveDirectOnly);
            return this;
        }

        void SetInheritanceMethod(BindingInheritanceMethods method)
        {
            BindInfo.BindingInheritanceMethod = method;

            if (_secondaryBindInfos != null)
            {
                foreach (var secondaryBindInfo in _secondaryBindInfos)
                {
                    secondaryBindInfo.BindingInheritanceMethod = method;
                }
            }
        }
    }
}
