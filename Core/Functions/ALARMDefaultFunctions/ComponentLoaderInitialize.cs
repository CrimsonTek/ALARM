using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Components;

namespace ALARM.Core.Functions.ALARMDefaultFunctions
{
    internal class ComponentLoaderInitialize : InitializeFunction
    {
        public override void Initialize()
        {
            ComponentLoader.Load();
        }
    }
}
