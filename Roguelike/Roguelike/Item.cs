using System;

namespace Roguelike
{
    [Serializable()]
    public abstract class Item : IGameObject
    {
        public float Weight { get; set; }
    }
}
