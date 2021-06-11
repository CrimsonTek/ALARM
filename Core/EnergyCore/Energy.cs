using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.EnergyCore
{
    /// <summary>
    /// A struct representing energy.
    /// </summary>
    public struct Energy : IEquatable<Energy>, IComparable<Energy>
    {
        /// <summary>
        /// The amount of energy.
        /// </summary>
        public double amount;

        /// <summary>
        /// The type of energy.
        /// </summary>
        public int type;

        /// <summary>
        /// Converts this to Luna.
        /// </summary>
        public Energy ToLuna => EnergyManager.ConvertToLuna(this);

        /// <summary>
        /// Constructs this with 0 energy and the given type.
        /// </summary>
        public Energy(int type)
        {
            amount = 0;
            this.type = type;
        }

        /// <summary>
        /// Constructs this using the given value and type 'Luna'.
        /// </summary>
        public Energy(double amount)
        {
            this.amount = amount;
            type = EnergyID.Luna;
        }

        /// <summary>
        /// Constructs this using the given value and the given type.
        /// </summary>
        public Energy(double amount, int type)
        {
            this.amount = amount;
            this.type = type;
        }

        /// <summary>
        /// Converts this to the given energy type.
        /// </summary>
        public Energy Convert(int toType) => EnergyManager.Convert(this, toType);

        /// <summary>
        /// Returns whether the amount and type of this and <paramref name="obj"/> are equal if <paramref name="obj"/> is type <see cref="Energy"/>.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Energy)obj);
        }

        /// <summary>
        /// Returns whether the amount and type of this and <paramref name="other"/> are equal.
        /// </summary>
        public bool Equals(Energy other)
        {
            return amount == other.amount && type == other.type;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        public override int GetHashCode()
        {
            var hashCode = -811997554;
            hashCode = hashCode * -1521134295 + amount.GetHashCode();
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Compares the converted value of <paramref name="other"/>.
        /// </summary>
        public int CompareTo(Energy other)
        {
            Energy energy = EnergyManager.Convert(other, type);
            return amount.CompareTo(energy.amount);
        }

        /// <summary>
        /// Deconstructs this.
        /// </summary>
        public void Deconstruct(out double amount, out int energyType)
        {
            amount = this.amount;
            energyType = type;
        }

        /// <summary>
        /// Returns <paramref name="a"/>.
        /// </summary>
        public static Energy operator +(Energy a) => a;

        /// <summary>
        /// Returns the negative value of <paramref name="a"/>.
        /// </summary>
        public static Energy operator -(Energy a) => new Energy(-a.amount, a.type);

        /// <summary>
        /// Adds the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static Energy operator +(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return new Energy(a.amount + converted.amount, a.type);
        }

        /// <summary>
        /// Adds amount <paramref name="b"/> to <paramref name="a"/>.
        /// </summary>
        public static Energy operator +(Energy a, double b)
        {
            return new Energy(a.amount + b, a.type);
        }

        /// <summary>
        /// Adds amount <paramref name="a"/> to <paramref name="b"/>.
        /// </summary>
        public static Energy operator +(double a, Energy b)
        {
            return new Energy(b.amount + a, b.type);
        }

        /// <summary>
        /// Subtracts the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static Energy operator -(Energy a, Energy b) => a + -b;

        /// <summary>
        /// Subracts amount <paramref name="b"/> from <paramref name="a"/>.
        /// </summary>
        public static Energy operator -(Energy a, double b) => a + -b;

        /// <summary>
        /// Multiplies the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static Energy operator *(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return new Energy(a.amount * converted.amount, a.type);
        }

        /// <summary>
        /// Multiplies the amount <paramref name="a"/> by <paramref name="b"/>.
        /// </summary>
        public static Energy operator *(Energy a, double b) => new Energy(a.amount * b, a.type);

        /// <summary>
        /// Multiplies the amount <paramref name="b"/> by <paramref name="a"/>.
        /// </summary>
        public static Energy operator *(double a, Energy b) => new Energy(b.amount * a, b.type);

        /// <summary>
        /// Divides the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static Energy operator /(Energy a, Energy b)
        {
            if (b.amount == 0)
            {
                throw new DivideByZeroException();
            }
            Energy converted = b.Convert(a.type);
            return new Energy(a.amount / converted.amount, a.type);
        }

        /// <summary>
        /// Divides the amount <paramref name="a"/> by <paramref name="b"/>.
        /// </summary>
        public static Energy operator /(Energy a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException();
            }
            return new Energy(a.amount / b, a.type);
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator ==(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount == converted.amount;
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator !=(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount != converted.amount;
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator <(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount < converted.amount;
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator >(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount > converted.amount;
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator <=(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount <= converted.amount;
        }

        /// <summary>
        /// Compares the converted energy of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static bool operator >=(Energy a, Energy b)
        {
            Energy converted = b.Convert(a.type);
            return a.amount >= converted.amount;
        }

        /// <summary>
        /// Converts this to a value tuple.
        /// </summary>
        public static implicit operator (double amount, int energyType)(Energy value) => (value.amount, value.type);

        /// <summary>
        /// Converts a value tuple to this.
        /// </summary>
        public static implicit operator Energy((double amount, int energyType) value) => new Energy(value.amount, value.energyType);
    }
}
