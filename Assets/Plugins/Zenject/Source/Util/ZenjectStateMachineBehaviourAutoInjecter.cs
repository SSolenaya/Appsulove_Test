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
    public class ZenjectStateMachineBehaviourAutoInjecter : MonoBehaviour
    {
        DiContainer _container;
        Animator _animator;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);
        }

        // The unity docs (https://unity3d.com/learn/tutorials/modules/beginner/5-pre-order-beta/state-machine-behaviours)
        // mention that StateMachineBehaviour's should only be retrieved in the Start method
        // which is why we do it here
        public void Start()
        {
            // Animator can be null when users create GameObjects directly so in that case
            // Just don't bother attempting to inject the behaviour classes
            if (_animator != null)
            {
                var behaviours = _animator.GetBehaviours<StateMachineBehaviour>();

                if (behaviours != null)
                {
                    foreach (var behaviour in behaviours)
                    {
                        _container.Inject(behaviour);
                    }
                }
            }
        }
    }
}
