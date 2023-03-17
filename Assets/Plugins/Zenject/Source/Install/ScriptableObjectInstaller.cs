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

#if !NOT_UNITY3D

using ModestTree;
using UnityEngine;

namespace Zenject
{
    // We'd prefer to make this abstract but Unity 5.3.5 has a bug where references
    // can get lost during compile errors for classes that are abstract
    public class ScriptableObjectInstaller : ScriptableObjectInstallerBase
    {
    }

    //
    // Derive from this class instead to install like this:
    //     FooInstaller.InstallFromResource(Container);
    // Or
    //     FooInstaller.InstallFromResource("My/Path/ToScriptableObjectInstance", Container);
    //
    // (Instead of needing to add the ScriptableObjectInstaller directly via inspector)
    //
    // This approach is needed if you want to pass in strongly typed runtime parameters too it
    //
    public class ScriptableObjectInstaller<TDerived> : ScriptableObjectInstaller
        where TDerived : ScriptableObjectInstaller<TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container)
        {
            var installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.Inject(installer);
            installer.InstallBindings();
            return installer;
        }
    }

    public class ScriptableObjectInstaller<TParam1, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1)
        {
            var installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1));
            installer.InstallBindings();
            return installer;
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2)
        {
            var installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2));
            installer.InstallBindings();
            return installer;
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            var installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3));
            installer.InstallBindings();
            return installer;
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            var installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3, p4));
            installer.InstallBindings();
            return installer;
        }
    }

    public static class ScriptableObjectInstallerUtil
    {
        public static string GetDefaultResourcePath<TInstaller>()
            where TInstaller : ScriptableObjectInstallerBase
        {
            return "Installers/" + typeof(TInstaller).PrettyName();
        }

        public static TInstaller CreateInstaller<TInstaller>(
            string resourcePath, DiContainer container)
            where TInstaller : ScriptableObjectInstallerBase
        {
            var installers = Resources.LoadAll(resourcePath);

            Assert.That(installers.Length == 1,
                "Could not find unique ScriptableObjectInstaller with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);

            var installer = installers[0];

            Assert.That(installer is TInstaller,
                "Expected to find installer with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);

            return (TInstaller)installer;
        }
    }
}

#endif
