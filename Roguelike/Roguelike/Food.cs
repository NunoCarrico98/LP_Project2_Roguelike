using System;

namespace Roguelike
{
    [Serializable()]
    public class Food : Item
    {
        public double HPIncrease { get; private set; }
        public TypesOfFood FoodType { get; set; }
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Food()
        {
            FoodType = (TypesOfFood)
                rnd.Next(Enum.GetNames(typeof(TypesOfFood)).Length);
            SetHp();
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
                    HPIncrease = 4d;
                    Weight = 0.5f;
                    break;
                case TypesOfFood.Burger:
                    HPIncrease = 10d;
                    Weight = 1.5f;
                    break;
                case TypesOfFood.Noodles:
                    HPIncrease = 7d;
                    Weight = 1f;
                    break;
                case TypesOfFood.Pizza:
                    HPIncrease = 15d;
                    Weight = 2f;
                    break;
                case TypesOfFood.Sushi:
                    HPIncrease = 2d;
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
