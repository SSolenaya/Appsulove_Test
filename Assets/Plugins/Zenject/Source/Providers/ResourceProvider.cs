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
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    [NoReflectionBaking]
    public class ResourceProvider : IProvider
    {
        readonly Type _resourceType;
        readonly string _resourcePath;
        readonly bool _matchSingle;

        public ResourceProvider(
            string resourcePath, Type resourceType, bool matchSingle)
        {
            _resourceType = resourceType;
            _resourcePath = resourcePath;
            _matchSingle = matchSingle;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _resourceType;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);

            Assert.IsNotNull(context);

            if (_matchSingle)
            {
                var obj = Resources.Load(_resourcePath, _resourceType);

                Assert.That(obj != null,
                "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

                // Are there any resource types which can be injected?
                injectAction = null;
                buffer.Add(obj);
                return;
            }

            var objects = Resources.LoadAll(_resourcePath, _resourceType);

            Assert.That(objects.Length > 0,
            "Could not find resource at path '{0}' with type '{1}'", _resourcePath, _resourceType);

            // Are there any resource types which can be injected?
            injectAction = null;

            buffer.AllocFreeAddRange(objects);
        }
    }
}

#endif


