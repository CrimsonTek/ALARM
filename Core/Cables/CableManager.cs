using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.EnergyCore;

namespace ALARM.Core.Cables
{
    public static class CableManager
    {
        public static Dictionary<(int, int), Cable> cables = new Dictionary<(int, int), Cable>();

        public static Cable GetCable(int i, int j)
        {
            throw new NotImplementedException();
        }

        public static EnergyConnection[] FindConnectedMachines()
        {
            throw new NotImplementedException();
        }
    }
}
