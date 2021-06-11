using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Components;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ALARM.Core.Machines
{
    /// <summary>
    /// This class acts as the base class for your machines
    /// </summary>
    public class Machine
    {
        #region Components
        /// <summary>
        /// Adds a component of the type <typeparamref name="T"/>, then returns the initialized component.
        /// </summary>
        public T AddComponent<T>() where T : Component => (T)AddComponent(typeof(T));

        /// <summary>
        /// Adds a component of the type <paramref name="classType"/>, then returns the initialized component.
        /// </summary>
        public Component AddComponent(Type classType)
        {
            Component component = Component.NewComponent(this, classType);
            components.Add(component);
            componentsToRunStartOn.Add(component);
            component.Initialize();
            return component;
        }

        /// <summary>
        /// Returns the component of Type <typeparamref name="T"/> if the machine has one attached, <see langword="null"/> if it doesn't.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exactType"></param>
        /// <returns></returns>
        public T GetComponent<T>(bool exactType = false) where T : Component
        {
            Type tType = typeof(T).GetType();
            foreach (Component component in components)
            {
                if (component is T t)
                {
                    if (exactType)
                    {
                        if (component.GetType().Equals(tType))
                        {
                            return t;
                        }
                    }
                    else
                    {
                        return t;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Returns the component of Type <paramref name="type"/> if the machine has one attached, <see langword="null"/> if it doesn't.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="exactType"></param>
        /// <returns></returns>
        public Component GetComponent(Type type, bool exactType = false)
        {
            foreach (Component component in components)
            {
                if (exactType)
                {
                    if (component.GetType().Equals(type))
                    {
                        return component;
                    }
                }
                else
                {
                    if (type.IsAssignableFrom(component.GetType()))
                    {
                        return component;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Gets all the components which can be assigned to <typeparamref name="T"/>. Will return an empty array if none are found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exactType"></param>
        /// <returns></returns>
        public T[] GetComponents<T>(bool exactType = false)
        {
            Type tType = typeof(T).GetType();
            List<T> tList = new List<T>();
            foreach (var component in components)
            {
                if (component is T t)
                {
                    if (exactType)
                    {
                        if (component.GetType().Equals(tType))
                        {
                            tList.Add(t);
                        }
                    }
                    else
                    {
                        tList.Add(t);
                    }
                }
            }
            return tList.ToArray();
        }

        /// <summary>
        /// Tries to get the first component of type <typeparamref name="T"/>. Returns false and sets <paramref name="component"/> to default if it can't find one.
        /// </summary>
        public bool TryGetComponent<T>(out T component, bool exactType = false) where T : Component
        {
            foreach (Component current in components)
            {
                if (current is T t)
                {
                    if (exactType)
                    {
                        if (current.GetType().Equals(typeof(T)))
                        {
                            component = t;
                            return true;
                        }
                    }
                    else
                    {
                        component = t;
                        return true;
                    }
                }
            }
            component = default;
            return false;
        }
        #endregion

        #region Fields
        /// <summary>
        /// This is the key that represents your machine, just like <see cref="Item.type"/>.
        /// </summary>
        public int type;

        /// <summary>
        /// This is the mod this machine belongs to.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// This is the name of this machine.
        /// </summary>
        public string name;

        /// <summary>
        /// The right-most tile of this machine.
        /// </summary>
        public int i = -1;

        /// <summary>
        /// The top-most tile of this machine.
        /// </summary>
        public int j = -1;

        /// <summary>
        /// The width of this machine in tile coordinates.
        /// </summary>
        public int width = -1;

        /// <summary>
        /// The height of this machine in tile coordinates.
        /// </summary>
        public int height = -1;

        /// <summary>
        /// You should not add components to this list directly.
        /// </summary>
        public List<Component> components = new List<Component>();

        /// <summary>
        /// Components which have not yet have Start() run on them yet.
        /// </summary>
        internal List<Component> componentsToRunStartOn = new List<Component>();
        #endregion

        #region Other
        /// <summary>
        /// This is used to create a new machine. This is the machine equivalent to <see cref="NPC.NewNPC"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Machine NewMachine(int type)
        {
            if (type < 0 || type >= MachineLoader.machineEntries.Count)
            {
                throw new IndexOutOfRangeException($"A machine with the index {type} could not be found.");
            }

            MachineEntry machineEntry = MachineLoader.machineEntries[type];
            Type classType = machineEntry.classType;
            Machine machine = (Machine)Activator.CreateInstance(classType);
            machine.type = type;
            machine.mod = machineEntry.mod;
            machine.name = machineEntry.name;
            MachineManager.machines.Add(machine);
            machine.Initialize();
            return machine;
        }

        /// <summary>
        /// Destroys this machine.
        /// </summary>
        public bool Destroy()
        {
            List<Machine> machineArr = MachineManager.machines;
            for (int index = 0; index < machineArr.Count; index++)
            {
                Machine machine = machineArr[index];
                if (machine == this)
                {
                    if (machine.PreDestroy())
                    {
                        machine.OnDestroy();
                        machineArr.RemoveAt(index);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Finds a machine at [<paramref name="i"/>, <paramref name="j"/>] and destroys it. Use <paramref name="exactCoordinates"/> to only remove the machine if it's top-left tile is exactly [<paramref name="i"/>, <paramref name="j"/>].
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="exactCoordinates">Only remove the machine if it's top-left tile is exactly [<paramref name="i"/>, <paramref name="j"/>].</param>
        public static bool DestroyAt(int i, int j, bool exactCoordinates = false)
        {
            List<Machine> machineArr = MachineManager.machines;
            for (int index = 0; index < machineArr.Count; index++)
            {
                Machine machine = machineArr[index];
                if (exactCoordinates)
                {
                    if (machine.i == i && machine.j == j)
                    {
                        RemoveMachine(machine, index);
                    }
                }
                else
                {
                    if (machine.i <= i && i < machine.i + machine.width
                        && machine.j <= j && j < machine.j + machine.height)
                    {
                        RemoveMachine(machine, index);
                    }
                }
            }
            return false;

            bool RemoveMachine(Machine machine, int index)
            {
                if (machine.PreDestroy())
                {
                    machine.OnDestroy();

                    foreach (Component component in machine.components)
                    {
                        component.OnDestroy();
                    }

                    machineArr.RemoveAt(index);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the machine at [<paramref name="i"/>, <paramref name="j"/>]. Returns <see langword="null"/> if no machine is found. Use <paramref name="exactCoordinates"/> to find only the machine if its top-left tile is exactly [<paramref name="i"/>, <paramref name="j"/>].
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="exactCoordinates">Set to <see langword="true"/> to find only the machine if its top-left tile is exactly [<paramref name="i"/>, <paramref name="j"/>].</param>
        /// <returns></returns>
        public static Machine GetAt(int i, int j, bool exactCoordinates = false)
        {
            List<Machine> machineArr = MachineManager.machines;
            foreach (Machine machine in machineArr)
            {
                if (exactCoordinates)
                {
                    if (machine.i == i && machine.j == j)
                    {
                        return machine;
                    }
                }
                else
                {
                    if (machine.i <= i && i < machine.i + machine.width)
                    {
                        if (machine.j <= j && j < machine.j + machine.height)
                        {
                            return machine;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method will find any <see cref="Component"/> that derives <see cref="IClickable"/>, and call <see cref="IClickable.Click"/>. If any of these components returns true, <see cref="Click"/> will return <see langword="true"/>. <br/>
        /// This method should be called when right clicking a machine. This call must be implemented in the <see cref="ModTile"/> which represents this machine. <br/>
        /// You should have something like this in your ModTile class: <para/>
        /// public override bool NewRightClick(int i, int j)    <br/>
        /// {                                                   <br/>
        ///     return Machine.GetAt(i, j).Click();             <br/>
        /// }                                                   <para/>
        /// </summary>
        public bool Click()
        {
            bool any = false;
            IClickable[] clickArr = GetComponents<IClickable>();
            foreach (var clickable in clickArr)
            {
                any |= clickable.Click();
            }
            return any;
        }
        #endregion

        #region Virtual
        /// <summary>
        /// This is called when a new machine is created via <see cref="NewMachine"/>, and during the load process. This can be used to add components, set data fields, etc.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// This is called when this machine is loaded from save. This will always be called after <see cref="Initialize"/>.
        /// </summary>
        /// <param name="tagCompound"></param>
        public virtual void Load(TagCompound tagCompound) { }

        /// <summary>
        /// Whether or not a given component should update or not. This could be used for something like stopping a crafting component from updating if a machine is overheated. <br/>
        /// Returns <see langword="true"/> by default.
        /// </summary>
        public virtual bool PreUpdateComponent(Component component) => true;

        /// <summary>
        /// This is called after all other components have updated.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Return false to stop this machine and its components from being destroyed. <br/>
        /// Returns <see langword="true"/> by default.
        /// </summary>
        /// <returns></returns>
        public virtual bool PreDestroy() => true;

        /// <summary>
        /// Is called when this machine is destroyed. This can be used to drop items, create sound or particle effects, etc. This is called after <see cref="Component.OnDestroy"/> has been called for all components. This will only be called if <see cref="PreDestroy"/> returns <see langword="true"/> (it does by default).
        /// </summary>
        public virtual void OnDestroy() { }
        #endregion
    }
}
