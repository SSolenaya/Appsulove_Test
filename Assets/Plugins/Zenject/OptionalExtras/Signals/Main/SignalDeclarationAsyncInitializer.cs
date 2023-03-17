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
using ModestTree;

namespace Zenject
{
    // This class just exists to solve a circular dependency that would otherwise happen if we
    // attempted to inject TickableManager into either SignalDeclaration or SignalBus
    // And we need to directly depend on TickableManager because we need each SignalDeclaration
    // to have a unique tick priority
    public class SignalDeclarationAsyncInitializer : IInitializable
    {
        readonly LazyInject<TickableManager> _tickManager;
        readonly List<SignalDeclaration> _declarations;

        public SignalDeclarationAsyncInitializer(
            [Inject(Source = InjectSources.Local)]
            List<SignalDeclaration> declarations,
            [Inject(Optional = true, Source = InjectSources.Local)]
            LazyInject<TickableManager> tickManager)
        {
            _declarations = declarations;
            _tickManager = tickManager;
        }

        public void Initialize()
        {
            for (int i = 0; i < _declarations.Count; i++)
            {
                var declaration = _declarations[i];

                if (declaration.IsAsync)
                {
                    Assert.IsNotNull(_tickManager.Value, "TickableManager is required when using asynchronous signals");
                    _tickManager.Value.Add(declaration, declaration.TickPriority);
                }
            }
        }
    }
}

