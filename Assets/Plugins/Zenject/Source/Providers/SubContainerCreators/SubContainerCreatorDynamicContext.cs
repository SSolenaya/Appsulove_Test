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

#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using UnityEngine;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public abstract class SubContainerCreatorDynamicContext : ISubContainerCreator
    {
        readonly DiContainer _container;

        public SubContainerCreatorDynamicContext(DiContainer container)
        {
            _container = container;
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        public DiContainer CreateSubContainer(
            List<TypeValuePair> args, InjectContext parentContext, out Action injectAction)
        {
            bool shouldMakeActive;
            var gameObj = CreateGameObject(out shouldMakeActive);

            var context = gameObj.AddComponent<GameObjectContext>();

            AddInstallers(args, context);

            context.Install(_container);

            injectAction = () => 
            {
                // Note: We don't need to call ResolveRoots here because GameObjectContext does this for us
                _container.Inject(context);

                if (shouldMakeActive && !_container.IsValidating)
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                    {
                        gameObj.SetActive(true);
                    }
                }
            };

            return context.Container;
        }

        protected abstract void AddInstallers(List<TypeValuePair> args, GameObjectContext context);
        protected abstract GameObject CreateGameObject(out bool shouldMakeActive);
    }
}

#endif
