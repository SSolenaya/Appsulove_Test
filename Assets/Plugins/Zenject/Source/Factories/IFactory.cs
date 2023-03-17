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
    public interface IFactory
    {
    }

    public interface IFactory<out TValue> : IFactory
    {
        TValue Create();
    }

    public interface IFactory<in TParam1, out TValue> : IFactory
    {
        TValue Create(TParam1 param);
    }

    public interface IFactory<in TParam1, in TParam2, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9, in TParam10, out TValue> : IFactory
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10);
    }
}

