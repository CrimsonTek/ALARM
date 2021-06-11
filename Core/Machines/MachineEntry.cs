using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM
{
    /// <summary>
    /// The class used to store information about types of machines.
    /// </summary>
    public class MachineEntry : IEquatable<MachineEntry>
    {
        /// <summary>
        /// The mod this type of machine belongs to.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// The name of this type of machine.
        /// </summary>
        public string name;

        /// <summary>
        /// The <see cref="Type"/> of this type of machine.
        /// </summary>
        public Type classType;

        /// <summary>
        /// The unique ID of this type of machine.
        /// </summary>
        public int type;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MachineEntry(Mod mod, string name, Type classType, int type)
        {
            this.mod = mod;
            this.name = name;
            this.classType = classType;
            this.type = type;
        }

        /// <summary>
        /// Clone.
        /// </summary>
        public MachineEntry Clone()
        {
            return new MachineEntry(mod, name, classType, type);
        }

        /// <summary>
        /// Compares fields.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is MachineEntry entry)
            {
                return Equals(entry);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compares fields.
        /// </summary>
        public bool Equals(MachineEntry other)
        {
            return classType == other.classType
                && mod == other.mod
                && name == other.name
                && type == other.type;
        }

        /// <summary>
        /// (classType, mod, name, type).GetHashCode();
        /// </summary>
        public override int GetHashCode()
        {
            return (classType, mod, name, type).GetHashCode();
        }
    }
}
