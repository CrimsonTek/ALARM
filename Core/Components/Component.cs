using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Machines;
using Terraria;
using Terraria.ModLoader;

namespace ALARM.Core.Components
{
    /// <summary>
    /// Components are the heart of machines. They can be used to communicate with other machines, and handle complicated functionality in a simple and organized way. Many presets may be used or overriden for your machines.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// This is the key that represents your component, just like <see cref="Item.type"/>.
        /// </summary>
        public int type;

        /// <summary>
        /// This is the owner of this component. It is automatically added when <see cref="Machine.AddComponent{T}"/> is called.
        /// </summary>
        public Machine machine;

        /// <summary>
        /// This is the mod this component belongs to.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// This is the name of this component.
        /// </summary>
        public string name;

        /// <summary>
        /// A list of methods loaded.
        /// </summary>
        internal Dictionary<string, MethodInfo> loadedMethods;

        /// <summary>
        /// This is similar to initialization, but during startup, it is guaranteed to run after all components have already been added to its machine. <para/>
        /// Do not add components to machine in this field.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// This is the initialization method. This is called when the component is added to a machine.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Removes the component from its machine.
        /// </summary>
        public void Remove()
        {
            bool success = machine.components.Remove(this);
            if (!success)
            {
                throw new Exception($"The component {this} could not be removed from {machine}.");
            }
        }

        /// <summary>
        /// This function is a generalized update function that can be used for the functionality of this component. This will usually run once per frame (60 times a second), so you may want to have a "cooldown" for whatever operation this component does. <br/>
        /// Note that this will only be called if this component's machine <see cref="Machine.PreUpdateComponent(Component)"/> returns true (it does by default).
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// This function is called on all components when their owner is destroyed, if it returns <see langword="true"/> on <see cref="Machine.PreDestroy"/> (it does by default).
        /// </summary>
        public virtual void OnDestroy() { }

        /// <summary>
        /// Returns the component of Type <typeparamref name="T"/> if this component's owner has one attached, <see langword="null"/> if it doesn't.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSibling<T>(bool exactType = false) where T : Component
        {
            return machine.GetComponent<T>(exactType);
        }

        /// <summary>
        /// This will create a component and initialize it. It will not be added to its machine parent though, so using this method is not recommended. <see cref="Machine.AddComponent(Type)"/> or <see cref="Machine.AddComponent{T}"/> is much more recommended.
        /// </summary>
        internal static Component NewComponent(Machine machine, int type)
        {
            if (type < 0 || type >= ComponentLoader.componentEntries.Count)
            {
                throw new IndexOutOfRangeException($"A component could not be found with index {type}.");
            }

            ComponentEntry entry = ComponentLoader.componentEntries[type];
            Component component = (Component)Activator.CreateInstance(entry.classType);
            component.machine = machine;
            component.mod = entry.mod;
            component.name = entry.name;
            component.type = type;
            component.Initialize();
            return component;
        }
        internal static Component NewComponent(Machine machine, Type classType)
        {
            ComponentEntry entry;
            try
            {
                entry = ComponentLoader.classTypeToEntry[classType];
            }
            catch (Exception inner)
            {
                throw new Exception($"{nameof(NewComponent)} could not find an entry for key \"{classType}\". Maybe it didn't get initialized? ({nameof(ComponentLoader.classTypeToEntry)}.count = {ComponentLoader.classTypeToEntry.Count}).", inner);
            }

            object obj = null;
            try
            {
                obj = Activator.CreateInstance(classType);
            }
            catch (Exception inner)
            {
                throw new Exception($"{nameof(NewComponent)} could not instantiate a component for type {classType}.", inner);
            }

            if (obj is Component component)
            {
                component.machine = machine;
                component.mod = entry.mod;
                component.name = entry.name;
                component.type = entry.type;
                component.Initialize();
                return component;
            }
            else
            {
                throw new ArgumentException($"Parameter {nameof(classType)} must be a Type that is assignable to {nameof(Component)}.", nameof(classType));
            }
        }
    }
}
