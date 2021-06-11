using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Components.Presets.EnergyPresets;
using Terraria.ModLoader;

namespace ALARM.Core.EnergyCore
{
    /// <summary>
    /// This represents energy connections between an Input and an Output.
    /// </summary>
    public class EnergyConnection : IEquatable<EnergyConnection>
    {
        /// <summary>
        /// The mod that represents this connection.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// The machine that energyInput is connected from.
        /// </summary>
        public EnergyOutput energyOutput;

        /// <summary>
        /// The machine that energyOutput is connected to.
        /// </summary>
        public EnergyInput energyInput;

        /// <summary>
        /// The limit of this connection, e.g. the cables' transfer rate.
        /// </summary>
        public double connectionLimit;

        /// <summary>
        /// The connection type of this connection.
        /// </summary>
        public int connectionType;

        /// <summary>
        /// The energy type of this connection.
        /// </summary>
        public int energyType;

        /// <summary>
        /// The standard constructor for Connection.
        /// </summary>
        public EnergyConnection(EnergyOutput connectionFrom, EnergyInput connectionTo, double? energyTransferAmount, int? energyTransferCooldown, int connectionType, int energyType)
        {
            energyOutput = connectionFrom;
            energyInput = connectionTo;
            this.connectionType = connectionType;
            this.energyType = energyType;
        }

        /// <summary>
        /// Implements value comparison.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;

            return Equals((EnergyConnection)obj);
        }

        /// <summary>
        /// Implements value comparison.
        /// </summary>
        public bool Equals(EnergyConnection other)
        {
            // todo: update this when done fiddling
            return energyOutput.Equals(other.energyOutput) &&
                energyInput.Equals(other.energyInput) &&
                connectionType == other.connectionType &&
                energyType == other.energyType;
        }

        /// <summary>
        /// Returns the Hash value of all fields and the string "connectionClass".
        /// </summary>
        public override int GetHashCode()
        {
            // todo: update this when done fiddling
            return (energyOutput, energyInput, connectionType, energyType, "connectionClass").GetHashCode();
        }
    }
}
