using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ALARM.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ALARM.Core.Cables
{
    /// <summary>
    /// Loads and creates users' custom cables.
    /// </summary>
    public static class CableLoader
    {
        public static CableEntry[] cableEntries;

        internal static void Load()
        {
            Mod[] mods = ModLoader.Mods;
            List<CableEntry> entryList = new List<CableEntry>();
            foreach (Mod mod in mods)
            {
                LoadMod(mod, entryList);
            }
            cableEntries = entryList.ToArray();
        }

        private static void LoadMod(Mod mod, List<CableEntry> entryList)
        {
            Type[] typeArr = mod?.Code?.GetTypes();

            if (typeArr == null)
                return;

            foreach (Type type in typeArr)
            {
                if (type.IsAbstract)
                    continue;

                if (!typeof(Cable).IsAssignableFrom(type))
                    continue;

                ConstructorInfo constructorInfo = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault((c) => c.GetParameters().Length == 0);
                if (constructorInfo == null)
                    continue;

                string name = DisplayNameAttribute.Default(type); // todo: look for DisplayNameAttribute

                TextureNameAttribute textureNameAttr = type.GetCustomAttribute<TextureNameAttribute>(false);
                string textureName = textureNameAttr != null ? textureNameAttr.TextureName : GetDefaultTextureName(type);
                Texture2D texture2D = ModContent.GetTexture(textureName);

                CableEntry entry = new CableEntry(mod, name, texture2D, entryList.Count);
                entryList.Add(entry);
            }
        }

        private static string GetDefaultTextureName(Type type)
        {
            string fullName = type.FullName;
            return $"{fullName.Replace(".", "/")}";
        }
    }
}
