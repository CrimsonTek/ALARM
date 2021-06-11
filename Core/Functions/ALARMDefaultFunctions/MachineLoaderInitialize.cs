using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Machines;

namespace ALARM.Core.Functions.ALARMDefaultFunctions
{
    [Obsolete]
    internal class MachineLoaderInitialize : InitializeFunction
    {
        public override float Priority => InitializePriorityID.MachineLoaderInitializePriority;

        public override void Initialize()
        {
            MachineLoader.Load();
        }
    }
}
