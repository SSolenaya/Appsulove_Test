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
    public class SubContainerCreatorCached : ISubContainerCreator
    {
        readonly ISubContainerCreator _subCreator;

#if ZEN_MULTITHREADING
        readonly object _locker = new object();
#else
        bool _isLookingUp;
#endif
        DiContainer _subContainer;

        public SubContainerCreatorCached(ISubContainerCreator subCreator)
        {
            _subCreator = subCreator;
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context, out Action injectAction)
        {
            // We can't really support arguments if we are using the cached value since
            // the arguments might change when called after the first time
            Assert.IsEmpty(args);

#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                if (_subContainer == null)
                {
#if !ZEN_MULTITHREADING
                    Assert.That(!_isLookingUp,
                        "Found unresolvable circular dependency when looking up sub container!  Object graph:\n {0}", context.GetObjectGraphString());
                    _isLookingUp = true;
#endif

                    _subContainer = _subCreator.CreateSubContainer(
                            new List<TypeValuePair>(), context, out injectAction);

#if !ZEN_MULTITHREADING
                    _isLookingUp = false;
#endif

                    Assert.IsNotNull(_subContainer);
                }
                else 
                {
                    injectAction = null;
                }

                return _subContainer;
            }
        }
    }
}
