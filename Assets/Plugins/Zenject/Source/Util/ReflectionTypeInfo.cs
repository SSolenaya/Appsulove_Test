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
using System.Reflection;

namespace Zenject.Internal
{
    [NoReflectionBaking]
    public class ReflectionTypeInfo
    {
        public readonly Type Type;
        public readonly Type BaseType;
        public readonly List<InjectPropertyInfo> InjectProperties;
        public readonly List<InjectFieldInfo> InjectFields;
        public readonly InjectConstructorInfo InjectConstructor;
        public readonly List<InjectMethodInfo> InjectMethods;

        public ReflectionTypeInfo(
            Type type,
            Type baseType,
            InjectConstructorInfo injectConstructor,
            List<InjectMethodInfo> injectMethods,
            List<InjectFieldInfo> injectFields,
            List<InjectPropertyInfo> injectProperties)
        {
            Type = type;
            BaseType = baseType;
            InjectFields = injectFields;
            InjectConstructor = injectConstructor;
            InjectMethods = injectMethods;
            InjectProperties = injectProperties;
        }

        [NoReflectionBaking]
        public class InjectFieldInfo
        {
            public readonly FieldInfo FieldInfo;
            public readonly InjectableInfo InjectableInfo;

            public InjectFieldInfo(
                FieldInfo fieldInfo,
                InjectableInfo injectableInfo)
            {
                InjectableInfo = injectableInfo;
                FieldInfo = fieldInfo;
            }
        }

        [NoReflectionBaking]
        public class InjectParameterInfo
        {
            public readonly ParameterInfo ParameterInfo;
            public readonly InjectableInfo InjectableInfo;

            public InjectParameterInfo(
                ParameterInfo parameterInfo,
                InjectableInfo injectableInfo)
            {
                InjectableInfo = injectableInfo;
                ParameterInfo = parameterInfo;
            }
        }

        [NoReflectionBaking]
        public class InjectPropertyInfo
        {
            public readonly PropertyInfo PropertyInfo;
            public readonly InjectableInfo InjectableInfo;

            public InjectPropertyInfo(
                PropertyInfo propertyInfo,
                InjectableInfo injectableInfo)
            {
                InjectableInfo = injectableInfo;
                PropertyInfo = propertyInfo;
            }
        }

        [NoReflectionBaking]
        public class InjectMethodInfo
        {
            public readonly MethodInfo MethodInfo;
            public readonly List<InjectParameterInfo> Parameters;

            public InjectMethodInfo(
                MethodInfo methodInfo,
                List<InjectParameterInfo> parameters)
            {
                MethodInfo = methodInfo;
                Parameters = parameters;
            }
        }

        [NoReflectionBaking]
        public class InjectConstructorInfo
        {
            public readonly ConstructorInfo ConstructorInfo;
            public readonly List<InjectParameterInfo> Parameters;

            public InjectConstructorInfo(
                ConstructorInfo constructorInfo,
                List<InjectParameterInfo> parameters)
            {
                ConstructorInfo = constructorInfo;
                Parameters = parameters;
            }
        }
    }
}

