using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM.Core.Components
{
    /// <summary>
    /// This class loads your mod's components and stores information about them.
    /// </summary>
    public static class ComponentLoader
    {
        internal static List<ComponentEntry> componentEntries = new List<ComponentEntry>();
        internal static Dictionary<Tuple<Mod, string>, ComponentEntry> modAndNameToEntry = new Dictionary<Tuple<Mod, string>, ComponentEntry>();
        internal static Dictionary<Type, ComponentEntry> classTypeToEntry = new Dictionary<Type, ComponentEntry>();

        /// <summary>
        /// Returns an array of clones of <see cref="ComponentEntry"/>, the class used for storing data about component types.
        /// </summary>
        public static ComponentEntry[] ComponentEntries
        {
            get
            {
                ComponentEntry[] arr = new ComponentEntry[componentEntries.Count];
                for (int i = 0; i < componentEntries.Count; i++)
                {
                    arr[i] = componentEntries[i].Clone();
                }
                return arr;
            }
        }

        internal static void Add(ComponentEntry entry, Mod mod, string name, Type classType)
        {
            componentEntries.Add(entry);
            modAndNameToEntry.Add(new Tuple<Mod, string>(mod, name), entry);
            classTypeToEntry.Add(classType, entry);
        }

        internal static bool ContainsAny(Mod mod, string name, Type classType)
        {
            if (modAndNameToEntry.ContainsKey(new Tuple<Mod, string>(mod, name))) return true;
            if (classTypeToEntry.ContainsKey(classType)) return true;
            return false;
        }

        internal static void Clear()
        {
            componentEntries.Clear();
            modAndNameToEntry.Clear();
            classTypeToEntry.Clear();
        }

        internal static int currentType = 0;
        /// <summary>The number of components loaded.</summary>
        [Obsolete]
        public static int ComponentCount => currentType;

        internal static void Load()
        {
            Clear();
            Mod[] mods = ModLoader.Mods;
            foreach (Mod mod in mods)
            {
                Type[] typeArr = mod.Code?.GetTypes();
                if (typeArr == null)
                {
                    continue;
                }

                foreach (Type type in typeArr)
                {
                    if (!typeof(Component).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo == null)
                    {
                        continue;
                    }

                    string name;
                    if (ReflectionCache.TryGetMethodWithAttribute(type, null, typeof(string), out MethodInfo info, out DisplayNameAttribute _))
                    {
                        name = (string)info.Invoke(null, null);
                    }
                    else
                    {
                        name = DisplayNameAttribute.Default(type);
                    }

                    AddComponent(mod, name, type);
                }
            }
        }

        /// <summary>
        /// Gets the component type with the name <paramref name="name"/>. Returns -1 if none are found.
        /// </summary>
        public static int GetComponent(this Mod mod, string name)
        {
            if (modAndNameToEntry.TryGetValue(new Tuple<Mod, string>(mod, name), out ComponentEntry componentEntry))
            {
                return componentEntry.type;
            }
            return -1;
        }

        /// <summary>
        /// Gets the component of the type <typeparamref name="T"/>. Returns -1 if none are found.
        /// </summary>
        public static int GetComponent<T>(this Mod mod) where T : Component => GetComponent<T>();

        /// <summary>
        /// Gets the component of the type <typeparamref name="T"/>. Returns -1 if none are found.
        /// </summary>
        public static int GetComponent<T>() where T : Component
        {
            if (classTypeToEntry.TryGetValue(typeof(T), out ComponentEntry componentEntry))
            {
                return componentEntry.type;
            }
            return -1;
        }

        // TODO: Should this be allowed?
        /// <summary>
        /// This can be used to manually add components.
        /// </summary>
        public static void AddComponent<T>(Mod mod, string name) => AddComponent(mod, name, typeof(T));
        /// <summary>
        /// This can be used to manually add components.
        /// </summary>
        public static void AddComponent(Mod mod, string name, Type classType)
        {
            ComponentEntry entry = new ComponentEntry(mod, name, currentType, classType);
            if (!ContainsAny(mod, name, classType))
            {
                Add(entry, mod, name, classType);
                currentType++;
            }
            else
            {
                throw new ArgumentException($"{entry} could not be added because a key it uses has already been added.");
            }
        }
    }
}
