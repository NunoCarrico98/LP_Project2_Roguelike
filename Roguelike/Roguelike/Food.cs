using System;

namespace Roguelike
{
    public class Food : Item
    {
        public float HPIncrease { get; private set; }
        public TypesOfFood FoodType { get; set; }
        public Position FoodPos { get; set; }
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Food(Position pos)
        {
            FoodType = (TypesOfFood)rnd.Next(0, Enum.GetNames(typeof(TypesOfTraps)).Length);
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
                    Weight = 0.5f;
                    break;
                case TypesOfFood.Burger:
                    HPIncrease = 10;
                    Weight = 1;
                    break;
                case TypesOfFood.Noodles:
                    HPIncrease = 7;
                    Weight = 1;
                    break;
                case TypesOfFood.Pizza:
                    HPIncrease = 8;
                    Weight = 2;
                    break;
                case TypesOfFood.Sushi:
                    HPIncrease = 2;
                    Weight = 0.1f;
                    break;
            }

        }
        public override string ToString()
        {
            char plus = '+';
            return ($"{FoodType,-14}|{plus,12}{HPIncrease,4}| {Weight, 10}|");
        }
    }
}
