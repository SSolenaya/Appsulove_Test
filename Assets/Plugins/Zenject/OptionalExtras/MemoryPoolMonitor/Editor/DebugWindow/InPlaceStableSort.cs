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

namespace Zenject.MemoryPoolMonitor
{
    // Fastest known in place stable sort. No risk of exploding a stack. Cost: a relatively high number of moves. Stack can still be expensive too.
    // This is a merge sort with a smart in place merge that 'rotates' the sub arrays.
    // Taken from: http://thomas.baudel.name/Visualisation/VisuTri/inplacestablesort.html
    public class InPlaceStableSort<T>
    {
        static void Exchange(List<T> list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        static int Lower(List<T> list, Comparison<T> comparer, int from, int to, int val)
        {
            int len = to - from, half;
            while (len > 0)
            {
                half = len / 2;
                int mid = from + half;
                if (comparer(list[mid], list[val]) < 0)
                {
                    from = mid + 1;
                    len = len - half - 1;
                }
                else
                {
                    len = half;
                }
            }
            return from;
        }

        static int Upper(List<T> list, Comparison<T> comparer, int from, int to, int val)
        {
            int len = to - from, half;
            while (len > 0)
            {
                half = len / 2;
                int mid = from + half;
                if (comparer(list[val], list[mid]) < 0)
                {
                    len = half;
                }
                else
                {
                    from = mid + 1;
                    len = len - half - 1;
                }
            }
            return from;
        }

        static void InsertSort(List<T> list, Comparison<T> comparer, int from, int to)
        {
            if (to > from + 1)
            {
                for (int i = from + 1; i < to; i++)
                {
                    for (int j = i; j > from; j--)
                    {
                        if (comparer(list[j], list[j - 1]) < 0)
                        {
                            Exchange(list, j, j - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        static int Gcd(int m, int n)
        {
            while (n != 0)
            {
                int t = m % n;
                m = n;
                n = t;
            }
            return m;
        }

        static void Reverse(List<T> list, int from, int to)
        {
            while (from < to)
            {
                Exchange(list, from++, to--);
            }
        }

        static void Rotate(List<T> list, Comparison<T> comparer, int from, int mid, int to)
        {
            /*  a less sophisticated but costlier version:
                    Reverse(from, mid-1);
                    Reverse(mid, to-1);
                    Reverse(from, to-1);
                    */
            if (from == mid || mid == to)
            {
                return;
            }
            int n = Gcd(to - from, mid - from);
            while (n-- != 0)
            {
                T val = list[from + n];
                int shift = mid - from;
                int p1 = from + n, p2 = from + n + shift;
                while (p2 != from + n)
                {
                    list[p1] = list[p2];
                    p1 = p2;
                    if (to - p2 > shift)
                    {
                        p2 += shift;
                    }
                    else
                    {
                        p2 = from + (shift - (to - p2));
                    }
                }
                list[p1] = val;
            }
        }

        static void Merge(List<T> list, Comparison<T> comparer, int from, int pivot, int to, int len1, int len2)
        {
            if (len1 == 0 || len2 == 0)
            {
                return;
            }

            if (len1 + len2 == 2)
            {
                if (comparer(list[pivot], list[from]) < 0)
                {
                    Exchange(list, pivot, from);
                }

                return;
            }

            int first_cut, second_cut;
            int len11, len22;

            if (len1 > len2)
            {
                len11 = len1 / 2;
                first_cut = from + len11;
                second_cut = Lower(list, comparer, pivot, to, first_cut);
                len22 = second_cut - pivot;
            }
            else
            {
                len22 = len2 / 2;
                second_cut = pivot + len22;
                first_cut = Upper(list, comparer, from, pivot, second_cut);
                len11 = first_cut - from;
            }

            Rotate(list, comparer, first_cut, pivot, second_cut);
            int new_mid = first_cut + len22;
            Merge(list, comparer, from, first_cut, new_mid, len11, len22);
            Merge(list, comparer, new_mid, second_cut, to, len1 - len11, len2 - len22);
        }

        public static void Sort(List<T> list, Comparison<T> comparer, int from, int to)
        {
            if (to - from < 12)
            {
                InsertSort(list, comparer, from, to);
                return;
            }

            int middle = (from + to) / 2;
            Sort(list, comparer, from, middle);
            Sort(list, comparer, middle, to);
            Merge(list, comparer, from, middle, to, middle - from, to - middle);
        }

        public static void Sort(List<T> list, Comparison<T> comparer)
        {
            Sort(list, comparer, 0, list.Count);
        }
    };
}
