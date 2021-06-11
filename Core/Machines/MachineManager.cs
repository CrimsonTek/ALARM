using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ALARM.Attributes;
using ALARM.Core.Components;

namespace ALARM.Core.Machines
{
    /// <summary>
    /// This class contains all the machines that exist in the world.
    /// </summary>
    public static class MachineManager
    {
        /// <summary>
        /// This is the list of all machines.
        /// </summary>
        public static List<Machine> machines;

        /// <summary>
        /// This function updates all machines in the world and their components.
        /// </summary>
        public static void Update()
        {
            foreach (Machine machine in machines)
            {
                UpdateMachine(machine);
            }
        }

        /// <summary>
        /// Updates the machine.
        /// </summary>
        private static void UpdateMachine(Machine machine)
        {
            try
            {
                foreach (Component toRunStartOn in machine.componentsToRunStartOn)
                {
                    RequireComponentAttribute requiredCompAttr = toRunStartOn.GetType().GetCustomAttribute<RequireComponentAttribute>();
                    if (requiredCompAttr != null)
                    {
                        Component foundComp = machine.GetComponent(requiredCompAttr.type);
                        if (foundComp == null)
                        {
                            throw new RequiredComponentMissingException(requiredCompAttr.type.Name);
                        }
                    }

                    toRunStartOn.Start();
                }
            }
            catch (InvalidOperationException invalidOperationException) // this is the exception thrown when an enumerable is modified during enumeration
            {
                throw new InvalidOperationException($"Do not add or remove components in {nameof(Component.Start)}", invalidOperationException);
            }
            machine.componentsToRunStartOn.Clear();

            foreach (Component component in machine.components)
            {
                if (machine.PreUpdateComponent(component))
                {
                    component.Update();
                }
            }

            machine.Update();
        }

        /// <summary>
        /// Gets the machine if there's one at the given tile coordinates, null if there isn't.
        /// </summary>
        public static Machine GetMachine(int i, int j)
        {
            foreach (Machine machine in machines)
            {
                if (machine.i > i || i >= machine.i + machine.width)
                {
                    continue;
                }

                if (machine.j > j || j >= machine.j + machine.height)
                {
                    continue;
                }

                return machine;
            }

            return null;
        }
    }
}
