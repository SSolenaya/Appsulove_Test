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
using ModestTree;

namespace Zenject
{
    public class BindSignalToBinder<TSignal>
    {
        DiContainer _container;
        BindStatement _bindStatement;
        SignalBindingBindInfo _signalBindInfo;

        public BindSignalToBinder(DiContainer container, SignalBindingBindInfo signalBindInfo)
        {
            _container = container;

            _signalBindInfo = signalBindInfo;
            // This will ensure that they finish the binding
            _bindStatement = container.StartBinding();
        }

        protected SignalBindingBindInfo SignalBindInfo
        {
            get { return _signalBindInfo; }
        }

        public SignalCopyBinder ToMethod(Action<TSignal> callback)
        {
            Assert.That(!_bindStatement.HasFinalizer);
            _bindStatement.SetFinalizer(new NullBindingFinalizer());

            var bindInfo = _container.Bind<IDisposable>()
                .To<SignalCallbackWrapper>()
                .AsCached()
                // Note that there's a reason we don't just make SignalCallbackWrapper have a generic
                // argument for signal type - because when using struct type signals it throws
                // exceptions on AOT platforms
                .WithArguments(_signalBindInfo, (Action<object>)(o => callback((TSignal)o)))
                .NonLazy().BindInfo;

            return new SignalCopyBinder(bindInfo);
        }

        public SignalCopyBinder ToMethod(Action callback)
        {
            return ToMethod(signal => callback());
        }

        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Action<TObject, TSignal> handler)
        {
            return ToMethod<TObject>(x => (Action<TSignal>)(s => handler(x, s)));
        }

        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Func<TObject, Action> handlerGetter)
        {
            return ToMethod<TObject>(x => (Action<TSignal>)(s => handlerGetter(x)()));
        }

        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Func<TObject, Action<TSignal>> handlerGetter)
        {
            return new BindSignalFromBinder<TObject, TSignal>(
                _signalBindInfo, _bindStatement, handlerGetter, _container);
        }
    }
}
