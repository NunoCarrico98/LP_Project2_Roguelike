using System;

namespace Roguelike
{
    /// <summary>
    /// Class that defines a Food object. It is a Game Object.
    /// </summary>
    [Serializable()]
    public class Food : Item
    {
        /// <summary>
        /// Property that defines how much HP the player gets when eating food.
        /// It depends on the type of food it is.
        /// </summary>
        public double HPIncrease { get; private set; }

        /// <summary>
        /// Property that defines the type of food it is.
        /// </summary>
        public TypesOfFood FoodType { get; set; }

        /// <summary>
        /// Instance variable to be able to randomize numbers.
        /// </summary>
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Constructor to initialise the food type and the HPIncrease.
        /// </summary>
        public Food()
        {
            // Food Type is Random between all types of food that exist
            FoodType = (TypesOfFood)
                rnd.Next(Enum.GetNames(typeof(TypesOfFood)).Length);
            // Call Method to Set the HPIncrease for each type of food
            SetHp();
        }

        /// <summary>
        /// Constructor to initialise the food type and the HPIncrease.
        /// </summary>
        /// <param name="typefood">Foodtype we want the food to be.</param>
        public Food(TypesOfFood typefood)
        {
            // Set Property as the type of food we want
            FoodType = typefood;
            // Call Method to Set the HPIncrease for each type of food
            SetHp();
        }

        /// <summary>
        /// Method that defines HPIncrease and Weight for all types of food.
        /// </summary>
        public void SetHp()
        {
            // Dependending on food type
            switch (FoodType)
            {
                // If it's bacon
                case TypesOfFood.Bacon:
                    // Define HPIncrease and Weight
                    HPIncrease = 4d;
                    Weight = 0.5f;
                    break;
                // If it's a burger
                case TypesOfFood.Burger:
                    // Define HPIncrease and Weight
                    HPIncrease = 10d;
                    Weight = 1.5f;
                    break;
                // If it's noodles
                case TypesOfFood.Noodles:
                    // Define HPIncrease and Weight
                    HPIncrease = 7d;
                    Weight = 1f;
                    break;
                // If it's pizza
                case TypesOfFood.Pizza:
                    // Define HPIncrease and Weight
                    HPIncrease = 15d;
                    Weight = 2f;
                    break;
                // If it's sushi
                case TypesOfFood.Sushi:
                    // Define HPIncrease and Weight
                    HPIncrease = 2d;
                    Weight = 0.1f;
                    break;
            }
        }

        /// <summary>
        /// Overriding ToString() method of class object.
        /// </summary>
        /// <returns>Returns a string with a formatted line.</returns>
        public override string ToString()
        {
            // Set + as variable
            char plus = '+';

            // Return a formatted line with food type, HPIncrease and Weight
            return ($"{FoodType,-14}|{plus,12}{HPIncrease,4}| {Weight, 10}|");
        }
    }
}
