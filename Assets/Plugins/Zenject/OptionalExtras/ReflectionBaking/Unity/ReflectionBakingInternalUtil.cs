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
using System.IO;
using System.Reflection;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Zenject.ReflectionBaking
{
    public static class ReflectionBakingInternalUtil
    {
        public static string ConvertAssetPathToSystemPath(string assetPath)
        {
            string path = Application.dataPath;
            int pathLength = path.Length;
            path = path.Substring(0, pathLength - /* Assets */ 6);
            path = Path.Combine(path, assetPath);
            return path;
        }

        public static ZenjectReflectionBakingSettings TryGetEnabledSettingsInstance()
        {
            string[] guids = AssetDatabase.FindAssets("t:ZenjectReflectionBakingSettings");

            if (guids.IsEmpty())
            {
                return null;
            }

            ZenjectReflectionBakingSettings enabledSettings = null;

            foreach (var guid in guids)
            {
                var candidate = AssetDatabase.LoadAssetAtPath<ZenjectReflectionBakingSettings>(
                    AssetDatabase.GUIDToAssetPath(guid));

                if ((Application.isEditor && candidate.IsEnabledInEditor) || (BuildPipeline.isBuildingPlayer && candidate.IsEnabledInBuilds))
                {
                    Assert.IsNull(enabledSettings, "Found multiple enabled ZenjectReflectionBakingSettings objects!  Please disable/delete one to continue.");
                    enabledSettings = candidate;
                }
            }

            return enabledSettings;
        }

        public static string ConvertAbsoluteToAssetPath(string systemPath)
        {
            var projectPath = Application.dataPath;

            // Remove 'Assets'
            projectPath = projectPath.Substring(0, projectPath.Length - /* Assets */ 6);

            int systemPathLength = systemPath.Length;
            int assetPathLength = systemPathLength - projectPath.Length;

            Assert.That(assetPathLength > 0, "Unexpect path '{0}'", systemPath);

            return systemPath.Substring(projectPath.Length, assetPathLength);
        }

        public static void TryForceUnityFullCompile()
        {
            Type compInterface = typeof(UnityEditor.Editor).Assembly.GetType(
                "UnityEditor.Scripting.ScriptCompilation.EditorCompilationInterface");

            if (compInterface != null)
            {
                var dirtyAllScriptsMethod = compInterface.GetMethod(
                    "DirtyAllScripts", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                dirtyAllScriptsMethod.Invoke(null, null);
            }

            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
