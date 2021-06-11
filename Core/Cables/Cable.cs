using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Connections;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ALARM.Core.Cables
{
    public abstract class Cable
    {
        public int type;

        public string name;

        public Mod mod;

        /// <summary>
        /// The texture given to this cable.
        /// </summary>
        public Texture2D texture2D;

        public bool IsConnected(Side side)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows you to draw things behind this cable. Return false to stop ALARM from drawing this cable normally. <para/>
        /// Returns true by default.
        /// </summary>
        public virtual bool PreDraw(SpriteBatch spriteBatch) => true;

        /// <summary>
        /// Allows you to draw things in from of this cable. This can also be used to things such as creating dust.
        /// </summary>
        public virtual void PostDraw(SpriteBatch spriteBatch) { }
    }
}
