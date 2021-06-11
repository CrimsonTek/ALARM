using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.EnergyCore;
using Terraria.ModLoader;

namespace ALARM.Core.Components.Presets.EnergyPresets
{
    /// <summary>
    /// This component may be added to machines for energy usage. You may add this class as it is, or override this class to use various hooks and override functions, such as overriding <see cref="AddEnergy"/> to allow for energy conversion.
    /// </summary>
    public class EnergyStorage : Component
    {
        /// <summary>
        /// The type of energy this component uses. Defaults to ALARM's energy, Luna. You can change this when you add the component to your machine, or if you derive this, you can change it in <see cref="Component.Initialize"/>.
        /// </summary>
        public int energyType;

        [Obsolete]
        public double oldCurrentEnergy;

        /// <summary>
        /// The current energy of this component.
        /// </summary>
        public Energy currentEnergy;

        /// <summary>
        /// The max energy of this component. Set this to the desired value when adding it to your machine, or if you derive this, you can change it in <see cref="Component.Initialize"/>.
        /// </summary>
        public Energy maxEnergy;

        /// <summary>
        /// The amount of energy missing. This is shorthand for currentEnergy - maxEnergy;
        /// </summary>
        public Energy MissingEnergy => maxEnergy - currentEnergy;

        /// <summary>
        /// Adds energy to this component. Cannot add more energy it has room for, so any energy that could not be added will be left in energyAmount. 
        /// </summary>
        public virtual void AddEnergy(ref Energy energy)
        {
            Energy maxGive = EnergyUtils.Min(MissingEnergy, energy);
            currentEnergy += maxGive;
            energy -= maxGive;
        }

        /// <summary>
        /// Removes energy from this component. 
        /// </summary>
        public virtual Energy RemoveEnergy(Energy energy)
        {
            Energy maxTake = EnergyUtils.Min(currentEnergy, energy);
            currentEnergy -= maxTake;
            return maxTake;
        }
    }
}
