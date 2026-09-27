using CoffeeShopApp.Enums;

namespace CoffeeShopApp.Models
{
    internal class Coffee
    {
        public Coffee(CoffeeMenuOption Name, TimeSpan SourcingTime, TimeSpan PreparationTime, List<IngredientRequired> ingredientRequirements)
        {
            this.Name = Name;
            this.IngredientRequirements = ingredientRequirements;
            this.SourcingTime = SourcingTime;
            this.PreparationTime = PreparationTime;
        }

        public CoffeeMenuOption Name { get; set; }

        public TimeSpan SourcingTime { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public List<IngredientRequired> IngredientRequirements { get; set; }
    }
}