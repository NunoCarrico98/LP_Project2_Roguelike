﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class NPC : IGameObject
    {
        public StateOfNpc NpcType { get; set; }
        public Position Pos { get; set; }
        public double HP { get; set; }
        public double AP { get; set; }
        public double Damage { get; set; }
        public double MaxAPForThisLevel { get; set; }
        public double MaxHPForThisLevel { get; set; }
        public double HostileProbabilityForThisLevel { get; set; }

        private Random rnd = new Random();

        public NPC(Position pos, GridManager grid)
        {
            MaxAPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, 20d, 0.1d);

            MaxHPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, 20d, 0.1d);

            HostileProbabilityForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 1d, 20d, 0.1d);

            NpcType = rnd.NextDouble() < HostileProbabilityForThisLevel
                ? StateOfNpc.Enemy : StateOfNpc.Neutral;

            Pos = pos;

            AP = rnd.NextDouble() * MaxAPForThisLevel;
            HP = rnd.NextDouble() * MaxHPForThisLevel;
        }

        public void Fight(GridManager grid, Player p)
        {
            Damage = rnd.NextDouble() * AP;
            p.Health -= Damage;
            p.Die(grid);
        }

        public void Die(GridManager grid)
        {
            if (HP <= 0)
            {
                grid.gameGrid[Pos.X, Pos.Y].Remove(this);
            }
        }
    }
}
