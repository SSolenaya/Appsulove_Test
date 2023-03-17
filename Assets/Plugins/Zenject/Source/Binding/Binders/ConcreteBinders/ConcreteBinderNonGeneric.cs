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
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class ConcreteBinderNonGeneric : FromBinderNonGeneric
    {
        public ConcreteBinderNonGeneric(
            DiContainer bindContainer, BindInfo bindInfo,
            BindStatement bindStatement)
            : base(bindContainer, bindInfo, bindStatement)
        {
            ToSelf();
        }

        // Note that this is the default, so not necessary to call
        public FromBinderNonGeneric ToSelf()
        {
            Assert.IsEqual(BindInfo.ToChoice, ToChoices.Self);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo, (container, type) => new TransientProvider(
                    type, container, BindInfo.Arguments, BindInfo.ContextInfo, BindInfo.ConcreteIdentifier,
                    BindInfo.InstantiatedCallback));

            return this;
        }

        public FromBinderNonGeneric To<TConcrete>()
        {
            return To(typeof(TConcrete));
        }

        public FromBinderNonGeneric To(params Type[] concreteTypes)
        {
            return To((IEnumerable<Type>)concreteTypes);
        }

        public FromBinderNonGeneric To(IEnumerable<Type> concreteTypes)
        {
            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes.Clear();
            BindInfo.ToTypes.AddRange(concreteTypes);

            if (BindInfo.ToTypes.Count > 1 && BindInfo.ContractTypes.Count > 1)
            {
                // Be more lenient in this case to behave similar to convention based bindings
                BindInfo.InvalidBindResponse = InvalidBindResponses.Skip;
            }
            else
            {
                BindingUtil.AssertIsDerivedFromTypes(concreteTypes, BindInfo.ContractTypes, BindInfo.InvalidBindResponse);
            }

            return this;
        }

#if !(UNITY_WSA && ENABLE_DOTNET)
        public FromBinderNonGeneric To(
            Action<ConventionSelectTypesBinder> generator)
        {
            var bindInfo = new ConventionBindInfo();

            // This is nice because it allows us to do things like Bind(all interfaces).To(specific types)
            // instead of having to do Bind(all interfaces).To(specific types that inherit from one of these interfaces)
            BindInfo.InvalidBindResponse = InvalidBindResponses.Skip;

            generator(new ConventionSelectTypesBinder(bindInfo));

            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes.Clear();
            BindInfo.ToTypes.AddRange(bindInfo.ResolveTypes());

            return this;
        }
#endif
    }
}
