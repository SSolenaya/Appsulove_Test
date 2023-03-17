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

#if !ODIN_INSPECTOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    [CustomEditor(typeof(SceneDecoratorContext))]
    [NoReflectionBaking]
    public class SceneDecoratorContextEditor : ContextEditor
    {
        SerializedProperty _decoratedContractNameProperty;

        protected override string[] PropertyNames
        {
            get
            {
                return base.PropertyNames.Concat(new string[]
                    {
                        "_lateInstallers",
                        "_lateInstallerPrefabs",
                        "_lateScriptableObjectInstallers"
                    })
                    .ToArray();
            }
        }

        protected override string[] PropertyDisplayNames
        {
            get
            {
                return base.PropertyDisplayNames.Concat(new string[]
                    {
                        "Late Installers",
                        "Late Prefab Installers",
                        "Late Scriptable Object Installers"
                    })
                    .ToArray();
            }
        }

        protected override string[] PropertyDescriptions
        {
            get
            {
                return base.PropertyDescriptions.Concat(new string[]
                    {
                        "Drag any MonoInstallers that you have added to your Scene Hierarchy here. They'll be installed after the target installs its bindings",
                        "Drag any prefabs that contain a MonoInstaller on them here. They'll be installed after the target installs its bindings",
                        "Drag any assets in your Project that implement ScriptableObjectInstaller here. They'll be installed after the target installs its bindings"
                    })
                    .ToArray();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _decoratedContractNameProperty = serializedObject.FindProperty("_decoratedContractName");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_decoratedContractNameProperty);
        }
    }
}

#endif
