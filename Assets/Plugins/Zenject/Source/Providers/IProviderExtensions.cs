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
    public static class IProviderExtensions
    {
        static readonly List<TypeValuePair> EmptyArgList = new List<TypeValuePair>();

        public static void GetAllInstancesWithInjectSplit(
            this IProvider creator, InjectContext context, out Action injectAction, List<object> buffer)
        {
            creator.GetAllInstancesWithInjectSplit(
                context, EmptyArgList, out injectAction, buffer);
        }

        public static void GetAllInstances(
            this IProvider creator, InjectContext context, List<object> buffer)
        {
            creator.GetAllInstances(context, EmptyArgList, buffer);
        }

        public static void GetAllInstances(
            this IProvider creator, InjectContext context, List<TypeValuePair> args, List<object> buffer)
        {
            Assert.IsNotNull(context);

            Action injectAction;
            creator.GetAllInstancesWithInjectSplit(context, args, out injectAction, buffer);

            if (injectAction != null)
            {
                injectAction.Invoke();
            }
        }

        public static object TryGetInstance(
            this IProvider creator, InjectContext context)
        {
            return creator.TryGetInstance(context, EmptyArgList);
        }

        public static object TryGetInstance(
            this IProvider creator, InjectContext context, List<TypeValuePair> args)
        {
            var allInstances = ZenPools.SpawnList<object>();

            try
            {
                creator.GetAllInstances(context, args, allInstances);

                if (allInstances.Count == 0)
                {
                    return null;
                }

                Assert.That(allInstances.Count == 1,
                    "Provider returned multiple instances when one or zero was expected");

                return allInstances[0];
            }
            finally
            {
                ZenPools.DespawnList(allInstances);
            }
        }

        public static object GetInstance(
            this IProvider creator, InjectContext context)
        {
            return creator.GetInstance(context, EmptyArgList);
        }

        public static object GetInstance(
            this IProvider creator, InjectContext context, List<TypeValuePair> args)
        {
            var allInstances = ZenPools.SpawnList<object>();

            try
            {
                creator.GetAllInstances(context, args, allInstances);

                Assert.That(allInstances.Count > 0,
                    "Provider returned zero instances when one was expected when looking up type '{0}'", context.MemberType);

                Assert.That(allInstances.Count == 1,
                    "Provider returned multiple instances when only one was expected when looking up type '{0}'", context.MemberType);

                return allInstances[0];
            }
            finally
            {
                ZenPools.DespawnList(allInstances);
            }
        }
    }
}
