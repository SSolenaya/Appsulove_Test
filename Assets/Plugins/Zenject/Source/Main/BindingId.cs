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
using System.Diagnostics;
using ModestTree;

namespace Zenject
{
    [DebuggerStepThrough]
    public struct BindingId : IEquatable<BindingId>
    {
        Type _type;
        object _identifier;

        public BindingId(Type type, object identifier)
        {
            _type = type;
            _identifier = identifier;
        }

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public object Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public override string ToString()
        {
            if (_identifier == null)
            {
                return _type.PrettyName();
            }

            return "{0} (ID: {1})".Fmt(_type, _identifier);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + _type.GetHashCode();
                hash = hash * 29 + (_identifier == null ? 0 : _identifier.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is BindingId)
            {
                BindingId otherId = (BindingId)other;
                return otherId == this;
            }

            return false;
        }

        public bool Equals(BindingId that)
        {
            return this == that;
        }

        public static bool operator ==(BindingId left, BindingId right)
        {
            return left.Type == right.Type && Equals(left.Identifier, right.Identifier);
        }

        public static bool operator !=(BindingId left, BindingId right)
        {
            return !left.Equals(right);
        }
    }
}
