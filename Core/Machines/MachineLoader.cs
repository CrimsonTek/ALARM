using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM.Core.Machines
{
    /// <summary>
    /// This class loads other mods' machines. This class can also be used to lookup the index of other mods' machines. 
    /// If you want to add machines, use MachineLoader.AddMachine().
    /// </summary>
    public static class MachineLoader
    {
        /// <summary>
        /// Stores all machineEnties.
        /// </summary>
        internal static List<MachineEntry> machineEntries = new List<MachineEntry>();
        /// <summary>
        /// This takes the mod and the name of a machine and gets the MachineEntry associated with it.
        /// </summary>
        internal static Dictionary<Tuple<Mod, string>, MachineEntry> nameToEntry = new Dictionary<Tuple<Mod, string>, MachineEntry>();
        /// <summary>
        /// Takes the system type and returns a MachineEntry.
        /// </summary>
        internal static Dictionary<Type, MachineEntry> classTypeToEntry = new Dictionary<Type, MachineEntry>();

        /// <summary>
        /// An array of copies of all loaded <see cref="MachineEntry"/>, the class used for storing information about types of machines.
        /// </summary>
        public static MachineEntry[] MachineEntries
        {
            get
            {
                MachineEntry[] arr = new MachineEntry[machineEntries.Count];
                for (int i = 0; i < machineEntries.Count; i++)
                {
                    arr[i] = machineEntries[i].Clone();
                }
                return arr;
            }
        }

        /// <summary>
        /// This clears all the dictionaries.
        /// </summary>
        internal static void Clear()
        {
            machineEntries.Clear();
            nameToEntry.Clear();
            classTypeToEntry.Clear();
        }

        /// <summary>
        /// Adds the given value to all dictionaries.
        /// </summary>
        internal static void Add(MachineEntry machineEntry, Tuple<Mod, string> modAndName, Type classType)
        {
            machineEntries.Add(machineEntry);
            nameToEntry.Add(modAndName, machineEntry);
            classTypeToEntry.Add(classType, machineEntry);
        }

        internal static bool ContainsAny(Tuple<Mod, string> modAndName, Type classType)
        {
            if (nameToEntry.ContainsKey(modAndName)) return true;
            if (classTypeToEntry.ContainsKey(classType)) return true;
            return false;
        }

        #region Get Functions
        /// <summary>
        /// This returns the <see cref="Type"/> of a machine using it's index. If no machine for this type exists, returns null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetMachineClassType(int type)
        {
            if (type >= 0 && type < machineEntries.Count)
            {
                return machineEntries[type].GetType();
            }
            return null;
        }

        /// <summary>
        /// Gets the type of the machine with the given name. Returns -1 if none are found.
        /// </summary>
        public static int GetMachine(this Mod mod, string name)
        {
            if (nameToEntry.TryGetValue(new Tuple<Mod, string>(mod, name), out MachineEntry entry))
            {
                return entry.type;
            }
            return -1;
        }

        /// <summary>
        /// Returns the type of the class type if one can be found. If none are found, returns -1.
        /// </summary>
        public static int GetMachine<T>(this Mod mod) where T : Machine => GetMachine(typeof(T));

        /// <summary>
        /// Returns the type of the class type if one can be found. If none are found, returns -1.
        /// </summary>
        public static int GetMachine<T>() where T : Machine => GetMachine(typeof(T));

        /// <summary>
        /// Returns the type of the class type if one can be found. If none are found, returns -1.
        /// </summary>
        public static int GetMachine(Type classType)
        {
            if (classTypeToEntry.TryGetValue(classType, out MachineEntry entry))
            {
                return entry.type;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        /// <summary>
        /// This function initializes and registers other mods' machines.
        /// </summary>
        internal static void Load()
        {
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod == null) continue;
                if (mod.Code == null) continue;

                foreach (Type classType in mod.Code.GetTypes())
                {
                    if (!typeof(Machine).IsAssignableFrom(classType)) continue;

                    string name;
                    if (ReflectionCache.TryGetMethodWithAttribute(classType, null, typeof(string), out MethodInfo methodInfo, out DisplayNameAttribute _))
                    {
                        name = (string)methodInfo.Invoke(null, null);
                    }
                    else
                    {
                        name = DisplayNameAttribute.Default(classType);
                    }

                    AddMachine(mod, name, classType);
                }
            }
        }

        // note: should this be accessible to users? if so, why?
        internal static void AddMachine<T>(Mod mod, string name) where T : Machine => AddMachine(mod, name, typeof(T));

        // note: should this be accessible to users? if so, why?
        internal static void AddMachine(Mod mod, string name, Type classType)
        {
            int type = machineEntries.Count;
            MachineEntry entry = new MachineEntry(mod, name, classType, type);
            Tuple<Mod, string> modAndName = new Tuple<Mod, string>(mod, name);
            if (!ContainsAny(modAndName, classType))
            {
                Add(entry, modAndName, classType);
            }
            else
            {
                throw new ArgumentException($"{entry} could not be added because a key it uses has already been added.");
            }
        }
    }
}
