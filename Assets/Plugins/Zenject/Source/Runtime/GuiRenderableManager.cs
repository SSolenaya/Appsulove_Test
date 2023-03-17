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
using ModestTree.Util;

namespace Zenject
{
    // See comment in IGuiRenderable.cs for usage
    public class GuiRenderableManager
    {
        List<RenderableInfo> _renderables;

        public GuiRenderableManager(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<IGuiRenderable> renderables,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ValuePair<Type, int>> priorities)
        {
            _renderables = new List<RenderableInfo>();

            foreach (var renderable in renderables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = priorities
                    .Where(x => renderable.GetType().DerivesFromOrEqual(x.First))
                    .Select(x => x.Second).ToList();

                int priority = matches.IsEmpty() ? 0 : matches.Distinct().Single();

                _renderables.Add(
                    new RenderableInfo(renderable, priority));
            }

            _renderables = _renderables.OrderBy(x => x.Priority).ToList();

#if UNITY_EDITOR
            foreach (var renderable in _renderables.Select(x => x.Renderable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate IGuiRenderable with type '{0}'".Fmt(renderable.GetType()));
            }
#endif
        }

        public void OnGui()
        {
            foreach (var renderable in _renderables)
            {
                try
                {
#if ZEN_INTERNAL_PROFILING
                    using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
#if UNITY_EDITOR
                    using (ProfileBlock.Start("{0}.GuiRender()", renderable.Renderable.GetType()))
#endif
                    {
                        renderable.Renderable.GuiRender();
                    }
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while calling {0}.GuiRender", renderable.Renderable.GetType());
                }
            }
        }

        class RenderableInfo
        {
            public IGuiRenderable Renderable;
            public int Priority;

            public RenderableInfo(IGuiRenderable renderable, int priority)
            {
                Renderable = renderable;
                Priority = priority;
            }
        }
    }
}
