using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace ALARM
{
	///
    public abstract class LoadFunction : Function
    {
	    ///
        public virtual void Load(TagCompound tag) { }
    }
}
