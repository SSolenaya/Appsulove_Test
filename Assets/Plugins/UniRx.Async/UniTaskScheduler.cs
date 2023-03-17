// The MIT License (MIT)
//
// Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc.
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

#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;

namespace UniRx.Async
{
    // UniTask has no scheduler like TaskScheduler.
    // Only handle unobserved exception.

    public static class UniTaskScheduler
    {
        public static event Action<Exception> UnobservedTaskException;

        /// <summary>
        /// Propagate OperationCanceledException to UnobservedTaskException when true. Default is false.
        /// </summary>
        public static bool PropagateOperationCanceledException = false;

        /// <summary>
        /// Write log type when catch unobserved exception and not registered UnobservedTaskException. Default is Warning.
        /// </summary>
        public static UnityEngine.LogType UnobservedExceptionWriteLogType = UnityEngine.LogType.Warning;

        /// <summary>
        /// Dispatch exception event to Unity MainThread.
        /// </summary>
        public static bool DispatchUnityMainThread = true;

        // cache delegate.
        static readonly SendOrPostCallback handleExceptionInvoke = InvokeUnobservedTaskException;

        internal static void PublishUnobservedTaskException(Exception ex)
        {
            if (ex != null)
            {
                if (!PropagateOperationCanceledException && ex is OperationCanceledException)
                {
                    return;
                }

                if (UnobservedTaskException != null)
                {
                    if (Thread.CurrentThread.ManagedThreadId == PlayerLoopHelper.MainThreadId)
                    {
                        // allows inlining call.
                        UnobservedTaskException.Invoke(ex);
                    }
                    else
                    {
                        // Post to MainThread.
                        PlayerLoopHelper.UnitySynchronizationContext.Post(handleExceptionInvoke, ex);
                    }
                }
                else
                {
                    string msg = null;
                    if (UnobservedExceptionWriteLogType != UnityEngine.LogType.Exception)
                    {
                        msg = "UnobservedTaskException:" + ex.ToString();
                    }
                    switch (UnobservedExceptionWriteLogType)
                    {
                        case UnityEngine.LogType.Error:
                            UnityEngine.Debug.LogError(msg);
                            break;
                        case UnityEngine.LogType.Assert:
                            UnityEngine.Debug.LogAssertion(msg);
                            break;
                        case UnityEngine.LogType.Warning:
                            UnityEngine.Debug.LogWarning(msg);
                            break;
                        case UnityEngine.LogType.Log:
                            UnityEngine.Debug.Log(msg);
                            break;
                        case UnityEngine.LogType.Exception:
                            UnityEngine.Debug.LogException(ex);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        static void InvokeUnobservedTaskException(object state)
        {
            UnobservedTaskException((Exception)state);
        }
    }
}

#endif