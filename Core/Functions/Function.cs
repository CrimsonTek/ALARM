using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM
{
    ///
    public abstract class Function
    {
        /// <summary>
        /// Your mod.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// The name of this function.
        /// </summary>
        public virtual string Name => DisplayNameAttribute.Default(GetType());

        /// <summary>
        /// This represents the priority of your function. Your function will be executed before other functions with a lower priority. Default ALARM Functions have a constant priority, which you can find in 4 static ID classes named below. You can use this information to place your functions before or after ALARM functions. <br/>
        /// - <see cref="UpdatePriorityID"/> <br/>
        /// - <see cref="InitializePriorityID"/> <br/>
        /// - <see cref="LoadPriorityID"/> <br/>
        /// - <see cref="SavePriorityID"/> <br/>
        /// </summary>
        public virtual float Priority { get; }
    }
}
