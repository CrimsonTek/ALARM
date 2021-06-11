using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM
{
    /// <summary>
    /// This class contains many helpful extensions for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2>(this List<(T1, T2)> list, T1 item1, T2 item2) => list.Add((item1, item2));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3>(this List<(T1, T2, T3)> list, T1 item1, T2 item2, T3 item3) => list.Add((item1, item2, item3));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3, T4>(this List<(T1, T2, T3, T4)> list, T1 item1, T2 item2, T3 item3, T4 item4) => list.Add((item1, item2, item3, item4));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3, T4, T5>(this List<(T1, T2, T3, T4, T5)> list, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => list.Add((item1, item2, item3, item4, item5));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3, T4, T5, T6>(this List<(T1, T2, T3, T4, T5, T6)> list, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => list.Add((item1, item2, item3, item4, item5, item6));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3, T4, T5, T6, T7>(this List<(T1, T2, T3, T4, T5, T6, T7)> list, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => list.Add((item1, item2, item3, item4, item5, item6, item7));
        /// <summary>An easier add method for adding value tuples to lists.</summary>
        public static void Add<T1, T2, T3, T4, T5, T6, T7, T8>(this List<(T1, T2, T3, T4, T5, T6, T7, T8)> list, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => list.Add((item1, item2, item3, item4, item5, item6, item7, item8));

        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2>(this List<(T1, T2)> list, int index, T1 item1, T2 item2) => list.Insert(index, (item1, item2));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3>(this List<(T1, T2, T3)> list, int index, T1 item1, T2 item2, T3 item3) => list.Insert(index, (item1, item2, item3));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3, T4>(this List<(T1, T2, T3, T4)> list, int index, T1 item1, T2 item2, T3 item3, T4 item4) => list.Insert(index, (item1, item2, item3, item4));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3, T4, T5>(this List<(T1, T2, T3, T4, T5)> list, int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => list.Insert(index, (item1, item2, item3, item4, item5));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3, T4, T5, T6>(this List<(T1, T2, T3, T4, T5, T6)> list, int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => list.Insert(index, (item1, item2, item3, item4, item5, item6));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3, T4, T5, T6, T7>(this List<(T1, T2, T3, T4, T5, T6, T7)> list, int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => list.Insert(index, (item1, item2, item3, item4, item5, item6, item7));
        /// <summary>An easier insert method for inserting tuples to tuple lists.</summary>
        public static void Insert<T1, T2, T3, T4, T5, T6, T7, T8>(this List<(T1, T2, T3, T4, T5, T6, T7, T8)> list, int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => list.Insert(index, (item1, item2, item3, item4, item5, item6, item7, item8));
    }
}
