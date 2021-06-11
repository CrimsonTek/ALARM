using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TextureNameAttribute : Attribute
    {
        public string TextureName { get; }

        public TextureNameAttribute(string textureName)
        {
            this.TextureName = textureName;
        }
    }
}
