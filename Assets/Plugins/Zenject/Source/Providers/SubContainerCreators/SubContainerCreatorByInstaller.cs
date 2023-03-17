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
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class SubContainerCreatorByInstaller : ISubContainerCreator
    {
        readonly Type _installerType;
        readonly DiContainer _container;
        readonly List<TypeValuePair> _extraArgs;
        readonly SubContainerCreatorBindInfo _containerBindInfo;

        public SubContainerCreatorByInstaller(
            DiContainer container,
            SubContainerCreatorBindInfo containerBindInfo,
            Type installerType,
            IEnumerable<TypeValuePair> extraArgs)
        {
            _installerType = installerType;
            _container = container;
            _extraArgs = extraArgs.ToList();
            _containerBindInfo = containerBindInfo;

            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
        }

        public SubContainerCreatorByInstaller(
            DiContainer container,
            SubContainerCreatorBindInfo containerBindInfo,
            Type installerType)
            : this(container, containerBindInfo, installerType, new List<TypeValuePair>())
        {
        }

        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context, out Action injectAction)
        {
            var subContainer = _container.CreateSubContainer();

            SubContainerCreatorUtil.ApplyBindSettings(_containerBindInfo, subContainer);

            var extraArgs = ZenPools.SpawnList<TypeValuePair>();

            extraArgs.AllocFreeAddRange(_extraArgs);
            extraArgs.AllocFreeAddRange(args);

            var installer = (InstallerBase)subContainer.InstantiateExplicit(
                _installerType, extraArgs);

            ZenPools.DespawnList(extraArgs);

            installer.InstallBindings();

            injectAction = () => 
            {
                subContainer.ResolveRoots();
            };

            return subContainer;
        }
    }
}
