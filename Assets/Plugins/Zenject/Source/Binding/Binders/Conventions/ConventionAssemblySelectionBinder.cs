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

#if !(UNITY_WSA && ENABLE_DOTNET)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zenject
{
    [NoReflectionBaking]
    public class ConventionAssemblySelectionBinder
    {
        public ConventionAssemblySelectionBinder(ConventionBindInfo bindInfo)
        {
            BindInfo = bindInfo;
        }

        protected ConventionBindInfo BindInfo
        {
            get;
            private set;
        }

        public void FromAllAssemblies()
        {
            // Do nothing
            // This is the default
        }

        public void FromAssemblyContaining<T>()
        {
            FromAssembliesContaining(typeof(T));
        }

        public void FromAssembliesContaining(params Type[] types)
        {
            FromAssembliesContaining((IEnumerable<Type>)types);
        }

        public void FromAssembliesContaining(IEnumerable<Type> types)
        {
            FromAssemblies(types.Select(t => t.Assembly).Distinct());
        }

        public void FromThisAssembly()
        {
            FromAssemblies(Assembly.GetCallingAssembly());
        }

        public void FromAssembly(Assembly assembly)
        {
            FromAssemblies(assembly);
        }

        public void FromAssemblies(params Assembly[] assemblies)
        {
            FromAssemblies((IEnumerable<Assembly>)assemblies);
        }

        public void FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            BindInfo.AddAssemblyFilter(assembly => assemblies.Contains(assembly));
        }

        public void FromAssembliesWhere(Func<Assembly, bool> predicate)
        {
            BindInfo.AddAssemblyFilter(predicate);
        }
    }
}

#endif
