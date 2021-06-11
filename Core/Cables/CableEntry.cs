using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ALARM.Core.Cables
{
    public class CableEntry
    {
        public readonly Mod mod;

        public readonly string name;

        public readonly Texture2D texture2D;

        public int type;

        public CableEntry(Mod mod, string name, Texture2D texture2D, int index)
        {
            this.mod = mod;
            this.name = name;
            this.texture2D = texture2D;
            type = index;
        }
    }
}
