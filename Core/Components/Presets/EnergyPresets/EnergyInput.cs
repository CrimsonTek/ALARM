using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Attributes;
using ALARM.Core.EnergyCore;

namespace ALARM.Core.Components.Presets.EnergyPresets
{
    /// <summary>
    /// A component which handles inputting energy. You may initialize this as is, or derive your own EnergyInput component. If you initialize this, you should set <see cref="old_inputAmount"/>. <para/>
    /// Note: This MUST be paired with an <see cref="EnergyStorage"/> component.
    /// </summary>
    [RequireComponent(typeof(EnergyStorage))]
    public class EnergyInput : Component
    {
        [Obsolete]
        public double old_inputAmount;

        /// <summary>
        /// The max amount this component can input any given frame.
        /// </summary>
        public Energy inputAmount;

        [Obsolete]
        public double old_recentInput;

        /// <summary>
        /// The amount this component has inputted this frame.
        /// </summary>
        public Energy recentInput;

        private EnergyStorage _energyStorage;
        /// <summary>
        /// Gets the sibling of type <see cref="EnergyStorage"/>.
        /// </summary>
        public EnergyStorage EnergyStorage
        {
            get
            {
                if (_energyStorage == null)
                {
                    _energyStorage = GetSibling<EnergyStorage>();
                }

                return _energyStorage;
            }
        }

        /// <summary>
        /// The amount of energy this can input this frame. This takes into consideration how much energy this has already recieved and also how much remaining energy storage this sibling EnergyStorage component has.
        /// </summary>
        public Energy AmountCanInput
        {
            get
            {
                Energy missingEnergy = EnergyStorage.MissingEnergy;
                Energy inputAmount = this.inputAmount - recentInput;

                return EnergyUtils.Max(0, EnergyUtils.Min(missingEnergy, inputAmount));
            }
        }

        /// <summary>
        /// The amount of energy this can input this frame. This takes into consideration how much energy this has already recieved and also how much remaining energy storage this sibling EnergyStorage component has.
        /// </summary>
        /// <param name="energyType"></param>
        /// <returns></returns>
        [Obsolete]
        public Energy old_AmountCanInput(out int energyType)
        {
            EnergyStorage energyStorage = machine.GetComponent<EnergyStorage>();

            Energy missingEnergy = energyStorage.MissingEnergy;
            Energy inputAmount = this.inputAmount - recentInput;

            energyType = energyStorage.energyType;
            return EnergyUtils.Max(0, EnergyUtils.Min(missingEnergy, inputAmount));
        }

        /// <summary>
        /// Override this to opt out of some connections. You can use this to accept multiple energy types, or make your machine unable to connection to other specific machines. <para/>
        /// By default, this returns true if the sibling <see cref="EnergyStorage.energyType"/> == <see cref="EnergyConnection.energyType"/>.
        /// </summary>
        public virtual bool CanInputFrom(EnergyConnection connection)
        {
            int energyType = EnergyStorage.energyType;
            return energyType == connection.energyType;
        }

        /// <summary>
        /// Adds <paramref name="energy"/> to <see cref="EnergyStorage.currentEnergy"/> and <see cref="recentInput"/>.
        /// </summary>
        public virtual void GiveEnergy(Energy energy)
        {
            int storageEnergyType = EnergyStorage.energyType;
            Energy toGive = energy.Convert(storageEnergyType);
            EnergyStorage.currentEnergy += toGive;
            recentInput += toGive;
        }
    }
}
