using CoffeeShopApp.Models;
using CoffeeShopApp.Repository;

namespace CoffeeShopApp.Services
{
    internal class InventoryService
    {
        private InventoryRepository _inventoryRepository;

        public InventoryService(InventoryRepository inventoryRepository)
        {
            this._inventoryRepository = inventoryRepository;
            this.RestockInventory();
        }

        internal bool CheckStock(Coffee coffeeOrdered)
        {
            IEnumerable<InventoryItems> inventoryItems = this._inventoryRepository.GetAll();
            foreach (IngredientRequired ingredient in coffeeOrdered.IngredientRequirements)
            {
                if (inventoryItems.First(item => item.Id == ingredient.Id).CurrentQuantity < ingredient.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool ReduceStock(Coffee coffeeOrdered)
        {
            lock (this)
            {
                if (this.CheckStock(coffeeOrdered))
                {
                    foreach (var item in coffeeOrdered.IngredientRequirements)
                    {
                        this._inventoryRepository.ReduceStock(item);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void RestockInventory()
        {
            System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(60));
            timer.Elapsed += Restock;
            timer.Start();
        }

        private void Restock(object? sender, System.Timers.ElapsedEventArgs e)
        {
            this._inventoryRepository.RefillStock();
        }
    }
}