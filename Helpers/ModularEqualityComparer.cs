using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Helpers
{
    ///
    public class ModularEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> hash;

        ///
        public ModularEqualityComparer()
        {
            equals = DefaultEquals;
            hash = DefaultGetHashCode;
        }

        ///
        public ModularEqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> hashFunc)
        {
            equals = equalsFunc ?? DefaultEquals;
            hash = hashFunc ?? DefaultGetHashCode;
        }

        private bool DefaultEquals(T x, T y)
        {
            return x.Equals(y);
        }

        ///
        public bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        private int DefaultGetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        ///
        public int GetHashCode(T obj)
        {
            return hash(obj);
        }
    }
}
