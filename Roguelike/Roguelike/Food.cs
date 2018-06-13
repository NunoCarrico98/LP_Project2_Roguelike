using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    class Food : Item
    {
        public float HPIncrease { get; private set; }
        public TypesOfFood FoodType { get; set; }
        public Position FoodPos { get; set; }
        Random rnd = new Random();

        public Food(Position pos)
        {
            FoodType = (TypesOfFood)rnd.Next(0, Enum.GetNames(typeof(TypesOfTraps)).Length + 1);
            SetHp();
            FoodPos = pos;
        }

        public Food(TypesOfFood typefood)
        {
            FoodType = typefood;
            SetHp();
        }

        public void SetHp()
        {
            switch (FoodType)
            {
                case TypesOfFood.Bacon:
                    HPIncrease = 4;
                    break;
                case TypesOfFood.Burger:
                    HPIncrease = 10;
                    break;
                case TypesOfFood.Noodles:
                    HPIncrease = 7;
                    break;
                case TypesOfFood.Pizza:
                    HPIncrease = 8;
                    break;
                case TypesOfFood.Sushi:
                    HPIncrease = 2;
                    break;
            }

        }
        public override string ToString()
        {

            return ($"{FoodType}| {HPIncrease}");
        }
    }
}
