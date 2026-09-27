using CoffeeShopApp.Enums;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Services
{
    internal class OrderServices
    {
        private static int _orderId = 0;
        internal Order? FetchOrderDetails(int userChoice, int userId)
        {
            int currentOrderId = Interlocked.Increment(ref _orderId);
            switch (userChoice)
            {
                case 1:
                    return new Order(
                        currentOrderId,
                        new Coffee((CoffeeMenuOption)userChoice, System.TimeSpan.FromSeconds(3), System.TimeSpan.FromSeconds(2),
                        new List<IngredientRequired>
                        {
                            new IngredientRequired(1, 50),
                            new IngredientRequired(2, 50),
                            new IngredientRequired(3, 50),
                            new IngredientRequired(4, 50),
                        }
                        ),
                        userId);
                case 2:
                    return new Order(
                        currentOrderId,
                        new Coffee((CoffeeMenuOption)userChoice, System.TimeSpan.FromSeconds(4), System.TimeSpan.FromSeconds(3),
                        new List<IngredientRequired>
                        {
                            new IngredientRequired(1, 50),
                            new IngredientRequired(2, 50),
                            new IngredientRequired(3, 50),
                            new IngredientRequired(4, 50),
                        }
                        ),
                        userId);
                case 3:
                    return new Order(
                        currentOrderId,
                        new Coffee((CoffeeMenuOption)userChoice, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromSeconds(2),
                        new List<IngredientRequired>
                        {
                            new IngredientRequired(1, 50),
                            new IngredientRequired(2, 50),
                            new IngredientRequired(3, 50),
                            new IngredientRequired(4, 50),
                        }
                        ),
                        userId);
                case 4:
                    return new Order(
                        currentOrderId,
                        new Coffee((CoffeeMenuOption)userChoice, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromSeconds(3),
                        new List<IngredientRequired>
                        {
                            new IngredientRequired(1, 50),
                            new IngredientRequired(2, 50),
                            new IngredientRequired(3, 50),
                            new IngredientRequired(4, 50),
                        }
                        ),
                        userId);
                default:
                    return null;
            }
        }
    }
}
