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
using System.IO;
using System.Reflection;
using Zenject.ReflectionBaking.Mono.Cecil;

namespace Zenject.ReflectionBaking
{
    public class UnityAssemblyResolver : BaseAssemblyResolver
    {
        readonly IDictionary<string, string> _appDomainAssemblyLocations;
        readonly IDictionary<string, AssemblyDefinition> _cache;

        public UnityAssemblyResolver()
        {
            _appDomainAssemblyLocations = new Dictionary<string, string>();
            _cache = new Dictionary<string, AssemblyDefinition>();

            AppDomain domain = AppDomain.CurrentDomain;

            Assembly[] assemblies = domain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
#if NET_4_6
                if (assemblies[i].IsDynamic)
                {
                    continue;
                }
#endif

                _appDomainAssemblyLocations[assemblies[i].FullName] = assemblies[i].Location;

                AddSearchDirectory(Path.GetDirectoryName(assemblies[i].Location));
            }
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            AssemblyDefinition assemblyDef = FindAssemblyDefinition(name.FullName, null);

            if (assemblyDef == null)
            {
                assemblyDef = base.Resolve(name);
                _cache[name.FullName] = assemblyDef;
            }

            return assemblyDef;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            AssemblyDefinition assemblyDef = FindAssemblyDefinition(name.FullName, parameters);

            if (assemblyDef == null)
            {
                assemblyDef = base.Resolve(name, parameters);
                _cache[name.FullName] = assemblyDef;
            }

            return assemblyDef;
        }

        /// Searches for AssemblyDefinition in our cache, and failing that,
        /// looks for a known location.  Returns null if both attempts fail.
        AssemblyDefinition FindAssemblyDefinition(string fullName, ReaderParameters parameters)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName");
            }

            AssemblyDefinition assemblyDefinition;

            // Look in cache first
            if (_cache.TryGetValue(fullName, out assemblyDefinition))
            {
                return assemblyDefinition;
            }

            // Try to use known location

            string location;

            if (_appDomainAssemblyLocations.TryGetValue(fullName, out location))
            {
                if (parameters != null)
                {
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(location, parameters);
                }
                else
                {
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(location);
                }

                _cache[fullName] = assemblyDefinition;

                return assemblyDefinition;
            }

            return null;
        }
    }
}
