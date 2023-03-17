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
using ModestTree.Util;

namespace Zenject
{
    // Responsibilities:
    // - Run Initialize() on all Iinitializable's, in the order specified by InitPriority
    public class InitializableManager
    {
        List<InitializableInfo> _initializables;

        bool _hasInitialized;

        [Inject]
        public InitializableManager(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<IInitializable> initializables,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ValuePair<Type, int>> priorities)
        {
            _initializables = new List<InitializableInfo>();

            for (int i = 0; i < initializables.Count; i++)
            {
                var initializable = initializables[i];

                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = priorities.Where(x => initializable.GetType().DerivesFromOrEqual(x.First)).Select(x => x.Second).ToList();
                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _initializables.Add(new InitializableInfo(initializable, priority));
            }
        }

        public void Add(IInitializable initializable)
        {
            Add(initializable, 0);
        }

        public void Add(IInitializable initializable, int priority)
        {
            Assert.That(!_hasInitialized);
            _initializables.Add(
                new InitializableInfo(initializable, priority));
        }

        public void Initialize()
        {
            Assert.That(!_hasInitialized);
            _hasInitialized = true;

            _initializables = _initializables.OrderBy(x => x.Priority).ToList();

#if UNITY_EDITOR
            foreach (var initializable in _initializables.Select(x => x.Initializable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate IInitializable with type '{0}'".Fmt(initializable.GetType()));
            }
#endif

            foreach (var initializable in _initializables)
            {
                try
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
#if UNITY_EDITOR
                    using (ProfileBlock.Start("{0}.Initialize()", initializable.Initializable.GetType()))
#endif
                    {
                        initializable.Initializable.Initialize();
                    }
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while initializing IInitializable with type '{0}'", initializable.Initializable.GetType());
                }
            }
        }

        class InitializableInfo
        {
            public IInitializable Initializable;
            public int Priority;

            public InitializableInfo(IInitializable initializable, int priority)
            {
                Initializable = initializable;
                Priority = priority;
            }
        }
    }
}
