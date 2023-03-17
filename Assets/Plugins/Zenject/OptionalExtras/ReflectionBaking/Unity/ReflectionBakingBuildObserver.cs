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
using System.Diagnostics;
using System.IO;
using System.Linq;
using ModestTree;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Zenject.ReflectionBaking.Mono.Cecil;
using Debug = UnityEngine.Debug;

namespace Zenject.ReflectionBaking
{
    public static class ReflectionBakingBuildObserver
    {
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompiled;
        }

        static void OnAssemblyCompiled(string assemblyAssetPath, CompilerMessage[] messages)
        {
#if !UNITY_2018
            if (Application.isEditor && !BuildPipeline.isBuildingPlayer)
            {
                return;
            }
#endif

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer)
            {
                Log.Warn("Zenject reflection baking skipped because it is not currently supported on WSA platform!");
            }
            else
            {
                TryWeaveAssembly(assemblyAssetPath);
            }
        }

        static void TryWeaveAssembly(string assemblyAssetPath)
        {
            var settings = ReflectionBakingInternalUtil.TryGetEnabledSettingsInstance();

            if (settings == null)
            {
                return;
            }

            if (settings.AllGeneratedAssemblies && settings.ExcludeAssemblies.Contains(assemblyAssetPath))
            {
                return;
            }

            if (!settings.AllGeneratedAssemblies && !settings.IncludeAssemblies.Contains(assemblyAssetPath))
            {
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var assemblyFullPath = ReflectionBakingInternalUtil.ConvertAssetPathToSystemPath(assemblyAssetPath);

            var readerParameters = new ReaderParameters
            {
                AssemblyResolver = new UnityAssemblyResolver(),
                // Is this necessary?
                //ReadSymbols = true,
            };

            var module = ModuleDefinition.ReadModule(assemblyFullPath, readerParameters);

            var assemblyRefNames = module.AssemblyReferences.Select(x => x.Name.ToLower()).ToList();

            if (!assemblyRefNames.Contains("zenject-usage"))
            {
                // Zenject-usage is used by the generated methods
                // Important that we do this check otherwise we can corrupt some dlls that don't have access to it
                return;
            }

            var assemblyName = Path.GetFileNameWithoutExtension(assemblyAssetPath);
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name == assemblyName).OnlyOrDefault();

            Assert.IsNotNull(assembly, "Could not find unique assembly '{0}' in currently loaded list of assemblies", assemblyName);

            int numTypesChanged = ReflectionBakingModuleEditor.WeaveAssembly(
                module, assembly, settings.NamespacePatterns);

            if (numTypesChanged > 0)
            {
                var writerParams = new WriterParameters()
                {
                    // Is this necessary?
                    //WriteSymbols = true
                };

                module.Write(assemblyFullPath, writerParams);

                Debug.Log("Added reflection baking to '{0}' types in assembly '{1}', took {2:0.00} seconds"
                    .Fmt(numTypesChanged, Path.GetFileName(assemblyAssetPath), stopwatch.Elapsed.TotalSeconds));
            }
        }
    }
}
