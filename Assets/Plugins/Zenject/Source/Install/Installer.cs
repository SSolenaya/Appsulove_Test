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
    //
    // I'd recommmend using Installer<> instead, and then always use the approach
    // of calling `MyInstaller.Install(Container)`
    // This way, if you want to add strongly typed parameters later you can do this
    // by deriving from a different Installer<> base class
    //
    public abstract class Installer : InstallerBase
    {
    }

    //
    // Derive from this class then install like this:
    //     FooInstaller.Install(Container);
    //
    public abstract class Installer<TDerived> : InstallerBase
        where TDerived : Installer<TDerived>
    {
        public static void Install(DiContainer container)
        {
            container.Instantiate<TDerived>().InstallBindings();
        }
    }

    // Use these versions to pass parameters to your installer

    public abstract class Installer<TParam1, TDerived> : InstallerBase
        where TDerived : Installer<TParam1, TDerived>
    {
        public static void Install(DiContainer container, TParam1 p1)
        {
            container.InstantiateExplicit<TDerived>(
                InjectUtil.CreateArgListExplicit(p1)).InstallBindings();
        }
    }

    public abstract class Installer<TParam1, TParam2, TDerived> : InstallerBase
        where TDerived : Installer<TParam1, TParam2, TDerived>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2)
        {
            container.InstantiateExplicit<TDerived>(
                InjectUtil.CreateArgListExplicit(p1, p2)).InstallBindings();
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TDerived> : InstallerBase
        where TDerived : Installer<TParam1, TParam2, TParam3, TDerived>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            container.InstantiateExplicit<TDerived>(
                InjectUtil.CreateArgListExplicit(p1, p2, p3)).InstallBindings();
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TParam4, TDerived> : InstallerBase
        where TDerived : Installer<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            container.InstantiateExplicit<TDerived>(
                InjectUtil.CreateArgListExplicit(p1, p2, p3, p4)).InstallBindings();
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived> : InstallerBase
        where TDerived : Installer<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>
    {
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            container.InstantiateExplicit<TDerived>(
                InjectUtil.CreateArgListExplicit(p1, p2, p3, p4, p5)).InstallBindings();
        }
    }
}
