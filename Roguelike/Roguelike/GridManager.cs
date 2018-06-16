using System;

namespace Roguelike
{
    /// <summary>
    /// Class that controls all grid movement and management.
    /// </summary>
    [Serializable()]
    public class GridManager
    {
        /// <summary>
        /// Property that defines number of rows.
        /// </summary>
        public int Rows { get; } = 8;
        /// <summary>
        /// Property that defines number of columns.
        /// </summary>
        public int Columns { get; } = 8;
        /// <summary>
        /// Property that defines max numberof visible objects.
        /// </summary>
        public int ObjectsPerTile { get; } = 10;
        /// <summary>
        /// Property that defines the current game level.
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Property that defines the game grid
        /// </summary>
        public GameTile[,] GameGrid { get; set; }
        /// <summary>
        /// Property that defines a Map. One map exists per level.
        /// </summary>
        public Map Map { get; private set; } = new Map();
        /// <summary>
        /// Property that defines an exit. One exit exists per level and 
        /// it occupies the whole tile.
        /// </summary>
        public Exit Exit { get; private set; } = new Exit();
        /// <summary>
        /// Create and Initialise an instance to save the old position of player.
        /// </summary>
        public Position oldPlayerPos;

        /// <summary>
        /// Create and Initialise an instance of type Random.
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Constructor to Initialise Level.
        /// </summary>
        public GridManager()
        {
            // Initialise game Grid
            GameGrid = new GameTile[Rows, Columns];

            // Cycle through all tiles and initialise them
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    GameGrid[x, y] = new GameTile();
                }
            }
        }

        /// <summary>
        /// Method to update Grid.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void Update(Player player)
        {
            // Update player position
            UpdatePlayerPosition(player);
            //Check current tile for traps
            CheckForTraps(player);
            CheckForNPC(player);
            // Check if player won level
            WinLevel(player);
        }

        /// <summary>
        /// Set initial positions for all game objects.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void SetInitialPositions(Player player)
        {
            // Create and Initialise Player and Exit randoms
            int exitRnd = rnd.Next(8);
            int playerRnd = rnd.Next(8);

            // Add player to grid
            GameGrid[playerRnd, 0][0] = player;
            // Save old player position
            oldPlayerPos = new Position(playerRnd, 0);
            // Save player position
            player.PlayerPos = new Position(playerRnd, 0);
            // Make sure that when player spawns tiles around him become explored
            Explore(player);

            // Add Map to Level
            GameGrid[rnd.Next(8), rnd.Next(8)].AddObject(Map);

            // Add Exit to tile on grid
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                GameGrid[exitRnd, 7][i] = Exit;
            }

            // Spawn Traps
            SpawnTraps();

            // Spawn Items
            SpawnItems();

            // Spawn NPCs
            SpawnNPC();

        }

        /// <summary>
        /// Method to spawn level NPCs
        /// </summary>
        public void SpawnNPC()
        {
            // Initialise the maximum number of NPCs. It increases with level
            int maxNPCsForLevel =
                (int)ProcGenFunctions.Logistic(Level, 20d, 10d, 0.3d);
            // Set the number of NPCs for the current level. 
            // It's random between 0 and the maximum number of NPCs.
            int numberOfNPCs = rnd.Next(maxNPCsForLevel + 1);

            // Repeat spawn for the number of NPCs of level
            for (int i = 0; i < numberOfNPCs; i++)
            {
                // Create variables row and columns
                int row;
                int column;
                // Do cycle while tile of row and column contains exit
                do
                {
                    // Define row and column for current NPC
                    row = rnd.Next(8);
                    column = rnd.Next(8);
                } while (GameGrid[row, column].Contains(Exit));
                // Define position to give to NPC
                Position pos = new Position(row, column);
                // Add NPC to tile
                GameGrid[row, column].AddObject(new NPC(pos, this));
            }
        }

        /// <summary>
        /// Method to spawn level traps.
        /// </summary>
        public void SpawnTraps()
        {
            // Initialise the maximum number of traps. It increases with level
            int maxTrapsPerLevel =
                (int)ProcGenFunctions.Logistic(Level, 20d, 10d, 0.3d);
            // Set the number of traps for the current level. 
            // It's random between 0 and the maximum number of traps.
            int numberOfTraps = rnd.Next(maxTrapsPerLevel + 1);

            // Repeat spawn for the number of traps of level
            for (int i = 0; i < numberOfTraps; i++)
            {
                // Create variables row and columns
                int row;
                int column;
                // Do cycle while the position chosen for the trap is the same 
                // as the exit
                do
                {
                    // Define row and column for current NPC
                    row = rnd.Next(8);
                    column = rnd.Next(8);
                } while (GameGrid[row, column].Contains(Exit));

                // Initialise trap in a random position
                Trap trap = new Trap();
                // Add trap to grid
                GameGrid[row, column].AddObject(trap);
            }
        }

        /// <summary>
        /// Method to spawn level items.
        /// </summary>
        public void SpawnItems()
        {
            // Initialise the maximum number of items. It decreases with level
            int maxItensPerLevel =
                (int)ProcGenFunctions.Logistic(Level, 30d, 10d, -0.3d);
            // Set the number of items for the current level. 
            // It's random between 0 and the maximum number of items.
            int numberOfItens = rnd.Next(maxItensPerLevel + 1);

            // Repeat spawn for the number of items of level
            for (int i = 0; i < numberOfItens; i++)
            {
                // Create variables row and columns
                int row;
                int column;
                // Do cycle while the position chosen for the trap is the same 
                // as the exit
                do
                {
                    // Define row and column for current NPC
                    row = rnd.Next(8);
                    column = rnd.Next(8);
                } while (GameGrid[row, column].Contains(Exit));

                // 50% probability that item is food
                if (rnd.NextDouble() < 0.5d)
                {
                    // Create and Initialise food
                    Item item = new Food();
                    // Add food to tile
                    GameGrid[row, column].AddObject(item);
                }
                // 50% probability that item is a weapon
                else
                {
                    // Create and Initialise weapon
                    Item item = new Weapon();
                    // Add weapon to tile
                    GameGrid[row, column].AddObject(item);
                }
            }
        }


        /// <summary>
        /// Method that updates player position according to user input.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void UpdatePlayerPosition(Player player)
        {
            // Remove Player from current tile
            GameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            GameGrid[oldPlayerPos.X, oldPlayerPos.Y].Add(null);
            //oldPlayerPos = player.PlayerPos;

            // Add Player to new tile
            GameGrid[player.PlayerPos.X, player.PlayerPos.Y].Insert(0, player);
            GameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveNullsOutsideView();

            // Make sure tiles around player are explored
            Explore(player);

            // Take 1 health from player
            player.Health--;
        }

        /// <summary>
        /// Method that checks if player got hit by a trap.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void CheckForTraps(Player player)
        {
            // Cycle through all objects in tile
            foreach (IGameObject go in GameGrid[player.PlayerPos.X, player.PlayerPos.Y])
            {
                // If object is a Trap and it wasn't activated yet
                if (go is Trap && (go as Trap).FallenInto == false)
                {
                    // Trap becomes Inactive
                    (go as Trap).FallenInto = true;
                    // Player takes damage from trap
                    player.Health -= (go as Trap).Damage;
                }
            }
        }

        /// <summary>
        /// Method that checks if player entered a tile with NPCs.
        /// </summary>
        /// <param name="p">Current player in game.</param>
        public void CheckForNPC(Player p)
        {
            // Cycle through all objects in tile 
            foreach (IGameObject go in GameGrid[p.PlayerPos.X, p.PlayerPos.Y])
            {
                // If object is a NPC and it is an Hostile
                if (go is NPC && (go as NPC).NpcType == StateOfNpc.Enemy)
                {
                    // NPC hits player
                    (go as NPC).Fight(this, p);
                }
            }
        }

        /// <summary>
        /// Method that checks if player got to an exit.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void WinLevel(Player player)
        {
            // If current tile contains player and exit
            if (GameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(Exit) &&
               GameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                // Restart grid
                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        GameGrid[x, y] = new GameTile();
                    }
                }

                // Add level
                Level++;

                // Begin new level
                SetInitialPositions(player);
            }
        }

        /// <summary>
        /// Method that makes sure that tiles around player are visible.
        /// </summary>
        /// <param name="player">Current player in game.</param>
        public void Explore(Player player)
        {
            // Tile that player is in becomes visible
            GameGrid[player.PlayerPos.X, player.PlayerPos.Y].Explored = true;

            // Make sure that the positions we're checking are not outside the map
            Position pos1 =
                RestrictToMap(player.PlayerPos.X - 1, player.PlayerPos.Y);
            Position pos2 =
                RestrictToMap(player.PlayerPos.X + 1, player.PlayerPos.Y);
            Position pos3 =
                RestrictToMap(player.PlayerPos.X, player.PlayerPos.Y - 1);
            Position pos4 =
                RestrictToMap(player.PlayerPos.X, player.PlayerPos.Y + 1);

            // Tiles above, below, to the left and right become visible
            GameGrid[pos1.X, pos1.Y].Explored = true;
            GameGrid[pos2.X, pos2.Y].Explored = true;
            GameGrid[pos3.X, pos3.Y].Explored = true;
            GameGrid[pos4.X, pos4.Y].Explored = true;
        }

        /// <summary>
        /// Method that checks if given coordinates are outside map.
        /// </summary>
        /// <param name="x">Row of Coordinates.</param>
        /// <param name="y">Column of Coordinates.</param>
        /// <returns>Returns the new Position inside grid.</returns>
        public Position RestrictToMap(int x, int y)
        {
            // If x or are less than 0, they become 0
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            // If x or y are bigger than the size of the grid, they become 
            // size - 1
            if (y > Columns - 1) y = Columns - 1;
            if (x > Rows - 1) x = Rows - 1;

            // return the new Position inside grid
            return new Position(x, y);
        }

        /// <summary>
        /// Method to get the tile in certain coordinates.
        /// </summary>
        /// <param name="x">Row of grid.</param>
        /// <param name="y">Column of grid.</param>
        /// <returns>Returns the tile in given coordinates.</returns>
        public GameTile GetTile(int x, int y)
        {
            // Return tile
            return GameGrid[x, y];
        }

        /// <summary>
        /// MMethod to get the game object in certain coordinates.
        /// </summary>
        /// <param name="x">Row of grid.</param>
        /// <param name="y">Column of grid.</param>
        /// <param name="posIntTile">Position in tile.</param>
        /// <returns>Returns the game object in given coordinates and 
        /// tile position.</returns>
        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            // Return game object on grid
            return GameGrid[x, y][posIntTile];
        }
    }
}
