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
    public class SignalCallbackWithLookupWrapper : IDisposable
    {
        readonly DiContainer _container;
        readonly SignalBus _signalBus;
        readonly Guid _lookupId;
        readonly Func<object, Action<object>> _methodGetter;
        readonly Type _objectType;
        readonly Type _signalType;
        readonly object _identifier;

        public SignalCallbackWithLookupWrapper(
            SignalBindingBindInfo signalBindInfo,
            Type objectType,
            Guid lookupId,
            Func<object, Action<object>> methodGetter,
            SignalBus signalBus,
            DiContainer container)
        {
            _objectType = objectType;
            _signalType = signalBindInfo.SignalType;
            _identifier = signalBindInfo.Identifier;
            _container = container;
            _methodGetter = methodGetter;
            _signalBus = signalBus;
            _lookupId = lookupId;

            signalBus.SubscribeId(signalBindInfo.SignalType, _identifier, OnSignalFired);
        }

        void OnSignalFired(object signal)
        {
            var objects = _container.ResolveIdAll(_objectType, _lookupId);

            for (int i = 0; i < objects.Count; i++)
            {
                _methodGetter(objects[i])(signal);
            }
        }

        public void Dispose()
        {
            _signalBus.UnsubscribeId(_signalType, _identifier, OnSignalFired);
        }
    }
}

