using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.EnergyCore
{
    /// <summary>
    /// The default energy type.
    /// </summary>
    public class LunaEnergy : ModEnergy
    {
        /// <summary>
        /// Overriding the default name.
        /// </summary>
        public override string Name()
        {
            return "Luna";
        }

        /// <summary>
        /// Returns <paramref name="energy"/>.
        /// </summary>
        public override double ConvertToLuna(double energy) => energy;

        /// <summary>
        /// Returns <paramref name="luna"/>.
        /// </summary>
        public override double ConvertFromLuna(double luna) => luna;
    }
}
