using System;

namespace Roguelike
{
    /// <summary>
    /// Abstract Class that defines an Item: Food or Weapon.
    /// </summary>
    [Serializable()]
    public abstract class Item : IGameObject
    {
        /// <summary>
        /// Property that defines item weight.
        /// </summary>
        public float Weight { get; set; }
    }
}
