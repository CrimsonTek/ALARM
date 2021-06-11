using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Cables;
using ALARM.Core.Components.Presets.EnergyPresets;
using ALARM.Core.Connections;
using ALARM.Core.Machines;
using ALARM.Helpers;

// TODO: remove
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ALARM.Core.EnergyCore
{
    public static class EnergyManager
    {
        #region Energy Transfer Stuff
        /// <summary>
        /// Updates all EnergyOutputs.
        /// </summary>
        public static void Update()
        {
            ResetLast();

            EnergyOutput[] outputMachines = GetOutputMachines();

            foreach (EnergyOutput energyOutput in outputMachines)
            {
                TransferEnergy(energyOutput);
            }
        }

        /// <summary>
        /// Resets <see cref="EnergyOutput.old_recentOutput"/> and <see cref="EnergyInput.old_recentInput"/>.
        /// </summary>
        private static void ResetLast()
        {
            foreach (Machine machine in MachineManager.machines)
            {
                if (machine.TryGetComponent(out EnergyOutput energyOutput))
                {
                    energyOutput.recentOutput.amount = 0;
                }
                if (machine.TryGetComponent(out EnergyInput energyInput))
                {
                    energyInput.recentInput.amount = 0;
                }
            }
        }

        /// <summary>
        /// Gets the output machines sorted by their output amount, highest first.
        /// </summary>
        /// <returns></returns>
        private static EnergyOutput[] GetOutputMachines()
        {
            List<EnergyOutput> list = new List<EnergyOutput>();
            foreach (Machine machine in MachineManager.machines)
            {
                if (machine.TryGetComponent(out EnergyOutput energyOutput))
                {
                    list.Add(energyOutput);
                }
            }

            return list.OrderByDescending((output) => output.outputAmount).ToArray();
        }

        private static void TransferEnergy(EnergyOutput energyOutput)
        {
            int energyType = energyOutput.EnergyStorage.energyType;

            List<EnergyConnection> connections = GetConnectedMachines(energyOutput, energyType);

            var sortedConnections = connections.OrderBy((conn) => conn.energyInput.AmountCanInput.ToLuna).ToArray();

            Energy initial = energyOutput.AmountCanOutput;
            Energy floatingEnergy = initial;

            for (int i = 0; i < sortedConnections.Length; i++)
            {
                EnergyInput energyInput = sortedConnections[i].energyInput;
                int remainingMachines = sortedConnections.Length - i;

                Energy average = floatingEnergy / remainingMachines;
                Energy canTake = energyInput.AmountCanInput;
                Energy toGive = EnergyUtils.Max(0, EnergyUtils.Min(average, canTake));

                energyInput.GiveEnergy(toGive);
                floatingEnergy -= toGive;
            }

            Energy gave = initial - floatingEnergy;
            energyOutput.TakeEnergy(gave);
        }

        /// <summary>
        /// Gets custom connections, adjacent connections, and cable connections. <para/>
        /// Note: These connections are unsorted.
        /// </summary>
        private static List<EnergyConnection> GetConnectedMachines(EnergyOutput energyOutput, int energyType)
        {
            List<EnergyConnection> list1 = GetCustomConnections(energyOutput, energyType);
            List<EnergyConnection> list2 = GetAdjacentConnections(energyOutput, energyType);
            List<EnergyConnection> list3 = GetCableConnections(energyOutput, energyType);

            List<EnergyConnection> baseConnections = new List<EnergyConnection>();
            baseConnections.AddRange(list1);
            baseConnections.AddRange(list2);
            baseConnections.AddRange(list3);

            return baseConnections;
        }

        /// <summary>
        /// Gets custom connections made by users.
        /// </summary>
        private static List<EnergyConnection> GetCustomConnections(EnergyOutput energyOutput, int energyType)
        {
            List<EnergyConnection> customInputs = new List<EnergyConnection>();
            foreach (EnergyConnection connection in energyOutput.CustomConnections())
            {
                EnergyInput energyInput = connection.energyInput;
                if (energyInput.CanInputFrom(connection))
                {
                    customInputs.Add(connection);
                }
            }

            return customInputs;
        }

        /// <summary>
        /// Gets adjacent connections.
        /// </summary>
        private static List<EnergyConnection> GetAdjacentConnections(EnergyOutput energyOutput, int energyType)
        {
            HashSet<EnergyConnection> adjacentConnections = new HashSet<EnergyConnection>();
            foreach (var (i, j, side) in ALARMUtils.AdjacentTilesWithSides(energyOutput.machine))
            {
                Machine machine = MachineManager.GetMachine(i, j);
                EnergyInput energyInput = machine?.GetComponent<EnergyInput>();
                if (energyInput == null)
                {
                    continue;
                }

                EnergyConnection connection = new EnergyConnection(energyOutput, energyInput, 0, 0, ConnectionID.AdjacentConnection, energyType);

                if (adjacentConnections.Contains(connection))
                {
                    continue;
                }

                if (!energyOutput.CanOutputTo(connection))
                {
                    continue;
                }

                if (!energyInput.CanInputFrom(connection))
                {
                    continue;
                }

                adjacentConnections.Add(connection);
            }

            return adjacentConnections.ToList();
        }

        /// <summary>
        /// Gets cable connections.
        /// </summary>
        private static List<EnergyConnection> GetCableConnections(EnergyOutput energyOutput, int energyType)
        {
            List<EnergyConnection> connections = new List<EnergyConnection>();
            foreach (var (i, j, side) in ALARMUtils.AdjacentTilesWithSides(energyOutput.machine))
            {
                Cable cable = CableManager.GetCable(i, j);
                Side opposite = side.Opposite();
                if (!cable.IsConnected(opposite))
                {
                    continue;
                }

                EnergyConnection connection = new EnergyConnection(energyOutput, null, null, null, ConnectionID.CableConnection, energyType);
                if (!energyOutput.CanOutputTo(connection))
                {
                    continue;
                }

                EnergyConnection[] connArr = CableManager.FindConnectedMachines();

                connections.AddRange(connArr);
            }

            return connections;
        }
        #endregion

        [Obsolete]
        public static double Convert(double energyAmount, int energyTypeFrom, int energyTypeTo)
        {
            if (energyTypeFrom == energyTypeTo)
                return energyAmount;

            ModEnergy[] modEnergies = EnergyLoader.ModEnergies;
            if (energyTypeFrom < 0 || energyTypeFrom >= modEnergies.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(energyTypeFrom), $"There is no energy of type {energyTypeFrom}.");
            }

            if (energyTypeTo < 0 || energyTypeTo >= modEnergies.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(energyTypeTo), $"There is no energy of type {energyTypeTo}.");
            }

            ModEnergy from = modEnergies[energyTypeFrom];
            ModEnergy to = modEnergies[energyTypeTo];

            double luna = from.ConvertToLuna(energyAmount);
            double converted = to.ConvertFromLuna(luna);
            return converted;
        }

        public static Energy Convert(Energy energy, int toType)
        {
            if (energy.type == toType)
                return energy;

            ModEnergy[] modEnergies = EnergyLoader.ModEnergies;
            if (energy.type < 0 || energy.type >= modEnergies.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(energy.type), $"There is no energy of type {energy.type}.");
            }

            if (toType < 0 || toType >= modEnergies.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(toType), $"There is no energy of type {toType}.");
            }

            ModEnergy from = modEnergies[energy.type];
            ModEnergy to = modEnergies[toType];

            double luna = from.ConvertToLuna(energy.amount);
            double converted = to.ConvertFromLuna(luna);
            return new Energy(converted, toType);
        }

        public static Energy ConvertToLuna(Energy energy)
        {
            if (energy.type == EnergyID.Luna)
                return energy;

            ModEnergy[] modEnergies = EnergyLoader.ModEnergies;
            if (energy.type < 0 || energy.type >= modEnergies.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(energy.type), $"There is no energy of type {energy.type}.");
            }

            ModEnergy from = modEnergies[energy.type];

            double luna = from.ConvertToLuna(energy.amount);
            return new Energy(luna, EnergyID.Luna);
        }
    }

    internal interface IEnumerableFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> original);
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
