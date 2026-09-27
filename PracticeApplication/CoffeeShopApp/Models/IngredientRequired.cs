namespace CoffeeShopApp.Models
{
    public class IngredientRequired
    {
        public IngredientRequired()
        {
        }
        public IngredientRequired(int Id, int Quantity)
        {
            this.Id = Id;
            this.Quantity = Quantity;
        }

        public int Id { get; set; }

        public int Quantity { get; set; }
    }
}