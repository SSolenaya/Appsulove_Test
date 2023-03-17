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
using System.IO;
using System.Linq;
using ModestTree;
using UnityEditorInternal;
using UnityEngine;

namespace Zenject.ReflectionBaking
{
    public class AssemblyPathRegistry
    {
        static List<string> _assemblies;

        public static List<string> GetAllGeneratedAssemblyRelativePaths()
        {
            if (_assemblies == null)
            {
                _assemblies = LookupAllGeneratedAssemblyPaths();
                Assert.IsNotNull(_assemblies);
            }

            return _assemblies;
        }

        static bool IsManagedAssembly(string systemPath)
        {
            DllType dllType = InternalEditorUtility.DetectDotNetDll(systemPath);
            return dllType != DllType.Unknown && dllType != DllType.Native;
        }

        static List<string> LookupAllGeneratedAssemblyPaths()
        {
            var assemblies = new List<string>(20);

            // We could also add the ones in the project but we probably don't want to edit those
            //FindAssemblies(Application.dataPath, 120, assemblies);

            FindAssemblies(Application.dataPath + "/../Library/ScriptAssemblies/", 2, assemblies);

            return assemblies;
        }

        public static void FindAssemblies(string systemPath, int maxDepth, List<string> result)
        {
            if (maxDepth > 0)
            {
                if (Directory.Exists(systemPath))
                {
                    var dirInfo = new DirectoryInfo(systemPath);

                    result.AddRange(
                        dirInfo.GetFiles().Select(x => x.FullName)
                        .Where(IsManagedAssembly)
                        .Select(ReflectionBakingInternalUtil.ConvertAbsoluteToAssetPath));

                    var directories = dirInfo.GetDirectories();

                    for (int i = 0; i < directories.Length; i++)
                    {
                        DirectoryInfo current = directories[i];

                        FindAssemblies(current.FullName, maxDepth - 1, result);
                    }
                }
            }
        }
    }
}
