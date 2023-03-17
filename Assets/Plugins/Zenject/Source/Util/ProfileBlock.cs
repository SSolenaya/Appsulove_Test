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
using System.Text.RegularExpressions;
using ModestTree;
#if UNITY_EDITOR
using UnityEngine.Profiling;
using System.Threading;
#endif

namespace Zenject
{
    [NoReflectionBaking]
    public class ProfileBlock : IDisposable
    {
#if UNITY_EDITOR
        static int _blockCount;
        static ProfileBlock _instance = new ProfileBlock();
        static Dictionary<int, string> _nameCache = new Dictionary<int, string>();

        ProfileBlock()
        {
        }

        public static Thread UnityMainThread
        {
            get; set;
        }

        public static Regex ProfilePattern
        {
            get;
            set;
        }

        static int GetHashCode(object p1, object p2)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + p1.GetHashCode();
                hash = hash * 29 + p2.GetHashCode();
                return hash;
            }
        }

        static int GetHashCode(object p1, object p2, object p3)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + p1.GetHashCode();
                hash = hash * 29 + p2.GetHashCode();
                hash = hash * 29 + p3.GetHashCode();
                return hash;
            }
        }

        public static ProfileBlock Start(string sampleNameFormat, object obj1, object obj2)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null
                || !UnityMainThread.Equals(Thread.CurrentThread))
            {
                return null;
            }

            if (!Profiler.enabled)
            {
                return null;
            }

            // We need to ensure that we do not have per-frame allocations in ProfileBlock
            // to avoid infecting the test too much, so use a cache of formatted strings given
            // the input values
            // This only works if the input values do not change per frame
            var hash = GetHashCode(sampleNameFormat, obj1, obj2);

            string formatString;

            if (!_nameCache.TryGetValue(hash, out formatString))
            {
                formatString = string.Format(sampleNameFormat, obj1, obj2);
                _nameCache.Add(hash, formatString);
            }

            return StartInternal(formatString);
#endif
        }

        public static ProfileBlock Start(string sampleNameFormat, object obj)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null
                || !UnityMainThread.Equals(Thread.CurrentThread))
            {
                return null;
            }

            if (!Profiler.enabled)
            {
                return null;
            }

            // We need to ensure that we do not have per-frame allocations in ProfileBlock
            // to avoid infecting the test too much, so use a cache of formatted strings given
            // the input values
            // This only works if the input values do not change per frame
            var hash = GetHashCode(sampleNameFormat, obj);

            string formatString;

            if (!_nameCache.TryGetValue(hash, out formatString))
            {
                formatString = string.Format(sampleNameFormat, obj);
                _nameCache.Add(hash, formatString);
            }

            return StartInternal(formatString);
#endif
        }

        public static ProfileBlock Start(string sampleName)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null
                || !UnityMainThread.Equals(Thread.CurrentThread))
            {
                return null;
            }

            if (!Profiler.enabled)
            {
                return null;
            }

            return StartInternal(sampleName);
#endif
        }

        static ProfileBlock StartInternal(string sampleName)
        {
            Assert.That(Profiler.enabled);

            if (ProfilePattern == null || ProfilePattern.Match(sampleName).Success)
            {
                Profiler.BeginSample(sampleName);
                _blockCount++;
                return _instance;
            }

            return null;
        }

        public void Dispose()
        {
            _blockCount--;
            Assert.That(_blockCount >= 0);
            Profiler.EndSample();
        }

#else
        ProfileBlock(string sampleName, bool rootBlock)
        {
        }

        ProfileBlock(string sampleName)
            : this(sampleName, false)
        {
        }

        public static Regex ProfilePattern
        {
            get;
            set;
        }

        public static ProfileBlock Start()
        {
            return null;
        }

        public static ProfileBlock Start(string sampleNameFormat, object obj1, object obj2)
        {
            return null;
        }

        // Remove the call completely for builds
        public static ProfileBlock Start(string sampleNameFormat, object obj)
        {
            return null;
        }

        // Remove the call completely for builds
        public static ProfileBlock Start(string sampleName)
        {
            return null;
        }

        public void Dispose()
        {
        }
#endif
    }
}
