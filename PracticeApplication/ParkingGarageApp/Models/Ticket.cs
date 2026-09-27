using ParkingGarageApp.Enums;

namespace ParkingGarageApp.Models
{
    internal class Ticket
    {
        public Ticket(string TicketId, string LicensePlate, VehicleType VehicleType, int LevelNumber, int SpotNumber, DateTime EntryTime)
        {
            this.TicketId = TicketId;
            this.LicensePlate = LicensePlate;
            this.VehicleType = VehicleType;
            this.LevelNumber = LevelNumber;
            this.SpotNumber = SpotNumber;
            this.EntryTime = EntryTime;
        }

        public string TicketId { get; set; }

        public string LicensePlate { get; set; }

        public VehicleType VehicleType { get; set; }

        public int LevelNumber { get; set; }

        public int SpotNumber { get; set; }

        public DateTime EntryTime { get; set; }
    }
}
