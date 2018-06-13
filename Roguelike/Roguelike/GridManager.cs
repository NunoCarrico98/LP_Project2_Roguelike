using System;

namespace Roguelike
{
    /// <summary>
    /// Class that controls all grid movement and management
    /// </summary>
    public class GridManager
    {
        /// <summary>
        /// Property that defines number of rows
        /// </summary>
        public int Rows { get; } = 8;
        /// <summary>
        /// Property that defines number of columns
        /// </summary>
        public int Columns { get; } = 8;
        /// <summary>
        /// Property that defines max numberof visible objects
        /// </summary>
        public int ObjectsPerTile { get; } = 10;
        /// <summary>
        /// Property that defines the current game level
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Property that defines the game grid
        /// </summary>
        public GameTile[,] gameGrid { get; private set; }
        /// <summary>
        /// Property that defines a Map. One map exists per level
        /// </summary>
        public Map Map { get; private set; } = new Map();
        /// <summary>
        /// Property that defines an exit. One exit exists per level and 
        /// it occupies the whole tile
        /// </summary>
        public Exit Exit { get; private set; } = new Exit();

        /// <summary>
        /// Create and Initialise an instance of type Random
        /// </summary>
        private Random rnd = new Random();
        /// <summary>
        /// Create and Initialise an instance to save the old position of player
        /// </summary>
        private Position oldPlayerPos;

        /// <summary>
        /// Constructor to Initialise Level
        /// </summary>
        public GridManager()
        {
            // Initialise game Grid
            gameGrid = new GameTile[Rows, Columns];

            // Cycle through all tiles and initialise them
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    gameGrid[x, y] = new GameTile();
                }
            }
        }

        /// <summary>
        /// Update Grid
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void Update(Player player)
        {
            // Update player position
            UpdatePlayerPosition(player);
            //Check current tile for traps
            CheckForTraps(player);
            // Check if player won level
            WinLevel(player);
        }

        /// <summary>
        /// Set initial positions for all game objects
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void SetInitialPositions(Player player)
        {
            // Create and Initialise Player and Exit randoms
            int exitRnd = rnd.Next(0, 8);
            int playerRnd = rnd.Next(0, 8);

            // Add player to grid
            gameGrid[playerRnd, 0][0] = player;
            // Save old player position
            oldPlayerPos = new Position(playerRnd, 0);
            // Save player position
            player.PlayerPos = new Position(playerRnd, 0);
            // Make sure that when player spawns tiles around him become explored
            Explore(player);

            // Add Map to Level
            gameGrid[rnd.Next(0, 8), rnd.Next(0, 8)].AddObject(Map);

            // Add Exit to tile on grid
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = Exit;
            }

            // Spawn Traps
            SpawnTraps();

            // Spawn NPCs
            SpawnNPC();
        }

        public void SpawnNPC()
        {
            int maxNPCsForLevel = (int)ProcGenFunctions.Linear(Level, 1d, 2d);
            int numberOfNPCs = rnd.Next(maxNPCsForLevel);

            for (int i = 0; i < numberOfNPCs; i++)
            {
                int row = rnd.Next(8);
                int column = rnd.Next(8);
                Position pos = new Position(row, column);
                gameGrid[row, column].AddObject(new NPC(pos, this));
            }
        }

        /// <summary>
        /// Method to spawn all traps from level
        /// </summary>
        public void SpawnTraps()
        {
            for (int i = 0; i < 5; i++)
            {
                // Create new trap
                Trap trap;
                // Do cycle while the position chosen for the trap is the same 
                // as the exit
                do
                {
                    // Initialise trap in a random position
                    trap = new Trap(new Position(rnd.Next(0, 8), rnd.Next(0, 8)));
                } while (gameGrid[trap.TrapPos.X, trap.TrapPos.Y].Contains(Exit));

                // Add trap to grid
                gameGrid[trap.TrapPos.X, trap.TrapPos.Y].AddObject(trap);
            }
        }

        /// <summary>
        /// Method that updates player position according to user input
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void UpdatePlayerPosition(Player player)
        {
            // Remove Player from current tile
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Add(null);
            oldPlayerPos = new Position(player.PlayerPos.X, player.PlayerPos.Y);

            // Add Player to new tile
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Insert(0, player);
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveNullsOutsideView();

            // Make sure tiles around player are explored
            Explore(player);

            // Take 1 health from player
            player.Health--;
        }

        /// <summary>
        /// Method that checks if player got hit by a trap
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void CheckForTraps(Player player)
        {
            // Cycle through all objects in tile backwards
            for (int i = ObjectsPerTile - 1; i >= 0; i--)
            {
                // Save object in variable
                IGameObject go = gameGrid[player.PlayerPos.X, player.PlayerPos.Y][i];
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

        public void PickUpMap(Player player)
        {
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(Map) &&
               gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Remove(Map);
                gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Add(null);
                foreach (GameTile gt in gameGrid) gt.Explored = true;
            }
        }

        /// <summary>
        /// Method that checks if player got to an exit
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void WinLevel(Player player)
        {
            // If current tile contains player and exit
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(Exit) &&
               gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                // Restart grid
                for (int x = 0; x < Rows; x++)
                {
                    for (int y = 0; y < Columns; y++)
                    {
                        gameGrid[x, y] = new GameTile();
                    }
                }

                // Add level
                Level++;

                // Begin new level
                SetInitialPositions(player);
            }
        }

        /// <summary>
        /// Method that makes sure that tiles around player are visible
        /// </summary>
        /// <param name="player">Current player in game</param>
        public void Explore(Player player)
        {
            // Tile that player is in becomes visible
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Explored = true;

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
            gameGrid[pos1.X, pos1.Y].Explored = true;
            gameGrid[pos2.X, pos2.Y].Explored = true;
            gameGrid[pos3.X, pos3.Y].Explored = true;
            gameGrid[pos4.X, pos4.Y].Explored = true;
        }

        /// <summary>
        /// Method that checks if given coordinates are outside map
        /// </summary>
        /// <param name="x">Row of Coordinates</param>
        /// <param name="y">Column of Coordinates</param>
        /// <returns>Returns the new Position inside grid</returns>
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
        /// Method to get the tile in certain coordinates
        /// </summary>
        /// <param name="x">Row of grid</param>
        /// <param name="y">Column of grid</param>
        /// <returns>Returns the tile in given coordinates</returns>
        public GameTile GetTile(int x, int y)
        {
            // Return tile
            return gameGrid[x, y];
        }

        /// <summary>
        /// MMethod to get the game object in certain coordinates
        /// </summary>
        /// <param name="x">Row of grid</param>
        /// <param name="y">Column of grid</param>
        /// <param name="posIntTile">Position in tile</param>
        /// <returns>Returns the game object in given coordinates and 
        /// tile position</returns>
        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            // Return game object on grid
            return gameGrid[x, y][posIntTile];
        }
    }
}
