using System;
using System.Collections.Generic;
using System.IO;
using ALARM.Core.Cables;
using ALARM.Core.Components;
using ALARM.Core.Machines;
using ALARM.TerrariaStuff;
using Terraria.ModLoader;

namespace ALARM
{
	///
	public class ALARM : Mod
	{
		private List<(Action update, Mod mod, string name)> updateFunctions;
		/// <summary>
		/// You can use this event to add your own update functions inbetween ALARM's default update functions. <para/>
		/// Usage:																													<br/>
		/// private void Usage()																									<br/>
		/// {																														<br/>
		///     AddToUpdateFunctions += (list) => list.Add((MethodToAdd, <see cref="ModContent.GetInstance{T}"/>, "MethodToAdd"));	<br/>
		/// }																														<br/>
		/// private void MethodToAdd()																								<br/>
		/// {																														<br/>
		///     //																													<br/>
		/// }																														<para/>
		/// Remember not to use parentheses when typing the method you want to call, otherwise it will not compile properly!
		/// </summary>
		public static event Action<List<(Action update, Mod mod, string name)>> AddToUpdateFunctions;
		private void Update()
		{
			for (int i = 0; i < updateFunctions.Count; i++)
			{
				(Action update, Mod mod, string name) = updateFunctions[i];
				try
				{
					update.Invoke();
				}
				catch (Exception inner)
				{
					throw new Exception($"An error occured while updating {mod}.{name}.", inner);
				}
			}
		}

		private List<(Action load, Mod mod, string name)> modLoadFunctions;
		/// <summary>
		/// You can use this event to add your own load functions inbetween ALARM's default load functions. <para/>
		/// Usage:																													<br/>
		/// private void Usage()																									<br/>
		/// {																														<br/>
		///     AddToModLoadFunctions += (list) => list.Add((MethodToAdd, <see cref="ModContent.GetInstance{T}"/>, "MethodToAdd"));	<br/>
		/// }																														<br/>
		/// private void MethodToAdd()																								<br/>
		/// {																														<br/>
		///     //																													<br/>
		/// }																														<para/>
		/// Remember not to use parentheses when typing the method you want to call, otherwise it will not compile properly!
		/// </summary>
		public static event Action<List<(Action load, Mod mod, string name)>> AddToModLoadFunctions;
		private void ModLoad()
		{
			for (int i = 0; i < modLoadFunctions.Count; i++)
			{
				(Action load, Mod mod, string name) = modLoadFunctions[i];
				try
				{
					load.Invoke();
				}
				catch (Exception inner)
				{
					throw new Exception($"An error occured while loading {mod}.{name}.", inner);
				}
			}
		}

		private void SetupFunctions()
		{
			ALARM mod = ModContent.GetInstance<ALARM>();
			updateFunctions = new List<(Action, Mod, string)>
			{
				(MachineManager.Update, mod, "UpdateFunction")
			};
			AddToUpdateFunctions?.Invoke(updateFunctions);

			modLoadFunctions = new List<(Action, Mod, string)>
			{
				(MachineLoader.Load, mod, "MachineLoaderLoad"),
				(ComponentLoader.Load, mod, "CompLoaderLoad"),
				(CableLoader.Load, mod, "CableLoaderLoad")
			};
			AddToModLoadFunctions?.Invoke(modLoadFunctions);
		}

		/// <summary>
		/// This method is run after other mods are loaded, so it's used for cross-mod load processes, such as automatically loading machines and components from other mods. This process is done from <see cref="MachineLoader"/>. This load process may be altered by subscribing to any of the events available in this class (as described later). <br/>
		/// To use these events, simply subscribe to them in your mod's Load function with a method or anonymous function which adds or inserts an applicable function, your mod, and a name which may be used for debugging. <para/>
		/// Some available events to use to integrate your methods with ALARM's default methods include: <br/>
		/// - <see cref="AddToUpdateFunctions"/> (Updates called every frame while a player is in the world and the game is running.) <br/>
		/// - <see cref="AddToModLoadFunctions"/> (Functions which happen during compile time, which is when machines and components are loaded.) <br/>
		/// </summary>
		public override void AddRecipes()
		{
			SetupFunctions();
			ModLoad();
		}

		/// <summary>This function is where general update functions are called.</summary>
		public override void PreUpdateEntities()
		{
			Update();
		}

		/// todo
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{

		}

		/// <summary>
		/// <see cref="ModContent.GetInstance{T}"/> wrapper.
		/// </summary>
		public static ALARM GetInstance() => ModContent.GetInstance<ALARM>();
	}
}