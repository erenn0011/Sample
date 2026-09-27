namespace ParkingGarageApp.Models
{
    internal class ParkingLevel
    {
        public ParkingLevel(int LevelNumber, int Capacity)
        {
            this.LevelNumber = LevelNumber;
            this.Capacity = Capacity;
            this.Spots = new List<ParkingSpot>();

            for (int i = 1; i <= Capacity; i++)
            {
                this.Spots.Add(new ParkingSpot(i));
            }
        }

        public int LevelNumber { get; set; }

        public int Capacity { get; set; }

        public List<ParkingSpot> Spots { get; set; }
    }
}
