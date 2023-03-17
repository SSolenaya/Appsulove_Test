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
#pragma warning disable CS1591

using System.Runtime.CompilerServices;

namespace UniRx.Async
{
    public enum AwaiterStatus
    {
        /// <summary>The operation has not yet completed.</summary>
        Pending = 0,
        /// <summary>The operation completed successfully.</summary>
        Succeeded = 1,
        /// <summary>The operation completed with an error.</summary>
        Faulted = 2,
        /// <summary>The operation completed due to cancellation.</summary>
        Canceled = 3
    }

    public interface IAwaiter : ICriticalNotifyCompletion
    {
        AwaiterStatus Status { get; }
        bool IsCompleted { get; }
        void GetResult();
    }

    public interface IAwaiter<out T> : IAwaiter
    {
        new T GetResult();
    }

    public static class AwaiterStatusExtensions
    {
        /// <summary>!= Pending.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this AwaiterStatus status)
        {
            return status != AwaiterStatus.Pending;
        }

        /// <summary>== Succeeded.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompletedSuccessfully(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Succeeded;
        }

        /// <summary>== Canceled.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCanceled(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Canceled;
        }

        /// <summary>== Faulted.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFaulted(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Faulted;
        }
    }
}

#endif