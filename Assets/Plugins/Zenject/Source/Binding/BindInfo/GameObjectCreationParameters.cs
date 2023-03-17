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
    [NoReflectionBaking]
    public class GameObjectCreationParameters
    {
        public string Name
        {
            get;
            set;
        }

        public string GroupName
        {
            get;
            set;
        }

        public Transform ParentTransform
        {
            get;
            set;
        }

        public Func<InjectContext, Transform> ParentTransformGetter
        {
            get;
            set;
        }

        public Vector3? Position
        {
            get;
            set;
        }

        public Quaternion? Rotation
        {
            get;
            set;
        }

        public static readonly GameObjectCreationParameters Default = new GameObjectCreationParameters();

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + (Name == null ? 0 : Name.GetHashCode());
                hash = hash * 29 + (GroupName == null ? 0 : GroupName.GetHashCode());
                hash = hash * 29 + (ParentTransform == null ? 0 : ParentTransform.GetHashCode());
                hash = hash * 29 + (ParentTransformGetter == null ? 0 : ParentTransformGetter.GetHashCode());
                hash = hash * 29 + (!Position.HasValue ? 0 : Position.Value.GetHashCode());
                hash = hash * 29 + (!Rotation.HasValue ? 0 : Rotation.Value.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is GameObjectCreationParameters)
            {
                GameObjectCreationParameters otherId = (GameObjectCreationParameters)other;
                return otherId == this;
            }

            return false;
        }

        public bool Equals(GameObjectCreationParameters that)
        {
            return this == that;
        }

        public static bool operator ==(GameObjectCreationParameters left, GameObjectCreationParameters right)
        {
            return Equals(left.Name, right.Name)
                && Equals(left.GroupName, right.GroupName);
        }

        public static bool operator !=(GameObjectCreationParameters left, GameObjectCreationParameters right)
        {
            return !left.Equals(right);
        }
    }
}

#endif
