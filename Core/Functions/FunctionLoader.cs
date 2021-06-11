using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ALARM
{
    /// <summary>
    /// This class acts as a loader for mods' 4 Functions:<br/>
    /// - <see cref="UpdateFunction"/><br/> (for world updates)
    /// - <see cref="InitializeFunction"/><br/> (for initializing mods)
    /// - <see cref="LoadFunction"/><br/> (for loading machines, components, etc)
    /// - <see cref="SaveFunction"/><br/> (for saving machines, components, etc)
    /// </summary>
    public static class FunctionLoader
    {
        internal static UpdateFunction[] updateFuncs = new UpdateFunction[0];
        internal static InitializeFunction[] initFuncs = new InitializeFunction[0];
        internal static LoadFunction[] loadFuncs = new LoadFunction[0];
        internal static SaveFunction[] saveFuncs = new SaveFunction[0];

        /// <summary>Initializes Functions using reflection.</summary>
        internal static void Setup()
        {
            List<UpdateFunction> ups = new List<UpdateFunction>();
            List<InitializeFunction> inits = new List<InitializeFunction>();
            List<LoadFunction> loads = new List<LoadFunction>();
            List<SaveFunction> saves = new List<SaveFunction>();

            for (int i = 0; i < ModLoader.Mods.Length; i++)
            {
                Mod mod = ModLoader.Mods[i];
                Type[] typeArr = mod.Code?.GetTypes() ?? Type.EmptyTypes;
                for (int j = 0; j < typeArr.Length; j++)
                {
                    Type type = typeArr[j];
                    ConstructorInfo emptyCtor = type.GetConstructor(Type.EmptyTypes);
                    if (type.IsAbstract || emptyCtor == null)
                        continue;

                    if (typeof(UpdateFunction).IsAssignableFrom(type))
                    {
                        UpdateFunction updateFunc = (UpdateFunction)Activator.CreateInstance(type, true);
                        updateFunc.mod = mod;
                        ups.Add(updateFunc);
                    }

                    if (typeof(InitializeFunction).IsAssignableFrom(type))
                    {
                        InitializeFunction initFunc = (InitializeFunction)Activator.CreateInstance(type, true);
                        initFunc.mod = mod;
                        inits.Add(initFunc);
                    }

                    if (typeof(LoadFunction).IsAssignableFrom(type))
                    {
                        LoadFunction loadFunc = (LoadFunction)Activator.CreateInstance(type, true);
                        loadFunc.mod = mod;
                        loads.Add(loadFunc);
                    }

                    if (typeof(SaveFunction).IsAssignableFrom(type))
                    {
                        SaveFunction saveFunc = (SaveFunction)Activator.CreateInstance(type, true);
                        saveFunc.mod = mod;
                        saves.Add(saveFunc);
                    }
                }
            }

            updateFuncs = ups.OrderBy((func) => func.Priority).ToArray();
            initFuncs = inits.OrderBy((func) => func.Priority).ToArray();
            loadFuncs = loads.OrderBy((func) => func.Priority).ToArray();
            saveFuncs = saves.OrderBy((func) => func.Priority).ToArray();
        }

        internal static void Update()
        {
            for (int i = 0; i < updateFuncs.Length; i++)
            {
                UpdateFunction updateFunction = updateFuncs[i];
                updateFunction.Update();
            }
        }

        internal static void Initialize()
        {
            for (int i = 0; i < initFuncs.Length; i++)
            {
                InitializeFunction initFunction = initFuncs[i];
                initFunction.Initialize();
            }
        }

        internal static void Load(TagCompound tagCompound)
        {
            for (int i = 0; i < loadFuncs.Length; i++)
            {
                LoadFunction loadFunction = loadFuncs[i];
                string modName = loadFunction.mod.Name;

                TagCompound modTag = null;
                if (tagCompound.ContainsKey(modName))
                {
                    modTag = tagCompound.GetCompound(modName);
                }
                loadFunction.Load(modTag);
            }
        }

        internal static TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            for (int i = 0; i < saveFuncs.Length; i++)
            {
                SaveFunction saveFunction = saveFuncs[i];
                TagCompound tagCompound = saveFunction.Save();
                if (tagCompound != null)
                {
                    string modName = saveFunction.mod.Name;
                    if (tag.ContainsKey(modName))
                    {
                        TagCompound modTag = tag.GetCompound(modName);
                        tag[modName] = modTag.Concat(tagCompound);
                    }
                    else
                    {
                        tag.Add(modName, tagCompound);
                    }
                }
            }
            return tag;
        }
    }
}
