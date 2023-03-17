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

namespace ModestTree
{
    // Simple wrapper around unity's logging system
    public static class Log
    {
        // Strip out debug logs outside of unity
        [Conditional("UNITY_EDITOR")]
        public static void Debug(string message, params object[] args)
        {
#if NOT_UNITY3D
            //Console.WriteLine(message.Fmt(args));
#else
            //UnityEngine.Debug.Log(message.Fmt(args));
#endif
        }

        /////////////

        public static void Info(string message, params object[] args)
        {
#if NOT_UNITY3D
            Console.WriteLine(message.Fmt(args));
#else
            UnityEngine.Debug.Log(message.Fmt(args));
#endif
        }

        /////////////

        public static void Warn(string message, params object[] args)
        {
#if NOT_UNITY3D
            Console.WriteLine(message.Fmt(args));
#else
            UnityEngine.Debug.LogWarning(message.Fmt(args));
#endif
        }

        /////////////

        public static void Trace(string message, params object[] args)
        {
#if NOT_UNITY3D
            Console.WriteLine(message.Fmt(args));
#else
            UnityEngine.Debug.Log(message.Fmt(args));
#endif
        }

        /////////////

        public static void ErrorException(Exception e)
        {
#if NOT_UNITY3D
            Console.WriteLine(e.ToString());
#else
            UnityEngine.Debug.LogException(e);
#endif
        }

        public static void ErrorException(string message, Exception e)
        {
#if NOT_UNITY3D
            Console.WriteLine(message);
#else
            UnityEngine.Debug.LogError(message);
            UnityEngine.Debug.LogException(e);
#endif
        }

        public static void Error(string message, params object[] args)
        {
#if NOT_UNITY3D
            Console.WriteLine(message.Fmt(args));
#else
            UnityEngine.Debug.LogError(message.Fmt(args));
#endif
        }
    }
}

