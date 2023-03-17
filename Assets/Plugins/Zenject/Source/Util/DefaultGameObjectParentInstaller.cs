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

using System;
using UnityEngine;

namespace Zenject
{
    public class DefaultGameObjectParentInstaller : Installer<string, DefaultGameObjectParentInstaller>
    {
        readonly string _name;

        public DefaultGameObjectParentInstaller(string name)
        {
            _name = name;
        }

        public override void InstallBindings()
        {
#if !ZEN_TESTS_OUTSIDE_UNITY
            var defaultParent = new GameObject(_name);

            defaultParent.transform.SetParent(
                Container.InheritedDefaultParent, false);

            Container.DefaultParent = defaultParent.transform;

            Container.Bind<IDisposable>()
                .To<DefaultParentObjectDestroyer>().AsCached().WithArguments(defaultParent);

            // Always destroy the default parent last so that the non-monobehaviours get a chance
            // to clean it up if they want to first
            Container.BindDisposableExecutionOrder<DefaultParentObjectDestroyer>(int.MinValue);
#endif
        }

        class DefaultParentObjectDestroyer : IDisposable
        {
            readonly GameObject _gameObject;

            public DefaultParentObjectDestroyer(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public void Dispose()
            {
                GameObject.Destroy(_gameObject);
            }
        }
    }
}

#endif
