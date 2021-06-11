using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ALARM.TerrariaStuff
{
    ///
    public class ALARMConfigServer : ModConfig
    {
        ///
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /// <summary>Gets the instance of <see cref="ALARMConfigServer"/></summary>
        public static ALARMConfigServer GetInstance() => ModContent.GetInstance<ALARMConfigServer>();

        /// <summary>
        /// Enables handy features for developers
        /// </summary>
        [Obsolete]
        [Header("Developer Stuff")]
        public bool EnableDeveloperMode
        {
            get => false;
            set
            {

            }
        }

        /// <summary>
        /// Will catch common places for errors. It's recommended to disable this for testing to find errors more easily. Enabling this will also stop ALARM from throwing its own custom errors.
        /// </summary>
        [Obsolete]
        [DefaultValue(true)]
        public bool TryCatchErrors { get; set; }
    }
}
