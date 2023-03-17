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
    public class SignalCopyBinder
    {
        readonly List<BindInfo> _bindInfos;

        public SignalCopyBinder()
        {
            _bindInfos = new List<BindInfo>();
        }

        public SignalCopyBinder(BindInfo bindInfo)
        {
            _bindInfos = new List<BindInfo>
            {
                bindInfo
            };
        }

        // This is used in cases where you have multiple bindings that depend on each other so should
        // be inherited together
        public void AddCopyBindInfo(BindInfo bindInfo)
        {
            _bindInfos.Add(bindInfo);
        }

        public void CopyIntoAllSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.CopyIntoAll);
        }

        // Only copy the binding into children and not grandchildren
        public void CopyIntoDirectSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.CopyDirectOnly);
        }

        // Do not apply the binding on the current container
        public void MoveIntoAllSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.MoveIntoAll);
        }

        // Do not apply the binding on the current container
        public void MoveIntoDirectSubContainers()
        {
            SetInheritanceMethod(BindingInheritanceMethods.MoveDirectOnly);
        }

        void SetInheritanceMethod(BindingInheritanceMethods method)
        {
            for (int i = 0; i < _bindInfos.Count; i++)
            {
                _bindInfos[i].BindingInheritanceMethod = method;
            }
        }
    }
}
