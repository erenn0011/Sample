using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repository
{
    internal class InventoryRepository
    {
        private readonly string _filePath;
        private readonly ConcurrentBag<InventoryItems> _inventoryItems = new ConcurrentBag<InventoryItems>();
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
            }
        };

        public InventoryRepository(string filePath)
        {
            this._filePath = filePath;
            this._inventoryItems = this.LoadAll();
            if (this._inventoryItems.Count == 0)
            {
                _inventoryItems.Add(new InventoryItems(1, "Milk", 1000, 1000));
                _inventoryItems.Add(new InventoryItems(2, "Coffee Bean", 1000, 1000));
                _inventoryItems.Add(new InventoryItems(3, "Sugar", 1000, 1000));
                _inventoryItems.Add(new InventoryItems(4, "Water", 1000, 1000));
                this.WriteAll();
            }
        }

        public void RefillStock()
        {
            foreach (var item in _inventoryItems)
            {
                item.CurrentQuantity = item.MaxQuantity;
            }
            this.WriteAll();
        }

        public IEnumerable<InventoryItems> GetAll()
        {
            return this._inventoryItems;
        }

        public void ReduceStock(IngredientRequired ingredientRequired)
        {
            InventoryItems inventoryItems = this._inventoryItems.First(item => item.Id == ingredientRequired.Id);
            inventoryItems.CurrentQuantity -= ingredientRequired.Quantity;
            this.WriteAll();
        }

        private void WriteAll()
        {
            string fileData = JsonSerializer.Serialize(this._inventoryItems, this._jsonSerializerOptions);
            File.WriteAllText(this._filePath, fileData);
        }

        private ConcurrentBag<InventoryItems> LoadAll()
        {
            if (!File.Exists(this._filePath))
            {
                return new ConcurrentBag<InventoryItems>();
            }
            string fileData = File.ReadAllText(this._filePath);
            var list = JsonSerializer.Deserialize<List<InventoryItems>>(fileData, this._jsonSerializerOptions);
            return list != null ? new ConcurrentBag<InventoryItems>(list) : new ConcurrentBag<InventoryItems>();
        }
    }
}
