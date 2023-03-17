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

using UnityEngine;
using UnityEngine.Serialization;

namespace Zenject
{
    public class ZenjectBinding : MonoBehaviour
    {
        [Tooltip("The component to add to the Zenject container")]
        [SerializeField]
        Component[] _components = null;

        [Tooltip("Note: This value is optional and can be ignored in most cases.  This can be useful to differentiate multiple bindings of the same type.  For example, if you have multiple cameras in your scene, you can 'name' them by giving each one a different identifier.  For your main camera you might call it 'Main' then any class can refer to it by using an attribute like [Inject(Id = 'Main')]")]
        [SerializeField]
        string _identifier = string.Empty;

        [Tooltip("When set, this will bind the given components to the SceneContext.  It can be used as a shortcut to explicitly dragging the SceneContext into the Context field.  This is useful when using ZenjectBinding inside GameObjectContext.  If your ZenjectBinding is for a component that is not underneath GameObjectContext then it is not necessary to check this")]
        [SerializeField]
        bool _useSceneContext = false;

        [Tooltip("Note: This value is optional and can be ignored in most cases.  This value will determine what container the component gets added to.  If unset, the component will be bound on the most 'local' context.  In most cases this will be the SceneContext, unless this component is underneath a GameObjectContext, or ProjectContext, in which case it will bind to that instead by default.  You can also override this default by providing the Context directly.  This can be useful if you want to bind something that is inside a GameObjectContext to the SceneContext container.")]
        [SerializeField]
        [FormerlySerializedAs("_compositionRoot")]
        Context _context = null;

        [Tooltip("This value is used to determine how to bind this component.  When set to 'Self' is equivalent to calling Container.FromInstance inside an installer. When set to 'AllInterfaces' this is equivalent to calling 'Container.BindInterfaces<MyMonoBehaviour>().ToInstance', and similarly for InterfacesAndSelf")]
        [SerializeField]
        BindTypes _bindType = BindTypes.Self;

        public bool UseSceneContext
        {
            get { return _useSceneContext; }
        }

        public Context Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public Component[] Components
        {
            get { return _components; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public BindTypes BindType
        {
            get { return _bindType; }
        }

        public void Start()
        {
            // Define this method so we expose the enabled check box
        }

        public enum BindTypes
        {
            Self,
            AllInterfaces,
            AllInterfacesAndSelf,
            BaseType
        }
    }
}

#endif
