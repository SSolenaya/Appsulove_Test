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
    [NoReflectionBaking]
    public class ArgConditionCopyNonLazyBinder : InstantiateCallbackConditionCopyNonLazyBinder
    {
        public ArgConditionCopyNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        // We use generics instead of params object[] so that we preserve type info
        // So that you can for example pass in a variable that is null and the type info will
        // still be used to map null on to the correct field
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<T>(T param)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param1));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param2));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3>(
            TParam1 param1, TParam2 param2, TParam3 param3)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param1));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param2));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param3));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param1));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param2));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param3));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param4));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param1));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param2));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param3));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param4));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param5));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            BindInfo.Arguments.Clear();
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param1));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param2));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param3));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param4));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param5));
            BindInfo.Arguments.Add(InjectUtil.CreateTypePair(param6));
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments(object[] args)
        {
            BindInfo.Arguments.Clear();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                Assert.IsNotNull(arg,
                    "Cannot include null values when creating a zenject argument list because zenject has no way of deducing the type from a null value.  If you want to allow null, use the Explicit form.");

                BindInfo.Arguments.Add(
                    new TypeValuePair(arg.GetType(), arg));
            }
            return this;
        }

        public InstantiateCallbackConditionCopyNonLazyBinder WithArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            BindInfo.Arguments.Clear();

            foreach (var arg in extraArgs)
            {
                BindInfo.Arguments.Add(arg);
            }
            return this;
        }
    }
}
