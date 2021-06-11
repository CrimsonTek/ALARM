using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace ALARM
{
	///
    public abstract class SaveFunction : Function
    {
        /// <summary>
        /// This will save your data for you, and it will be stored under your mod. Data from all of your <see cref="SaveFunction"/> Saves will be concated, so be careful not to reuse keys.
        /// </summary>
        /// <returns></returns>
        public virtual TagCompound Save() => null;
    }
}
