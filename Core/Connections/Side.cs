using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM.Core.Connections
{
    /// <summary>
    /// Represents the 4 sides that exist in 2D space.
    /// </summary>
    public enum Side
    {
        /// <summary>
        /// (i, j - 1)
        /// </summary>
        Up,
        /// <summary>
        /// (i + 1, j)
        /// </summary>
        Right,
        /// <summary>
        /// (i, j + 1)
        /// </summary>
        Down,
        /// <summary>
        /// (i - 1, j)
        /// </summary>
        Left
    }

    /// <summary>
    /// 
    /// </summary>
    public static class SideExtensions
    {
        /// <summary>
        /// Gets the opposite side.
        /// </summary>
        public static Side Opposite(this Side side)
        {
            switch (side)
            {
                case Side.Up:
                    return Side.Down;
                case Side.Right:
                    return Side.Left;
                case Side.Down:
                    return Side.Up;
                case Side.Left:
                    return Side.Right;
                default:
                    throw new ArgumentException($"Unexpected enum: {side}");
            }
        }
    }
}
