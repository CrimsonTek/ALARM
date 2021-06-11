using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Machines;
using Terraria.ModLoader;

namespace ALARM.Testing
{
    class TileTest : ModTile
    {
        public override bool NewRightClick(int i, int j)
        {
            return Machine.GetAt(i, j).Click();
        }
    }
}
