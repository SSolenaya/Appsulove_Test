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
using System.Reflection;
using System.Text;

namespace ModestTree
{
    public static class TypeStringFormatter
    {
        static readonly Dictionary<Type, string> _prettyNameCache = new Dictionary<Type, string>();

        public static string PrettyName(this Type type)
        {
            string prettyName;

            if (!_prettyNameCache.TryGetValue(type, out prettyName))
            {
                prettyName = PrettyNameInternal(type);
                _prettyNameCache.Add(type, prettyName);
            }

            return prettyName;
        }

        static string PrettyNameInternal(Type type)
        {
            var sb = new StringBuilder();

            if (type.IsNested)
            {
                sb.Append(type.DeclaringType.PrettyName());
                sb.Append(".");
            }

            if (type.IsArray)
            {
                sb.Append(type.GetElementType().PrettyName());
                sb.Append("[]");
            }
            else
            {
                var name = GetCSharpTypeName(type.Name);

                if (type.IsGenericType())
                {
                    var quoteIndex = name.IndexOf('`');

                    if (quoteIndex != -1)
                    {
                        sb.Append(name.Substring(0, name.IndexOf('`')));
                    }
                    else
                    {
                        sb.Append(name);
                    }

                    sb.Append("<");

                    if (type.IsGenericTypeDefinition())
                    {
                        var numArgs = type.GenericArguments().Count();

                        if (numArgs > 0)
                        {
                            sb.Append(new String(',', numArgs - 1));
                        }
                    }
                    else
                    {
                        sb.Append(string.Join(", ", type.GenericArguments().Select(t => t.PrettyName()).ToArray()));
                    }

                    sb.Append(">");
                }
                else
                {
                    sb.Append(name);
                }
            }

            return sb.ToString();
        }

        static string GetCSharpTypeName(string typeName)
        {
            switch (typeName)
            {
                case "String":
                case "Object":
                case "Void":
                case "Byte":
                case "Double":
                case "Decimal":
                    return typeName.ToLower();
                case "Int16":
                    return "short";
                case "Int32":
                    return "int";
                case "Int64":
                    return "long";
                case "Single":
                    return "float";
                case "Boolean":
                    return "bool";
                default:
                    return typeName;
            }
        }
    }
}

