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

namespace Zenject
{
    public class ZenAutoInjecter : MonoBehaviour
    {
        [SerializeField]
        ContainerSources _containerSource = ContainerSources.SearchHierarchy;

        bool _hasInjected;

        public ContainerSources ContainerSource
        {
            get { return _containerSource; }
            set { _containerSource = value; }
        }

        // Make sure they don't cause injection to happen twice
        [Inject]
        public void Construct()
        {
            if (!_hasInjected)
            {
                throw Assert.CreateException(
                    "ZenAutoInjecter was injected!  Do not use ZenAutoInjecter for objects that are instantiated through zenject or which exist in the initial scene hierarchy");
            }
        }

        public void Awake()
        {
            _hasInjected = true;
            LookupContainer().InjectGameObject(gameObject);
        }

        DiContainer LookupContainer()
        {
            if (_containerSource == ContainerSources.ProjectContext)
            {
                return ProjectContext.Instance.Container;
            }

            if (_containerSource == ContainerSources.SceneContext)
            {
                return GetContainerForCurrentScene();
            }

            Assert.IsEqual(_containerSource, ContainerSources.SearchHierarchy);

            var parentContext = transform.GetComponentInParent<Context>();

            if (parentContext != null)
            {
                return parentContext.Container;
            }

            return GetContainerForCurrentScene();
        }

        DiContainer GetContainerForCurrentScene()
        {
            return ProjectContext.Instance.Container.Resolve<SceneContextRegistry>()
                .GetContainerForScene(gameObject.scene);
        }

        public enum ContainerSources
        {
            SceneContext,
            ProjectContext,
            SearchHierarchy
        }
    }
}
