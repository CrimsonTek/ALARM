using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.EnergyCore
{
    public static class EnergyUtils
    {
        public static Energy Max(Energy val1, Energy val2)
        {
            Energy energy1 = val1.ToLuna;
            Energy energy2 = val2.ToLuna;
            return energy1 > energy2 ? energy1 : energy2;
        }

        public static Energy Max(Energy val1, double val2)
        {
            return new Energy(val1.amount > val2 ? val1.amount : val2, val1.type);
        }

        public static Energy Max(double val1, Energy val2)
        {
            return new Energy(val1 > val2.amount ? val1 : val2.amount, val2.type);
        }

        public static Energy Min(Energy val1, Energy val2)
        {
            var energy1 = val1.ToLuna;
            var energy2 = val2.ToLuna;
            return energy1 < energy2 ? energy1 : energy2;
        }

        public static Energy Min(Energy val1, double val2)
        {
            return new Energy(val1.amount < val2 ? val1.amount : val2, val1.type);
        }

        public static Energy Min(double val1, Energy val2)
        {
            return new Energy(val1 < val2.amount ? val1 : val2.amount, val2.type);
        }
    }
}
