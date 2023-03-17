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
#if !NOT_UNITY3D
using JetBrains.Annotations;
#endif

namespace Zenject
{
    // Zero parameters
    public class PlaceholderFactory<TValue> : PlaceholderFactoryBase<TValue>, IFactory<TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create()
        {
            return CreateInternal(new List<TypeValuePair>());
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield break; }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TValue> : PlaceholderFactory<TValue>
    {
    }

    // One parameter
    public class PlaceholderFactory<TParam1, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield return typeof(TParam1); }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TValue> : PlaceholderFactory<TParam1, TValue>
    {
    }

    // Two parameters
    public class PlaceholderFactory<TParam1, TParam2, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TValue> : PlaceholderFactory<TParam1, TParam2, TValue>
    {
    }

    // Three parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TValue>
    {
    }

    // Four parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TValue>
        : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
    }

    // Five parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
    }

    // Six parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5),
                    InjectUtil.CreateTypePair(param6)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
    {
    }

    // Ten parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
    {
        // If you were hoping to override this method, use BindFactory<>.ToFactory instead
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5),
                    InjectUtil.CreateTypePair(param6),
                    InjectUtil.CreateTypePair(param7),
                    InjectUtil.CreateTypePair(param8),
                    InjectUtil.CreateTypePair(param9),
                    InjectUtil.CreateTypePair(param10)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
                yield return typeof(TParam7);
                yield return typeof(TParam8);
                yield return typeof(TParam9);
                yield return typeof(TParam10);
            }
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
        : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
    {
    }
}

