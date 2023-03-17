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
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Zenject
{
    public abstract class ZenjectEditorWindow : EditorWindow
    {
        [Inject]
        [NonSerialized]
        Kernel _kernel;

        [Inject]
        [NonSerialized]
        GuiRenderableManager _guiRenderableManager;

        [NonSerialized]
        DiContainer _container;

        [NonSerialized]
        Exception _fatalError;

        [NonSerialized]
        GUIStyle _errorTextStyle;

        GUIStyle ErrorTextStyle
        {
            get
            {
                if (_errorTextStyle == null)
                {
                    _errorTextStyle = new GUIStyle(GUI.skin.label);
                    _errorTextStyle.fontSize = 18;
                    _errorTextStyle.normal.textColor = Color.red;
                    _errorTextStyle.wordWrap = true;
                    _errorTextStyle.alignment = TextAnchor.MiddleCenter;
                }

                return _errorTextStyle;
            }
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        public virtual void OnEnable()
        {
            if (_fatalError != null)
            {
                return;
            }

            Initialize();
        }

        protected virtual void Initialize()
        {
            Assert.IsNull(_container);

            _container = new DiContainer(new[] { StaticContext.Container });

            // Make sure we don't create any game objects since editor windows don't have a scene
            _container.AssertOnNewGameObjects = true;

            ZenjectManagersInstaller.Install(_container);

            _container.Bind<Kernel>().AsSingle();
            _container.Bind<GuiRenderableManager>().AsSingle();
            _container.BindInstance(this);

            InstallBindings();

            _container.QueueForInject(this);
            _container.ResolveRoots();

            _kernel.Initialize();
        }

        public virtual void OnDisable()
        {
            if (_fatalError != null)
            {
                return;
            }

            _kernel.Dispose();
        }

        public virtual void Update()
        {
            if (_fatalError != null)
            {
                return;
            }

            try
            {
                _kernel.Tick();
            }
            catch (Exception e)
            {
                Log.ErrorException(e);
                _fatalError = e;
            }

            // We might also consider only calling Repaint when changes occur
            Repaint();
        }

        public virtual void OnGUI()
        {
            if (_fatalError != null)
            {
                var labelWidth = 600;
                var labelHeight = 200;

                GUI.Label(new Rect(Screen.width / 2 - labelWidth / 2, Screen.height / 3 - labelHeight / 2, labelWidth, labelHeight), "Unrecoverable error occurred!  \nSee log for details.", ErrorTextStyle);

                var buttonWidth = 100;
                var buttonHeight = 50;
                var offset = new Vector2(0, 100);

                if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2 + offset.x, Screen.height / 3 - buttonHeight / 2 + offset.y, buttonWidth, buttonHeight), "Reload"))
                {
                    ExecuteFullReload();
                }
            }
            else
            {
                try
                {
                    if (_guiRenderableManager != null)
                    {
                        _guiRenderableManager.OnGui();
                    }
                }
                catch (Exception e)
                {
                    Log.ErrorException(e);
                    _fatalError = e;
                }
            }
        }

        protected virtual void ExecuteFullReload()
        {
            _kernel = null;
            _guiRenderableManager = null;
            _container = null;
            _fatalError = null;

            Initialize();
        }

        public abstract void InstallBindings();
    }
}
