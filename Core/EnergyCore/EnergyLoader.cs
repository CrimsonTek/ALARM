using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM.Core.EnergyCore
{
    /// <summary>
    /// Loads custom energy types, and allows them to be looked up.
    /// </summary>
    public static class EnergyLoader
    {
        /// <summary>
        /// A list of mod energies loaded by all mods. You may use this for enumerating, but do not modify it.
        /// </summary>
        private static List<ModEnergy> modEnergies = new List<ModEnergy>();
        internal static Dictionary<Tuple<Mod, string>, ModEnergy> modAndNameToEnergy = new Dictionary<Tuple<Mod, string>, ModEnergy>();
        internal static Dictionary<Type, ModEnergy> classToEnergy = new Dictionary<Type, ModEnergy>();

        /// <summary>
        /// The mod energies loaded.
        /// </summary>
        public static ModEnergy[] ModEnergies { get; set; }

        private static void Add(ModEnergy modEnergy, Mod mod, string name, Type @class)
        {
            modEnergies.Add(modEnergy);
            modAndNameToEnergy.Add(new Tuple<Mod, string>(mod, name), modEnergy);
            classToEnergy.Add(@class, modEnergy);
        }

        private static bool ContainsAny(Mod mod, string name, Type @class)
        {
            if (modAndNameToEnergy.ContainsKey(new Tuple<Mod, string>(mod, name))) return true;
            if (classToEnergy.ContainsKey(@class)) return true;
            return false;
        }

        private static void Clear()
        {
            modEnergies.Clear();
            modAndNameToEnergy.Clear();
            classToEnergy.Clear();
        }

        /// <summary>
        /// Gets the energy with the given <paramref name="name"/>. Returns -1 if none can be found.
        /// </summary>
        public static int GetEnergy(this Mod mod, string name)
        {
            if (modAndNameToEnergy.TryGetValue(new Tuple<Mod, string>(mod, name), out ModEnergy modEnergy))
            {
                return modEnergy.type;
            }
            return -1;
        }

        /// <summary>
        /// Gets the energy of the type <typeparamref name="T"/>. Returns -1 if none can be found.
        /// </summary>
        public static int GetEnergy<T>(this Mod mod)
        {
            if (classToEnergy.TryGetValue(typeof(T), out ModEnergy modEnergy))
            {
                return modEnergy.type;
            }
            return -1;
        }

        internal static void Load()
        {
            Clear();
            LoadALARM();

            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Name == nameof(ALARM)) continue;

                LoadMod(mod);
            }

            ModEnergies = modEnergies.ToArray();
        }

        private static void LoadALARM()
        {
            ALARM alarm = ModContent.GetInstance<ALARM>();
            LoadALARMEnergy(alarm, typeof(LunaEnergy), EnergyID.Luna);
        }
        private static void LoadMod(Mod mod)
        {
            Type[] typeArr = mod.Code?.GetTypes();

            if (typeArr == null)
                return;

            foreach (Type type in typeArr)
            {
                TryAddEnergyType(type, mod);
            }
        }

        private static ModEnergy TryAddEnergyType(Type type, Mod mod)
        {
            if (type.IsAbstract)
                return null;

            if (!typeof(ModEnergy).IsAssignableFrom(type))
                return null;

            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
                return null;

            ModEnergy energy = (ModEnergy)constructorInfo.Invoke(null);
            string name = energy.Name();
            Type @class = energy.GetType();

            energy.type = modEnergies.Count;
            energy.mod = mod;
            energy.name = name;
            if (ContainsAny(mod, name, @class))
            {
                return null;
            }

            Add(energy, mod, name, @class);
            energy.SetStaticDefaults();

            return energy;
        }

        private static ModEnergy LoadALARMEnergy(Mod mod, Type type, int expectedValue)
        {
            if (type.IsAbstract)
                throw new ArgumentException();

            if (!typeof(ModEnergy).IsAssignableFrom(type))
                throw new ArgumentException();

            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
                throw new ArgumentException();

            ModEnergy energy = (ModEnergy)constructorInfo.Invoke(null);
            string name = energy.Name();
            Type @class = energy.GetType();

            energy.type = modEnergies.Count;
            energy.mod = mod;
            energy.name = name;
            if (ContainsAny(mod, name, @class))
            {
                throw new ArgumentException();
            }

            if (modEnergies.Count != expectedValue)
                throw new ArgumentException();

            Add(energy, mod, name, @class);
            energy.SetStaticDefaults();

            return energy;
        }
    }
}
