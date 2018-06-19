using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a row and column (Position).
    /// </summary>
    [Serializable()]
    public class Position
    {
        /// <summary>
        /// Property that defines the row.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Property that defines the column.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor to initialise row and column.
        /// </summary>
        /// <param name="x">Given Row.</param>
        /// <param name="y">Given Column</param>
        public Position(int x, int y)
        {
            // Initialise row and column
            X = x;
            Y = y;
        }
    }
}
