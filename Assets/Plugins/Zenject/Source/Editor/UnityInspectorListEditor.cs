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
using System.Linq;
using ModestTree;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Zenject
{
    public abstract class UnityInspectorListEditor : Editor
    {
        List<ReorderableList> _installersLists;
        List<SerializedProperty> _installersProperties;

        protected abstract string[] PropertyDisplayNames
        {
            get;
        }

        protected abstract string[] PropertyNames
        {
            get;
        }

        protected abstract string[] PropertyDescriptions
        {
            get;
        }

        public virtual void OnEnable()
        {
            _installersProperties = new List<SerializedProperty>();
            _installersLists = new List<ReorderableList>();

            var descriptions = PropertyDescriptions;
            var names = PropertyNames;
            var displayNames = PropertyDisplayNames;

            Assert.IsEqual(descriptions.Length, names.Length);

            var infos = Enumerable.Range(0, names.Length).Select(i => new { Name = names[i], DisplayName = displayNames[i], Description = descriptions[i] }).ToList();

            foreach (var info in infos)
            {
                var installersProperty = serializedObject.FindProperty(info.Name);
                _installersProperties.Add(installersProperty);

                ReorderableList installersList = new ReorderableList(serializedObject, installersProperty, true, true, true, true);
                _installersLists.Add(installersList);

                var closedName = info.DisplayName;
                var closedDesc = info.Description;

                installersList.drawHeaderCallback += rect =>
                {
                    GUI.Label(rect,
                    new GUIContent(closedName, closedDesc));
                };
                installersList.drawElementCallback += (rect, index, active, focused) =>
                {
                    rect.width -= 40;
                    rect.x += 20;
                    EditorGUI.PropertyField(rect, installersProperty.GetArrayElementAtIndex(index), GUIContent.none, true);
                };
            }
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnGui();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnGui()
        {
            if (Application.isPlaying)
            {
                GUI.enabled = false;
            }

            foreach (var list in _installersLists)
            {
                list.DoLayoutList();
            }

            GUI.enabled = true;
        }
    }
}

