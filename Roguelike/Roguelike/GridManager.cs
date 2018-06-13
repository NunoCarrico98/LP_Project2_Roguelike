using System;
using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Class that controls all grid movement and management
    /// </summary>
    public class GridManager
    {
        public int Rows { get; } = 8;
        public int Columns { get; } = 8;
        public int ObjectsPerTile { get; } = 10;
        public int Level { get; set; } = 1;
        public GameTile[,] gameGrid { get; private set; }
        public Map Map { get; private set; } = new Map();
        public Exit Exit { get; private set; } = new Exit();

        private Random rnd = new Random();
        private Position oldPlayerPos;

        /// <summary>
        /// Constructor to Initialise Level
        /// </summary>
        public GridManager()
        {
            gameGrid = new GameTile[Rows, Columns];
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
        /// <param name="player"></param>
        public void Update(Player player)
        {
            // Update player position
            UpdatePlayerPosition(player);
            //Check current tile for traps
            CheckForTraps(player);
            // Check if player won level
            WinLevel(player);
        }

        public void SetInitialPositions(Player player)
        {
            // Create and Initialise Player and Exit randoms
            int exitRnd = rnd.Next(0, 8);
            int playerRnd = rnd.Next(0, 8);


            // Add player to grid
            gameGrid[playerRnd, 0][0] = player;
            // Save player position
            oldPlayerPos = new Position(playerRnd, 0);
            player.PlayerPos = new Position(playerRnd, 0);
            Explore(player);
            CheckForTraps(player);

            gameGrid[rnd.Next(0, 8), rnd.Next(0, 8)].AddObject(Map);

            // Add Exit to tile on grid
            for (int i = 0; i < ObjectsPerTile; i++)
            {
                gameGrid[exitRnd, 7][i] = Exit;
            }

            SpawnTraps();
            SpawnItems();
        }

        public void SpawnTraps()
        {
            for (int i = 0; i < 5; i++)
            {
                Trap trap = new Trap(new Position(rnd.Next(0, 8), rnd.Next(0, 8)));
                gameGrid[trap.TrapPos.X, trap.TrapPos.Y].AddObject(trap);
            }
        }

        public void SpawnItems()
        {
            for (int i = 0; i < 5; i++)
            {
                Food food = new Food(new Position(rnd.Next(0, 8), rnd.Next(0, 8)));
                gameGrid[food.FoodPos.X, food.FoodPos.Y].AddObject(food);

                Weapon weapon = new Weapon(new Position(rnd.Next(0, 8), rnd.Next(0, 8)));
                gameGrid[weapon.WeaponPos.X, weapon.WeaponPos.Y].AddObject(weapon);
            }
        }

        public void UpdatePlayerPosition(Player player)
        {
            // Remove Player from current tile
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Remove(player);
            gameGrid[oldPlayerPos.X, oldPlayerPos.Y].Add(null);
            oldPlayerPos = new Position(player.PlayerPos.X, player.PlayerPos.Y);

            // Add Player to new tile
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Insert(0, player);
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].RemoveNullsOutsideView();

            Explore(player);

            // Take 1 health from player
            player.Health--;
        }

        public void CheckForTraps(Player player)
        {
            for (int i = ObjectsPerTile - 1; i >= 0; i--)
            {
                IGameObject go = gameGrid[player.PlayerPos.X, player.PlayerPos.Y][i];
                if (go is Trap && (go as Trap).FallenInto == false)
                {
                    (go as Trap).FallenInto = true;
                    player.Health -= (go as Trap).Damage;


                }
            }

        }

        public void PickUpItems(Player p)
        {
            bool picked = false;
            string choice = Console.ReadLine();
            int i = Convert.ToInt32(choice);
            if (i == 0)
            {   
                p.Health++;
                return;
            }
            else
            {   
                do
                {
                    IGameObject go = gameGrid[p.PlayerPos.X, p.PlayerPos.Y][i];
                    if (go is Item)
                    {
                        if ((p.Weight + (go as Item).Weight) > p.MaxWeight)
                        {
                            Console.WriteLine("You can't carry anymore.");
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            p.Inventory.Add(go as Item);
                            p.Weight += (go as Item).Weight;
                            gameGrid[p.PlayerPos.X, p.PlayerPos.Y].RemoveAt(i);
                            gameGrid[p.PlayerPos.X, p.PlayerPos.Y].Add(null);
                            picked = true;
                        }
                    }
                    else if (go is Map)
                    {
                        gameGrid[p.PlayerPos.X, p.PlayerPos.Y].Remove(Map);
                        gameGrid[p.PlayerPos.X, p.PlayerPos.Y].Add(null);
                        foreach (GameTile gt in gameGrid) gt.Explored = true;
                        picked = true;
                    }
                    else
                    {
                        i++;
                    }
                } while (!picked);
            }
        }

        public void DropItems(Player p)
        {
            string choice = Console.ReadLine();
            int i = Convert.ToInt32(choice);

            if (i == 0)
            {
                p.Health++;
                return;
            }
            else
            {
                IGameObject go = p.Inventory[i - 1];
                if (go is Item)
                {
                    gameGrid[p.PlayerPos.X, p.PlayerPos.Y].AddObject(go);
                    p.Inventory.RemoveAt(i - 1);
                    p.Weight -= (go as Item).Weight;
                }
            }
        }

        public void UseItems(Player p)
        {
            string choice = Console.ReadLine();
            int i = Convert.ToInt32(choice);

            if (i == 0)
            {
                p.Health++;
                return;
            }
            else
            {
                IGameObject go = p.Inventory[i - 1];
                if (go is Food)
                {
                    p.Inventory.RemoveAt(i - 1);
                    p.Weight -= (go as Item).Weight;
                    if((p.Health + (go as Food).HPIncrease) > 100)
                    {
                        p.Health = 100;
                    } else
                    {
                        p.Health += (go as Food).HPIncrease;
                    }
                }
                else if (go is Weapon)
                {
                    p.Inventory.RemoveAt(i - 1);
                    if (p.Equipped == null)
                    {
                        p.Equipped = (go as Weapon);
                    } else 
                    {
                        p.Inventory.Add(p.Equipped as Weapon);
                        p.Equipped = (go as Weapon);
                    }
                }
            }
        }

        public void WinLevel(Player player)
        {
            // If current tile contains player and exit
            if (gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(Exit) &&
               gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Contains(player))
            {
                // Remove everything from grid
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

        public void Explore(Player player)
        {
            gameGrid[player.PlayerPos.X, player.PlayerPos.Y].Explored = true;

            Position pos1 = Verify(player.PlayerPos.X - 1, player.PlayerPos.Y);
            Position pos2 = Verify(player.PlayerPos.X + 1, player.PlayerPos.Y);
            Position pos3 = Verify(player.PlayerPos.X, player.PlayerPos.Y - 1);
            Position pos4 = Verify(player.PlayerPos.X, player.PlayerPos.Y + 1);

            gameGrid[pos1.X, pos1.Y].Explored = true;
            gameGrid[pos2.X, pos2.Y].Explored = true;
            gameGrid[pos3.X, pos3.Y].Explored = true;
            gameGrid[pos4.X, pos4.Y].Explored = true;
        }

        public Position Verify(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (y > Columns - 1) y = Columns - 1;
            if (x > Rows - 1) x = Rows - 1;

            return new Position(x, y);
        }

        public GameTile GetTile(int x, int y)
        {
            return gameGrid[x, y];
        }

        public IGameObject GetGO(int x, int y, int posIntTile)
        {
            // Return game object on grid
            return gameGrid[x, y][posIntTile];
        }
    }
}
