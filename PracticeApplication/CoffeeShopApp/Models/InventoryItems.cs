namespace CoffeeShopApp.Models
{
    internal class InventoryItems
    {
        public InventoryItems()
        {
        }

        public InventoryItems(int id, string Name, int currentQuantity, int maxQuantity)
        {
            this.Id = id;
            this.Name = Name;
            this.CurrentQuantity = currentQuantity;
            this.MaxQuantity = maxQuantity;
        }

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CurrentQuantity { get; set; }

        public int MaxQuantity { get; set; }
    }
}