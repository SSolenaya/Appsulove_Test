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
    // This is installed by default in ProjectContext, however, if you are using Zenject outside
    // of Unity then you might want to call this
    //
    // In this case though, you will have to manually call InitializableManager.Initialize,
    // DisposableManager.Dispose, TickableManager.Tick, etc. when appropriate for the environment
    // you are working in
    //
    // You might also want to use this installer in a ZenjectUnitTestFixture
    public class ZenjectManagersInstaller : Installer<ZenjectManagersInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(TickableManager), typeof(InitializableManager), typeof(DisposableManager))
                .ToSelf().AsSingle().CopyIntoAllSubContainers();
        }
    }
}

