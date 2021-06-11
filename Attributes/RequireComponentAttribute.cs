using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Components;

namespace ALARM.Attributes
{
    /// <summary>
    /// This attribute will cause a <see cref="RequiredComponentMissingException"/> if there is no sibling component of the given type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class RequireComponentAttribute : Attribute
    {
        /// <summary>
        /// The required Type.
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The type of component required. Use "typeof([ClassName])" to pass the type as a variable.</param>
        public RequireComponentAttribute(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
