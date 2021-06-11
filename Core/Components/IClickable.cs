using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.Components
{
    /// <summary>
    /// Use this interface for Components which allow a machine to be clicked. Examples include any UI component, machines that do something when clicked, etc.
    /// </summary>
    public interface IClickable
    {
        /// <summary>
        /// The function called when clicked. Return true if this does something.
        /// </summary>
        /// <returns></returns>
        bool Click();
    }
}
