using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.Components
{
    ///
    [Serializable]
    public class RequiredComponentMissingException : Exception
    {
        ///
        public RequiredComponentMissingException()
            : base($"Required component could not be found.") { }

        ///
        public RequiredComponentMissingException(string message)
            : base(message ?? "Required component could not be found.") { }

        ///
        public RequiredComponentMissingException(string componentName, string message)
            : base($"{message ?? "Required component could not be found."} (Component '{componentName}')") { }

        ///
        public RequiredComponentMissingException(string message, Exception inner) 
            : base(message ?? "Required component could not be found.", inner) { }

        ///
        public RequiredComponentMissingException(string componentName, string message, Exception inner)
            : base($"{message ?? "Required component could not be found."} (Component '{componentName}')", inner) { }

    }
}
