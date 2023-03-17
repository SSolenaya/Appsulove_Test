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
    public static class SignalExtensions
    {
        public static SignalDeclarationBindInfo CreateDefaultSignalDeclarationBindInfo(DiContainer container, Type signalType)
        {
            return new SignalDeclarationBindInfo(signalType)
            {
                RunAsync = container.Settings.Signals.DefaultSyncMode == SignalDefaultSyncModes.Asynchronous,
                MissingHandlerResponse = container.Settings.Signals.MissingHandlerDefaultResponse,
                TickPriority = container.Settings.Signals.DefaultAsyncTickPriority
            };
        }

        public static DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal(this DiContainer container, Type type)
        {
            var signalBindInfo = CreateDefaultSignalDeclarationBindInfo(container, type);

            var bindInfo = container.Bind<SignalDeclaration>().AsCached()
                .WithArguments(signalBindInfo).WhenInjectedInto(typeof(SignalBus), typeof(SignalDeclarationAsyncInitializer)).BindInfo;

            var signalBinder = new DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder(signalBindInfo);
            signalBinder.AddCopyBindInfo(bindInfo);
            return signalBinder;
        }

        public static DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal>(this DiContainer container)
        {
            return container.DeclareSignal(typeof(TSignal));
        }

        public static BindSignalIdToBinder<TSignal> BindSignal<TSignal>(this DiContainer container)
        {
            var signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));

            return new BindSignalIdToBinder<TSignal>(container, signalBindInfo);
        }
    }
}

