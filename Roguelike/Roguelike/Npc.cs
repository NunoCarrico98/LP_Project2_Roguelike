using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a NPC.It is a Game Object.
    /// </summary>
    [Serializable()]
    public class NPC : IGameObject
    {
        /// <summary>
        /// Property that defines Type of NPC: Enemy or Neutral.
        /// </summary>
        public StateOfNpc NpcType { get; set; }

        /// <summary>
        /// Property that defines NPC Position.
        /// </summary>
        public Position Pos { get; set; }

        /// <summary>
        /// Property that defines NPC HP for level.
        /// </summary>
        public double HP { get; set; }

        /// <summary>
        /// Property that defines NPC Attack Power for level.
        /// </summary>
        public double AP { get; set; }

        /// <summary>
        /// Property that defines damage given to player in a certain attack.
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Property that defines the maximum Attack Power for level.
        /// </summary>
        public double MaxAPForThisLevel { get; set; }

        /// <summary>
        /// Property that defines the maximum HP for level.
        /// </summary>
        public double MaxHPForThisLevel { get; set; }

        /// <summary>
        /// Property that defines hostile probability for level
        /// </summary>
        public double HostileProbabilityForThisLevel { get; set; }

        /// <summary>
        /// Instance variable to be able to randomize numbers.
        /// </summary>
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Constructor to Initialise all properties.
        /// </summary>
        /// <param name="pos">Player position.</param>
        /// <param name="grid">Game Grid.</param>
        public NPC(Position pos, GridManager grid)
        {
            // Initialise Max Attack Power for level. It increases with level
            MaxAPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, 20d, 0.1d);

            // Initialise Max HP for level. It increases with level
            MaxHPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, 20d, 0.1d);

            // Initialise Hostile Probability for level. It increases with level
            HostileProbabilityForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 1d, 20d, 0.1d);

            // Initialise NPC Type according to Hostile Probability for level
            NpcType = rnd.NextDouble() < HostileProbabilityForThisLevel
                ? StateOfNpc.Enemy : StateOfNpc.Neutral;

            // Initialise NPC Position
            Pos = pos;

            // Set Attack Power for level
            AP = rnd.NextDouble() * MaxAPForThisLevel;
            // Set HP for level
            HP = rnd.NextDouble() * MaxHPForThisLevel;
        }

        /// <summary>
        /// Method that defines a NPC attack.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        public void Fight(GridManager grid, Player p)
        {
            // Set Damage of NPC
            Damage = rnd.NextDouble() * AP;
            // Take health from player according to Damage of NPC
            p.Health -= Damage;
            // Verify if player is dead
            p.Die(grid);
        }

        /// <summary>
        /// Method that verifies if NPC if dead.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="p">Current player in game.</param>
        public void Die(GridManager grid, Player p)
        {
            // If NPC health is less or equal to 0
            if (HP <= 0)
            {
                // Player killed an NPC
                p.Killed = true;
                // Remove NPC from grid
                grid.GameGrid[Pos.X, Pos.Y].Remove(this);

                // Define rndom number of items between 0 and 5
                int numberOfItens = rnd.Next(6);
                // Repeat cycle for all items
                for (int i = 0; i < numberOfItens; i++)
                {
                    // 50% probability that item is food
                    if (rnd.NextDouble() < 0.5d)
                    {
                        // Create and Initialise food
                        Item item = new Food();
                        // Add food to tile
                        grid.GameGrid[Pos.X, Pos.Y].AddObject(item);
                    }
                    // 50% probability that item is weapon
                    else
                    {
                        // Create and Initialise weapon
                        Item item = new Weapon();
                        // Add weapon to tile
                        grid.GameGrid[Pos.X, Pos.Y].AddObject(item);
                    }
                }
            }
        }
    }
}
