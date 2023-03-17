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
using System.Linq;

namespace Zenject
{
    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TContract> : FactoryToChoiceBinder<TContract>
    {
        public FactoryArgumentsToChoiceBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // We use generics instead of params object[] so that we preserve type info
        // So that you can for example pass in a variable that is null and the type info will
        // still be used to map null on to the correct field
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<T>(T param)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param1, param2);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3>(
            TParam1 param1, TParam2 param2, TParam3 param3)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param1, param2, param3);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param1, param2, param3, param4);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4, TParam5>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param1, param2, param3, param4, param5);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit(param1, param2, param3, param4, param5, param6);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArguments(object[] args)
        {
            FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        public FactoryToChoiceBinder<TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            FactoryBindInfo.Arguments = extraArgs.ToList();
            return this;
        }
    }
}

