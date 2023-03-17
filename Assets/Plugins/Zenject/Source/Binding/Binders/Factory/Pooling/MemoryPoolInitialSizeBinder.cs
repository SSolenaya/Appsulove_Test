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

namespace Zenject
{
    [NoReflectionBaking]
    public class MemoryPoolMaxSizeBinder<TContract> : MemoryPoolExpandBinder<TContract>
    {
        public MemoryPoolMaxSizeBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolExpandBinder<TContract> WithMaxSize(int size)
        {
            MemoryPoolBindInfo.MaxSize = size;
            return this;
        }
    }

    [NoReflectionBaking]
    public class MemoryPoolInitialSizeMaxSizeBinder<TContract> : MemoryPoolMaxSizeBinder<TContract>
    {
        public MemoryPoolInitialSizeMaxSizeBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolMaxSizeBinder<TContract> WithInitialSize(int size)
        {
            MemoryPoolBindInfo.InitialSize = size;
            return this;
        }

        public FactoryArgumentsToChoiceBinder<TContract> WithFixedSize(int size)
        {
            MemoryPoolBindInfo.InitialSize = size;
            MemoryPoolBindInfo.MaxSize = size;
            MemoryPoolBindInfo.ExpandMethod = PoolExpandMethods.Disabled;
            return this;
        }
    }

    [NoReflectionBaking]
    public class MemoryPoolIdInitialSizeMaxSizeBinder<TContract> : MemoryPoolInitialSizeMaxSizeBinder<TContract>
    {
        public MemoryPoolIdInitialSizeMaxSizeBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolInitialSizeMaxSizeBinder<TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}

