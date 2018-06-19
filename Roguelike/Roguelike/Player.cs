using System;
using System.Collections.Generic;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a player. It is a Game Object.
    /// </summary>
    [Serializable()]
    public class Player : IGameObject
    {
        /// <summary>
        /// Property that defines player health.
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// Propertu that defines player damage.
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Property that saves the current player input.
        /// </summary>
        public string Input { get; private set; } = "";

        /// <summary>
        /// Property that defines the max weight the player can carry.
        /// </summary>
        public float MaxWeight { get; private set; }

        /// <summary>
        /// Property that defines current player weight.
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Property that defines if player attacked someone or not.
        /// </summary>
        public bool Attacked { get; set; } = false;

        /// <summary>
        /// Property that defines if the equipped weapon broke or not.
        /// </summary>
        public bool Broken { get; set; } = false;

        /// <summary>
        /// Property that defines if player killed a NPC or not.
        /// </summary>
        public bool Killed { get; set; } = false;

        /// <summary>
        /// Property that defines player position.
        /// </summary>
        public Position PlayerPos { get; set; }

        /// <summary>
        /// Property that defines player inventory.
        /// </summary>
        public List<Item> Inventory { get; set; } = new List<Item>();

        /// <summary>
        /// Property that defines the currently equipped weapon.
        /// </summary>
        public Weapon Equipped { get; set; }

        /// <summary>
        /// Instance variable to be able to add and save high scores.
        /// </summary>
        private HighScoreManager hsm = new HighScoreManager();

        /// <summary>
        /// Instance variable to render the game.
        /// </summary>
        private Renderer render = new Renderer();

        /// <summary>
        /// Instance variablle to be able to save game.
        /// </summary>
        private GameManager gm;

        /// <summary>
        /// Instance variable to be able to randomize numbers.
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Constructor to Initialise player properties.
        /// </summary>
        /// <param name="gm">Instance of game manager</param>
        public Player(GameManager gm)
        {
            Health = 100;
            Weight = 0;
            MaxWeight = 20;
            Equipped = null;
            Attacked = false;
            Broken = false;
            Killed = false;
            this.gm = gm;
        }

        /// <summary>
        /// Method that verifies if player is dead.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        public void Die(GridManager grid)
        {
            // If player health is less or equal to 0
            if (Health <= 0)
            {
                // Render Dead Screen
                render.ShowDead();

                // If within the 10 best scores, add new highscore
                AddNewHighScore(grid);

                // Quit game
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Methd that controls all player inputs.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        public void PlayerController(GridManager grid)
        {
            do
            {
                // Ask fro player input
                Console.Write("\n> ");

                // Read player input
                Input = Console.ReadLine();

                // Create and Initialise count to 0
                int count = 0;

                //Update oldPlayerPos
                grid.oldPlayerPos = PlayerPos;

                // Do something according to player input
                switch (Input.ToLower())
                {
                    // If Input is w
                    case "w":
                        // Update player position. Move Up
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X - 1, PlayerPos.Y);
                        break;
                    // If Input is s
                    case "s":
                        // Update player position. Move Down
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X + 1, PlayerPos.Y);
                        break;
                    // If Input is a
                    case "a":
                        // Update player position. Move Left
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X, PlayerPos.Y - 1);
                        break;
                    // If Input is d
                    case "d":
                        // Update player position. Move Right
                        PlayerPos =
                            grid.RestrictToMap(PlayerPos.X, PlayerPos.Y + 1);
                        break;
                    // If Input is i
                    case "i":
                        // Show info about all items and traps
                        render.InfoInterface();

                        // Render grid again
                        render.RenderBoard(grid, this);
                        break;
                    // If Input is q
                    case "q":
                        // make sure if player wants to quit
                        MakeSureQuit(grid);

                        // Render grid again
                        render.RenderBoard(grid, this);
                        break;
                    // If Input is e
                    case "e":
                        // Render pick up item screen. If there is item to pick up
                        if (render.PickUpScreen(grid, this, out count))
                            // Pick up item
                            PickUpItems(grid, count);
                        break;
                    // If Input is v
                    case "v":
                        // Render drop item screen. If there is item to drop
                        if (render.DropItemsScreen(grid, this, out count))
                            // Drop Item
                            DropItems(grid, count);
                        break;
                    // If Input is u
                    case "u":
                        // Render use item screen. If there is item to use
                        if (render.UseItemScreen(this, out count))
                            // Use item
                            UseItems(count);
                        break;
                    // If Input is f
                    case "f":
                        // Render attack enemy screen. If there is enemy to 
                        // attack and u have a weapon selected
                        if (render.ChooseEnemyScreen(grid, this, out count))
                            // Attack chosen enemy
                            Fight(grid, count);
                        break;
                    // If Input is k
                    case "k":
                        // Save Game
                        gm.SaveGame();

                        // Render Save Complete
                        render.SaveComplete();

                        // Render grid again
                        render.RenderBoard(grid, this);
                        break;
                }
                //Inputs i, q, k don't spend a turn
            } while (Input == "i" || Input == "q" || Input == "k");
        }

        /// <summary>
        /// Method that defines a player attack.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="count">Maximum number possible to input.</param>
        public void Fight(GridManager grid, int count)
        {
            // Variable to hold player input
            string choice = "";

            // Variable index
            int i = 0;

            // Set Attacked as false;
            Attacked = false;

            // DO cycle while input is invalid
            do
            {
                // Ask and save player input
                Console.Write("\n> ");
                choice = Console.ReadLine();

                // If input is a number convert string input to int
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            // If input is 0
            if (i == 0)
            {
                // Go back to grid screen without losing a turn
                Health++;
                return;
            }
            // If input is not 0
            else
            {
                // Do cycle while player does not attack an enemy
                do
                {
                    // Define game object in player position
                    // Initially, i = player input
                    IGameObject go = grid.GameGrid[PlayerPos.X, PlayerPos.Y][i];

                    // If game object is NPC
                    if (go is NPC)
                    {
                        // If state of NPC is Neutral
                        if ((go as NPC).NpcType == StateOfNpc.Neutral)
                        {
                            // NPC changes to Enemy
                            (go as NPC).NpcType = StateOfNpc.Enemy;
                        }

                        // Define player Damage
                        Damage = rnd.NextDouble() * Equipped.AttackPower;

                        // Take health from NPC according to player Damage
                        (go as NPC).HP -= Damage;

                        // Verify if player is dead
                        (go as NPC).Die(grid, this);

                        // Verify if weapon breaks
                        IsWeaponBroken();

                        // Player attacked
                        Attacked = true;
                    }
                    // If game object is not an NPC
                    else
                    {
                        // Increment to index
                        i++;
                    }
                } while (!Attacked);
            }
        }

        /// <summary>
        /// Method that verifies if weapon equipped will break.
        /// </summary>
        public void IsWeaponBroken()
        {
            // If weapon breaks
            if (rnd.NextDouble() < 1 - Equipped.Durability)
            {
                // Player weapon broke
                Broken = true;

                // Remove weight from player
                Weight -= Equipped.Weight;

                // Remove weapon from equipped weapon
                Equipped = null;
            }
        }

        /// <summary>
        /// Method that picks up the chosen item by the player.
        /// </summary>
        /// <param name="grid">Game Grid.</param>
        /// <param name="count">Maximum number possible to input.</param>
        public void PickUpItems(GridManager grid, int count)
        {
            // Variable to hold if player picked something up or not
            bool picked = false;

            // Varible to hold player input
            string choice = "";

            // Variable index
            int i = 0;

            // Do cyce while input is invalid
            do
            {
                // Ask and save player input
                Console.Write("\n> ");
                choice = Console.ReadLine();

                // If input is a number convert string input to int
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            // If input is 0
            if (i == 0)
            {
                // Go back to grid screen without losing a turn
                Health++;
                return;
            }
            // If input is not 0
            else
            {
                // Do cycle while player does not pick up an item
                do
                {
                    // Define game object in player position
                    // Initially, i = player input
                    IGameObject go = grid.GameGrid[PlayerPos.X, PlayerPos.Y][i];

                    // If game object is an Item
                    if (go is Item)
                    {
                        // If item does not fit in bag
                        if ((Weight + (go as Item).Weight) > MaxWeight)
                        {
                            // Send error message.
                            Console.WriteLine("You can't carry anymore.");
                            Console.ReadLine();
                            return;
                        }
                        // If item fits in bag
                        else
                        {
                            // Add item to Inventory
                            Inventory.Add(go as Item);

                            // Add item weight to player weight
                            Weight += (go as Item).Weight;

                            // Remove item from grid
                            grid.GameGrid[PlayerPos.X, PlayerPos.Y].RemoveAt(i);
                            grid.GameGrid[PlayerPos.X, PlayerPos.Y].Add(null);

                            // Player picked something
                            picked = true;
                        }
                    }
                    // If game object is Map
                    else if (go is Map)
                    {
                        // Remove map from grid
                        grid.GameGrid[PlayerPos.X, PlayerPos.Y].Remove(grid.Map);
                        grid.GameGrid[PlayerPos.X, PlayerPos.Y].Add(null);

                        // Make the whole level explored
                        foreach (GameTile gt in grid.GameGrid) gt.Explored = true;

                        // Player picked something
                        picked = true;
                    }
                    // If game object is not an Item or Map
                    else
                    {
                        // Increment index
                        i++;
                    }
                } while (!picked);
            }
        }

        /// <summary>
        /// Method that drops the chosen item by the player.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        /// <param name="count">Maximum number possible to input.</param>
        public void DropItems(GridManager grid, int count)
        {
            // Varible to hold player input
            string choice = "";

            // Variable index
            int i = 0;

            // Do cyce while input is invalid
            do
            {
                // Ask and save player input
                Console.Write("\n> ");
                choice = Console.ReadLine();

                // If input is a number convert string input to int
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            // If input is 0
            if (i == 0)
            {
                // Go back to grid screen without losing a turn
                Health++;
                return;
            }
            // If input is not 0
            else
            {
                // Define game object in inventory
                // Initially, i = player input
                IGameObject go = Inventory[i - 1];

                // If game object is an Item
                if (go is Item)
                {
                    // Add game object to grid
                    grid.GameGrid[PlayerPos.X, PlayerPos.Y].AddObject(go);

                    // Remove game object from player inventory
                    Inventory.RemoveAt(i - 1);

                    // Remove item weight from player weight
                    Weight -= (go as Item).Weight;
                }
            }
        }

        /// <summary>
        /// Method that uses the chosen item by the player.
        /// </summary>
        /// <param name="count">Maximum number possible to input.</param>
        public void UseItems(int count)
        {
            // Varible to hold player input
            string choice = "";

            // Variable index
            int i = 0;

            // Do cyce while input is invalid
            do
            {
                // Ask and save player input
                Console.Write("\n> ");
                choice = Console.ReadLine();

                // If input is a number convert string input to int
                if (choice != "") i = Convert.ToInt32(choice);
            } while (i < 0 || i > count - 1 || choice == "");

            // If input is 0
            if (i == 0)
            {
                // Go back to grid screen without losing a turn
                Health++;
                return;
            }
            // If input is not 0
            else
            {
                // Define game object in inventory
                // Initially, i = player input
                IGameObject go = Inventory[i - 1];

                // If game object is Food
                if (go is Food)
                {
                    // Remove food from inventory
                    Inventory.RemoveAt(i - 1);

                    // Remove food item from player weight
                    Weight -= (go as Item).Weight;

                    // If player health is more than 100 
                    if ((Health + (go as Food).HPIncrease) > 100)
                    {
                        // Health stays 100
                        Health = 100;
                    }
                    // If player health is less than 100 
                    else
                    {
                        // Increase health by the respective HPIncrease of food
                        Health += (go as Food).HPIncrease;
                    }
                }
                // If game object is a weapon
                else if (go is Weapon)
                {
                    // Remove weapon from inventory
                    Inventory.RemoveAt(i - 1);

                    // If there is no equipped weapon
                    if (Equipped == null)
                    {
                        // Equipp chosen weapon
                        Equipped = (go as Weapon);
                    }
                    // If there is a equipped weapon
                    else
                    {
                        // Add previous weapon to inventory
                        Inventory.Add(Equipped as Weapon);

                        // Equipp chosen weapon
                        Equipped = (go as Weapon);
                    }
                }
            }
        }

        /// <summary>
        /// Method that checks if the user actually wants to wuit the game.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        public void MakeSureQuit(GridManager grid)
        {
            // Variable to hold player input
            string input = "";

            // Clear Console
            Console.Clear();

            // Ask if player wants to quit game
            Console.WriteLine("Are you sure you want to quit? (y/n)");

            // Do cyce while input is invalid
            do
            {
                // Ask and save player input
                Console.Write("\n> ");
                input = Console.ReadLine();
            } while (input.ToLower() != "n" && input.ToLower() != "y");

            // Do something according to player input
            switch (input.ToLower())
            {
                // If input is y
                case "y":
                    // If within the 10 best scores, add new highscore
                    AddNewHighScore(grid);

                    // Quit game
                    Environment.Exit(1);
                    break;
                // If input is n
                case "n":
                    // Continue game
                    break;
            }
        }

        /// <summary>
        /// Method that adds a new highscore if applicable.
        /// </summary>
        /// <param name="grid">Game grid.</param>
        public void AddNewHighScore(GridManager grid)
        {
            // Variable to hold player name
            string name = "";

            // If current score is bigger than the last score on the list
            if (hsm.Highscores[hsm.Highscores.Count - 1].Item2 < grid.Level)
            {
                // Render Interface to add new high score
                render.AddNewHighscoreInterface(grid);

                // Do cycle while input is invalid
                do
                {
                    // Ask and save user input (name)
                    Console.Write("> ");
                    name = Console.ReadLine();
                } while (name.Length > 15 || name == "");


                // Add score to high score list
                hsm.AddScore(name, grid.Level);

                // Save new list to file
                hsm.Save();
            }
        }
    }
}
