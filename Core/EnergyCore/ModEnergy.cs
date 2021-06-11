using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ALARM.Core.EnergyCore
{
    /// <summary>
    /// This class is used to store data about types of energy mods add. This class is not used to store actual energy.
    /// </summary>
    public abstract class ModEnergy
    {
        /// <summary>
        /// The unique ID for this energy.
        /// </summary>
        public int type;

        /// <summary>
        /// The name of this energy.
        /// </summary>
        public string name;

        /// <summary>
        /// The mod this energy belongs to.
        /// </summary>
        public Mod mod;

        /// <summary>
        /// Whether or not this energy converts to and from Luna.
        /// </summary>
        [Obsolete]
        internal bool convertsToAndFromLuna;

        // TODO: This has no purpose rn
        /// <summary>
        /// This function will be called during the loading process.
        /// </summary>
        public virtual void SetStaticDefaults() { }

        /// <summary>
        /// The display name of your energy. By default, this uses the default of <see cref="DisplayNameAttribute"/>, which uses the class type and adds spaces after capital letters.
        /// </summary>
        public virtual string Name() => DisplayNameAttribute.Default(GetType());

        /// <summary>
        /// This function is to convert your energy to ALARM's base energy type, Luna. By default, this returns null, which disallows your energy to be converted to Luna. <para/>
        /// 
        /// Note: when implementing this function, you must also implement ConvertFromLuna. <para/>
        /// 
        /// This function should be fairly simple, and should probably be limited to linear functions. <br/>
        /// - If <paramref name="energy"/> is 0, this should return 0. <br/>
        /// - If <paramref name="energy"/> is positive, this should return positive. <br/>
        /// - If <paramref name="energy"/> is negative, this should return negative. <br/>
        /// A good function would be to simply multiply <paramref name="energy"/> by a constant, and when implementing ConvertFromLuna, divide by the same constant.
        /// </summary>
        /// <param name="energy">This is the amount of your energy to be converted to Luna.</param>
        /// <returns></returns>
        public abstract double ConvertToLuna(double energy);

        /// <summary>
        /// This function is to convert ALARM's base energy type, Luna, to your energy. By default, By default, this returns null, which disallows Luna to be converted to your energy. <para/>
        /// 
        /// Note: when implementing this function, you must also implement ConvertToLuna. <para/>
        /// 
        /// This function should be fairly simple, and should probably be limited to linear functions. <br/>
        /// - If <paramref name="luna"/> is 0, this should return 0. <br/>
        /// - If <paramref name="luna"/> is positive, this should return positive. <br/>
        /// - If <paramref name="luna"/> is negative, this should return negative. <br/>
        /// A good function would be to simply divide <paramref name="luna"/> by a constant, and when implementing ConvertToLuna, multiply by the same constant.
        /// </summary>
        /// <param name="luna">This is the amount of Luna to be converted to your energy.</param>
        /// <returns></returns>
        public abstract double ConvertFromLuna(double luna);
    }
}
