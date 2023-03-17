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

namespace Zenject
{
    // Note that there's a reason we don't just have a generic
    // argument for signal type - because when using struct type signals it throws
    // exceptions on AOT platforms
    public class SignalCallbackWrapper : IDisposable
    {
        readonly SignalBus _signalBus;
        readonly Action<object> _action;
        readonly Type _signalType;
        readonly object _identifier;

        public SignalCallbackWrapper(
            SignalBindingBindInfo bindInfo,
            Action<object> action,
            SignalBus signalBus)
        {
            _signalType = bindInfo.SignalType;
            _identifier = bindInfo.Identifier;
            _signalBus = signalBus;
            _action = action;

            signalBus.SubscribeId(bindInfo.SignalType, _identifier, OnSignalFired);
        }

        void OnSignalFired(object signal)
        {
            _action(signal);
        }

        public void Dispose()
        {
            _signalBus.UnsubscribeId(_signalType, _identifier, OnSignalFired);
        }
    }
}
