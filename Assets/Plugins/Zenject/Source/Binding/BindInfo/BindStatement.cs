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
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class BindStatement : IDisposable
    {
        readonly List<IDisposable> _disposables;
        IBindingFinalizer _bindingFinalizer;

        public BindStatement()
        {
            _disposables = new List<IDisposable>();
            Reset();
        }

        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                AssertHasFinalizer();
                return _bindingFinalizer.BindingInheritanceMethod;
            }
        }

        public bool HasFinalizer
        {
            get { return _bindingFinalizer != null; }
        }

        public void SetFinalizer(IBindingFinalizer bindingFinalizer)
        {
            _bindingFinalizer = bindingFinalizer;
        }

        void AssertHasFinalizer()
        {
            if (_bindingFinalizer == null)
            {
                throw Assert.CreateException(
                    "Unfinished binding!  Some required information was left unspecified.");
            }
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public BindInfo SpawnBindInfo()
        {
            var bindInfo = ZenPools.SpawnBindInfo();
            AddDisposable(bindInfo);
            return bindInfo;
        }

        public void FinalizeBinding(DiContainer container)
        {
            AssertHasFinalizer();
            _bindingFinalizer.FinalizeBinding(container);
        }

        public void Reset()
        {
            _bindingFinalizer = null;

            for (int i = 0; i < _disposables.Count; i++)
            {
                _disposables[i].Dispose();
            }

            _disposables.Clear();
        }

        public void Dispose()
        {
            ZenPools.DespawnStatement(this);
        }
    }
}
