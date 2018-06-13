using System;
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
        public double MaxAPForThisLevel { get; set; }
        public double MaxHPForThisLevel { get; set; }
        public double HostileProbabilityForThisLevel { get; set; }
        public double InitialHP { get; set; } = 10f;
        public double InitialAP { get; set; } = 7f;

        private Random rnd = new Random();

        public NPC(Position pos, GridManager grid)
        {
            MaxAPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, InitialHP, 0.15d);

            MaxHPForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 100d, InitialAP, 0.1d);

            HostileProbabilityForThisLevel =
                    ProcGenFunctions.Logistic(grid.Level, 1d, 0, 0.1d);

            NpcType = rnd.NextDouble() < HostileProbabilityForThisLevel
                ? StateOfNpc.Enemy : StateOfNpc.Neutral;

            Pos = pos;

            AP = rnd.NextDouble() * MaxAPForThisLevel;
            HP = rnd.NextDouble() * MaxHPForThisLevel;
        }
    }
}
