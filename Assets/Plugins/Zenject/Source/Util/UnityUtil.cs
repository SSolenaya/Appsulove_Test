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

#if !NOT_UNITY3D

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModestTree.Util
{
    public static class UnityUtil
    {
        public static IEnumerable<Scene> AllScenes
        {
            get
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    yield return SceneManager.GetSceneAt(i);
                }
            }
        }

        public static IEnumerable<Scene> AllLoadedScenes
        {
            get { return AllScenes.Where(scene => scene.isLoaded); }
        }

        public static bool IsAltKeyDown
        {
            get { return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt); }
        }

        public static bool IsControlKeyDown
        {
            get { return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl); }
        }

        public static bool IsShiftKeyDown
        {
            get { return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); }
        }

        public static bool WasShiftKeyJustPressed
        {
            get { return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift); }
        }

        public static bool WasAltKeyJustPressed
        {
            get { return Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt); }
        }

        public static int GetDepthLevel(Transform transform)
        {
            if (transform == null)
            {
                return 0;
            }

            return 1 + GetDepthLevel(transform.parent);
        }

        public static GameObject GetRootParentOrSelf(GameObject gameObject)
        {
            return GetParentsAndSelf(gameObject.transform).Select(x => x.gameObject).LastOrDefault();
        }

        public static IEnumerable<Transform> GetParents(Transform transform)
        {
            if (transform == null)
            {
                yield break;
            }

            foreach (var ancestor in GetParentsAndSelf(transform.parent))
            {
                yield return ancestor;
            }
        }

        public static IEnumerable<Transform> GetParentsAndSelf(Transform transform)
        {
            if (transform == null)
            {
                yield break;
            }

            yield return transform;

            foreach (var ancestor in GetParentsAndSelf(transform.parent))
            {
                yield return ancestor;
            }
        }

        public static IEnumerable<Component> GetComponentsInChildrenTopDown(GameObject gameObject, bool includeInactive)
        {
            return gameObject.GetComponentsInChildren<Component>(includeInactive)
                .OrderBy(x =>
                    x == null ? int.MinValue : GetDepthLevel(x.transform));
        }

        public static IEnumerable<Component> GetComponentsInChildrenBottomUp(GameObject gameObject, bool includeInactive)
        {
            return gameObject.GetComponentsInChildren<Component>(includeInactive)
                .OrderByDescending(x =>
                    x == null ? int.MinValue : GetDepthLevel(x.transform));
        }

        public static IEnumerable<GameObject> GetDirectChildrenAndSelf(GameObject obj)
        {
            yield return obj;

            foreach (Transform child in obj.transform)
            {
                yield return child.gameObject;
            }
        }

        public static IEnumerable<GameObject> GetDirectChildren(GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                yield return child.gameObject;
            }
        }

        public static IEnumerable<GameObject> GetAllGameObjects()
        {
            return GameObject.FindObjectsOfType<Transform>().Select(x => x.gameObject);
        }

        public static List<GameObject> GetAllRootGameObjects()
        {
            return GetAllGameObjects().Where(x => x.transform.parent == null).ToList();
        }
    }
}
#endif
