using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM.Core.Components
{
    /// <summary>
    /// This class represents the loaded data for a given component type.
    /// </summary>
    public class ComponentEntry : IEquatable<ComponentEntry>
    {
        /// <summary>
        /// The mod this component type belongs to.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// The name of this component type.
        /// </summary>
        public string name;

        /// <summary>
        /// The <see cref="Type"/> of this component type.
        /// </summary>
        public Type classType;

        /// <summary>
        /// The unique ID of this component type.
        /// </summary>
        public int type;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ComponentEntry(Mod mod, string name, int type, Type classType)
        {
            this.mod = mod;
            this.name = name;
            this.classType = classType;
            this.type = type;
        }

        /// <summary>
        /// Clone.
        /// </summary>
        public ComponentEntry Clone()
        {
            return new ComponentEntry(mod, name, type, classType);
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

            if (obj is ComponentEntry entry)
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
        public bool Equals(ComponentEntry other)
        {
            return classType == other.classType
                && mod == other.mod
                && name == other.name
                && type == other.type;
        }

        /// <summary>
        /// ((classType, mod, name, type).GetHashCode());
        /// </summary>
        public override int GetHashCode()
        {
            return (classType, mod, name, type).GetHashCode();
        }
    }
}
