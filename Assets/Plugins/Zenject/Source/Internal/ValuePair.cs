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
using System.Collections.Generic;

namespace ModestTree.Util
{
    public class ValuePair<T1, T2>
    {
        public readonly T1 First;
        public readonly T2 Second;

        public ValuePair()
        {
            First = default(T1);
            Second = default(T2);
        }

        public ValuePair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public override bool Equals(Object obj)
        {
            var that = obj as ValuePair<T1, T2>;

            if (that == null)
            {
                return false;
            }

            return Equals(that);
        }

        public bool Equals(ValuePair<T1, T2> that)
        {
            if (that == null)
            {
                return false;
            }

            return object.Equals(First, that.First) && object.Equals(Second, that.Second);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + (First == null ? 0 : First.GetHashCode());
                hash = hash * 29 + (Second == null ? 0 : Second.GetHashCode());
                return hash;
            }
        }
    }

    public class ValuePair<T1, T2, T3>
    {
        public readonly T1 First;
        public readonly T2 Second;
        public readonly T3 Third;

        public ValuePair()
        {
            First = default(T1);
            Second = default(T2);
            Third = default(T3);
        }

        public ValuePair(T1 first, T2 second, T3 third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public override bool Equals(Object obj)
        {
            var that = obj as ValuePair<T1, T2, T3>;

            if (that == null)
            {
                return false;
            }

            return Equals(that);
        }

        public bool Equals(ValuePair<T1, T2, T3> that)
        {
            if (that == null)
            {
                return false;
            }

            return object.Equals(First, that.First) && object.Equals(Second, that.Second) && object.Equals(Third, that.Third);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + (First == null ? 0 : First.GetHashCode());
                hash = hash * 29 + (Second == null ? 0 : Second.GetHashCode());
                hash = hash * 29 + (Third == null ? 0 : Third.GetHashCode());
                return hash;
            }
        }
    }

    public class ValuePair<T1, T2, T3, T4>
    {
        public readonly T1 First;
        public readonly T2 Second;
        public readonly T3 Third;
        public readonly T4 Fourth;

        public ValuePair()
        {
            First = default(T1);
            Second = default(T2);
            Third = default(T3);
            Fourth = default(T4);
        }

        public ValuePair(T1 first, T2 second, T3 third, T4 fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        public override bool Equals(Object obj)
        {
            var that = obj as ValuePair<T1, T2, T3, T4>;

            if (that == null)
            {
                return false;
            }

            return Equals(that);
        }

        public bool Equals(ValuePair<T1, T2, T3, T4> that)
        {
            if (that == null)
            {
                return false;
            }

            return object.Equals(First, that.First) && object.Equals(Second, that.Second)
                && object.Equals(Third, that.Third) && object.Equals(Fourth, that.Fourth);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + (First == null ? 0 : First.GetHashCode());
                hash = hash * 29 + (Second == null ? 0 : Second.GetHashCode());
                hash = hash * 29 + (Third == null ? 0 : Third.GetHashCode());
                hash = hash * 29 + (Fourth == null ? 0 : Fourth.GetHashCode());
                return hash;
            }
        }
    }

    public static class ValuePair
    {
        public static ValuePair<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            return new ValuePair<T1, T2>(first, second);
        }

        public static ValuePair<T1, T2, T3> New<T1, T2, T3>(T1 first, T2 second, T3 third)
        {
            return new ValuePair<T1, T2, T3>(first, second, third);
        }

        public static ValuePair<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth)
        {
            return new ValuePair<T1, T2, T3, T4>(first, second, third, fourth);
        }
    }
}

