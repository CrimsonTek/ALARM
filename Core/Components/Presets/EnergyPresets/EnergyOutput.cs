using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Attributes;
using ALARM.Core.Connections;
using ALARM.Core.EnergyCore;
using ALARM.Core.Machines;

namespace ALARM.Core.Components.Presets.EnergyPresets
{
    /// <summary>
    /// A component which handles outputting energy. You may initialize this as is. You may want to set <see cref="old_outputAmount"/>. For more complex control, you may derive your own EnergyOutput component and override its virtual functions. <para/>
    /// Notes: <para/>
    ///  This component MUST be paired with an <see cref="EnergyStorage"/> component.
    /// </summary>
    [RequireComponent(typeof(EnergyStorage))]
    public class EnergyOutput : Component
    {
        /// <summary>
        /// The total amount this component can output each frame.
        /// </summary>
        [Obsolete]
        public double old_outputAmount;

        /// <summary>
        /// The total amount this component can output each frame.
        /// </summary>
        public Energy outputAmount;

        /// <summary>
        /// The amount this has output this frame.
        /// </summary>
        [Obsolete]
        public double old_recentOutput;

        /// <summary>
        /// The amount this has output this frame.
        /// </summary>
        public Energy recentOutput;

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
                    _energyStorage = machine.GetComponent<EnergyStorage>();
                }

                return _energyStorage;
            }
        }

        /// <summary>
        /// How much this component can output. This takes into account the amount of energy this has already output, as well as how much energy this sibling EnergyStorage has.
        /// Returns the energy in the sibling EnergyStorage's type.
        /// </summary>
        /// <returns></returns>
        public Energy AmountCanOutput
        {
            get
            {
                Energy currentEnergy = EnergyStorage.currentEnergy;
                Energy outputAmount = this.outputAmount - recentOutput;

                return EnergyUtils.Max(0, EnergyUtils.Min(currentEnergy, outputAmount));
            }
        }

        /// <summary>
        /// This function allows you to create your own connections to machines. This may be used for custom cables, wireless connection, etc.
        /// </summary>
        public virtual List<EnergyConnection> CustomConnections() => new List<EnergyConnection>();

        /// <summary>
        /// Whether or not your machine should be able to connect to another using the given connection. This may be used to disallow certain connections, most notably adjacent connects, or connections with machines whose energy type doesn't match.
        /// </summary>
        public virtual bool CanOutputTo(EnergyConnection connection) => true;

        /// <summary>
        /// Removes <paramref name="energy"/> from <see cref="EnergyStorage.oldCurrentEnergy"/>, and adds the same amount to <see cref="old_recentOutput"/>.
        /// </summary>
        public void TakeEnergy(Energy energy)
        {
            EnergyStorage.currentEnergy -= energy;
            recentOutput += energy;
        }
    }
}
