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
using UnityEngine.SceneManagement;

namespace Zenject
{
    public class SceneContextRegistry
    {
        readonly Dictionary<Scene, SceneContext> _map = new Dictionary<Scene, SceneContext>();

        public IEnumerable<SceneContext> SceneContexts
        {
            get { return _map.Values; }
        }

        public void Add(SceneContext context)
        {
            Assert.That(!_map.ContainsKey(context.gameObject.scene));
            _map.Add(context.gameObject.scene, context);
        }

        public SceneContext GetSceneContextForScene(string name)
        {
            var scene = SceneManager.GetSceneByName(name);
            Assert.That(scene.IsValid(), "Could not find scene with name '{0}'", name);
            return GetSceneContextForScene(scene);
        }

        public SceneContext GetSceneContextForScene(Scene scene)
        {
            return _map[scene];
        }

        public SceneContext TryGetSceneContextForScene(string name)
        {
            var scene = SceneManager.GetSceneByName(name);
            Assert.That(scene.IsValid(), "Could not find scene with name '{0}'", name);
            return TryGetSceneContextForScene(scene);
        }

        public SceneContext TryGetSceneContextForScene(Scene scene)
        {
            SceneContext context;

            if (_map.TryGetValue(scene, out context))
            {
                return context;
            }

            return null;
        }

        public DiContainer GetContainerForScene(Scene scene)
        {
            var container = TryGetContainerForScene(scene);

            if (container != null)
            {
                return container;
            }

            throw Assert.CreateException(
                "Unable to find DiContainer for scene '{0}'", scene.name);
        }

        public DiContainer TryGetContainerForScene(Scene scene)
        {
            if (scene == ProjectContext.Instance.gameObject.scene)
            {
                return ProjectContext.Instance.Container;
            }

            var sceneContext = TryGetSceneContextForScene(scene);

            if (sceneContext != null)
            {
                return sceneContext.Container;
            }

            return null;
        }

        public void Remove(SceneContext context)
        {
            bool removed = _map.Remove(context.gameObject.scene);

            if (!removed)
            {
                Log.Warn("Failed to remove SceneContext from SceneContextRegistry");
            }
        }
    }

}
