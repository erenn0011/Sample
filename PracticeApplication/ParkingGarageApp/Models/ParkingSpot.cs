namespace ParkingGarageApp.Models
{
    internal class ParkingSpot
    {
        public ParkingSpot(int SpotNumber)
        {
            this.SpotNumber = SpotNumber;
        }

        public int SpotNumber { get; set; }

        public bool IsOccupied { get; set; }

        public string? LicensePlate { get; set; }
    }
}
