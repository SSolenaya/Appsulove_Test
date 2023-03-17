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
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Responsibilities:
    // - Output a file specifying the full object graph for a given root dependency
    // - This file uses the DOT language with can be fed into GraphViz to generate an image
    // - http://www.graphviz.org/
    public static class ObjectGraphVisualizer
    {
        public static void OutputObjectGraphToFile(
            DiContainer container, string outputPath,
            IEnumerable<Type> externalIgnoreTypes, IEnumerable<Type> contractTypes)
        {
            // Output the entire object graph to file
            var graph = CalculateObjectGraph(container, contractTypes);

            var ignoreTypes = new List<Type>
            {
                typeof(DiContainer),
                typeof(InitializableManager)
            };

            ignoreTypes.AddRange(externalIgnoreTypes);

            var resultStr = "digraph { \n";

            resultStr += "rankdir=LR;\n";

            foreach (var entry in graph)
            {
                if (ShouldIgnoreType(entry.Key, ignoreTypes))
                {
                    continue;
                }

                foreach (var dependencyType in entry.Value)
                {
                    if (ShouldIgnoreType(dependencyType, ignoreTypes))
                    {
                        continue;
                    }

                    resultStr += GetFormattedTypeName(entry.Key) + " -> " + GetFormattedTypeName(dependencyType) + "; \n";
                }
            }

            resultStr += " }";

            File.WriteAllText(outputPath, resultStr);
        }

        static bool ShouldIgnoreType(Type type, List<Type> ignoreTypes)
        {
            return ignoreTypes.Contains(type);
        }

        static Dictionary<Type, List<Type>> CalculateObjectGraph(
            DiContainer container, IEnumerable<Type> contracts)
        {
            var map = new Dictionary<Type, List<Type>>();

            foreach (var contractType in contracts)
            {
                var depends = GetDependencies(container, contractType);

                if (depends.Any())
                {
                    map.Add(contractType, depends);
                }
            }

            return map;
        }

        static List<Type> GetDependencies(
            DiContainer container, Type type)
        {
            var dependencies = new List<Type>();

            foreach (var contractType in container.GetDependencyContracts(type))
            {
                List<Type> dependTypes;

                if (contractType.FullName.StartsWith("System.Collections.Generic.List"))
                {
                    var subTypes = contractType.GenericArguments();
                    Assert.IsEqual(subTypes.Length, 1);

                    var subType = subTypes[0];
                    dependTypes = container.ResolveTypeAll(subType);
                }
                else
                {
                    dependTypes = container.ResolveTypeAll(contractType);
                    Assert.That(dependTypes.Count <= 1);
                }

                foreach (var dependType in dependTypes)
                {
                    dependencies.Add(dependType);
                }
            }

            return dependencies;
        }

        static string GetFormattedTypeName(Type type)
        {
            var str = type.PrettyName();

            // GraphViz does not read names with <, >, or . characters so replace them
            str = str.Replace(">", "_");
            str = str.Replace("<", "_");
            str = str.Replace(".", "_");

            return str;
        }
    }
}

