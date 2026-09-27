using ParkingGarageApp.Enums;

namespace ParkingGarageApp.Models
{
    internal class CompletedStay
    {
        public string TicketId { get; set; } = string.Empty;

        public string LicensePlate { get; set; } = string.Empty;

        public VehicleType VehicleType { get; set; }

        public int LevelNumber { get; set; }

        public int SpotNumber { get; set; }

        public DateTime EntryTime { get; set; }

        public DateTime ExitTime { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal Fee { get; set; }
    }
}
