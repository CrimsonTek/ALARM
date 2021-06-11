using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Connections;
using ALARM.Core.Machines;

namespace ALARM.Helpers
{
    /// <summary>
    /// Various utils that may come in handy.
    /// </summary>
    public static class ALARMUtils
    {
        /// <summary>
        /// Gets the adjacent tiles to machine.
        /// </summary>
        public static List<(int i, int j)> AdjacentTiles(Machine machine)
        {
            List<(int, int)> list = new List<(int, int)>();
            int i = machine.i;
            int j = machine.j;
            int width = machine.width;
            int height = machine.height;

            for (int x = i; x < i + width; x++)
            {
                list.Add((x, j - 1));
                list.Add((x, j + height));
            }

            for (int y = j; y < j + height; y++)
            {
                list.Add((i - 1, y));
                list.Add((i + width, y));
            }

            return list;
        }

        /// <summary>
        /// Gets the adjacent tiles to machine.
        /// </summary>
        public static List<(int i, int j, Side side)> AdjacentTilesWithSides(Machine machine)
        {
            List<(int, int, Side)> list = new List<(int, int, Side)>();
            int i = machine.i;
            int j = machine.j;
            int width = machine.width;
            int height = machine.height;

            for (int x = i; x < i + width; x++)
            {
                list.Add((x, j - 1, Side.Up));
                list.Add((x, j + height, Side.Down));
            }

            for (int y = j; y < j + height; y++)
            {
                list.Add((i - 1, y, Side.Left));
                list.Add((i + width, y, Side.Right));
            }

            return list;
        }

        /** **/ public static T Max<T>(params T[] nums) => nums.Max();
        /** **/ public static byte Max(params byte[] nums) => nums.Max();
        /** **/ public static sbyte Max(params sbyte[] nums) => nums.Max();
        /** **/ public static short Max(params short[] nums) => nums.Max();
        /** **/ public static ushort Max(params ushort[] nums) => nums.Max();
        /** **/ public static int Max(params int[] nums) => nums.Max();
        /** **/ public static uint Max(params uint[] nums) => nums.Max();
        /** **/ public static long Max(params long[] nums) => nums.Max();
        /** **/ public static ulong Max(params ulong[] nums) => nums.Max();
        /** **/ public static float Max(params float[] nums) => nums.Max();
        /** **/ public static double Max(params double[] nums) => nums.Max();
        /** **/ public static decimal Max(params decimal[] nums) => nums.Max();
    }
}
