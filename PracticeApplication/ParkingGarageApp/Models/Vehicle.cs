using ParkingGarageApp.Enums;

namespace ParkingGarageApp.Models
{
    internal class Vehicle
    {
        public Vehicle(string LicensePlate, VehicleType VehicleType)
        {
            this.LicensePlate = LicensePlate;
            this.VehicleType = VehicleType;
        }

        public string LicensePlate { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}
