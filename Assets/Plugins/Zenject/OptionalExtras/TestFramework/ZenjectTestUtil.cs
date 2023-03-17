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

using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zenject.Internal
{
    public static class ZenjectTestUtil
    {
        public const string UnitTestRunnerGameObjectName = "Code-based tests runner";

        public static void DestroyEverythingExceptTestRunner(bool immediate)
        {
            var testRunner = GameObject.Find(UnitTestRunnerGameObjectName);
            Assert.IsNotNull(testRunner);
            GameObject.DontDestroyOnLoad(testRunner);

            // We want to clear all objects across all scenes to ensure the next test is not affected
            // at all by previous tests
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                foreach (var obj in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    GameObject.DestroyImmediate(obj);
                }
            }

            if (ProjectContext.HasInstance)
            {
                var dontDestroyOnLoadRoots = ProjectContext.Instance.gameObject.scene
                    .GetRootGameObjects();

                foreach (var rootObj in dontDestroyOnLoadRoots)
                {
                    if (rootObj.name != UnitTestRunnerGameObjectName)
                    {
                        if (immediate)
                        {
                            GameObject.DestroyImmediate(rootObj);
                        }
                        else
                        {
                            GameObject.Destroy(rootObj);
                        }
                    }
                }
            }
        }
    }
}
