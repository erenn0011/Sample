namespace CoffeeShopApp.Models
{
    internal class Machine
    {
        public int MachineId { get; set; }

        public int? OrderId { get; set; }

        public bool IsAvailable { get; set; }
    }
}