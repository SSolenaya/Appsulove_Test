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

using UnityEditor;

namespace Zenject
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneContext))]
    [NoReflectionBaking]
    public class SceneContextEditor : RunnableContextEditor
    {
        SerializedProperty _contractNameProperty;
        SerializedProperty _parentNamesProperty;
        SerializedProperty _parentNewObjectsUnderSceneContextProperty;

        public override void OnEnable()
        {
            base.OnEnable();

            _contractNameProperty = serializedObject.FindProperty("_contractNames");
            _parentNamesProperty = serializedObject.FindProperty("_parentContractNames");
            _parentNewObjectsUnderSceneContextProperty = serializedObject.FindProperty("_parentNewObjectsUnderSceneContext");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_contractNameProperty, true);
            EditorGUILayout.PropertyField(_parentNamesProperty, true);
            EditorGUILayout.PropertyField(_parentNewObjectsUnderSceneContextProperty);
        }
    }
}


#endif
