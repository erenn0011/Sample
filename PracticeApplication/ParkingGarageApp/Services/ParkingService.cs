using ParkingGarageApp.Enums;
using ParkingGarageApp.Models;
using System.Threading.Channels;

namespace ParkingGarageApp.Services
{
    internal class ParkingService
    {
        private readonly List<ParkingLevel> _levels;
        private readonly Dictionary<string, Ticket> _tickets = new Dictionary<string, Ticket>();
        private readonly object _parkingLock = new object();
        private readonly Channel<CompletedStay> _stayChannel;

        private int _ticketNumber = 0;

        public ParkingService(int level1Capacity, int level2Capacity, int level3Capacity)
        {
            this._levels = new List<ParkingLevel>
            {
                new ParkingLevel(1, level1Capacity),
                new ParkingLevel(2, level2Capacity),
                new ParkingLevel(3, level3Capacity)
            };

            this._stayChannel = Channel.CreateUnbounded<CompletedStay>();
        }

        internal Channel<CompletedStay> StayChannel => this._stayChannel;

        internal Ticket? ParkVehicle(Vehicle vehicle)
        {
            lock (this._parkingLock)
            {
                foreach (ParkingLevel level in this._levels)
                {
                    if (!CanPark(vehicle.VehicleType, level.LevelNumber))
                    {
                        continue;
                    }

                    ParkingSpot? spot = level.Spots.FirstOrDefault(item => !item.IsOccupied);

                    if (spot == null)
                    {
                        continue;
                    }

                    spot.IsOccupied = true;
                    spot.LicensePlate = vehicle.LicensePlate;

                    int ticketNumber = Interlocked.Increment(ref this._ticketNumber);
                    string ticketId = $"T-{ticketNumber:D4}";

                    Ticket ticket = new Ticket(ticketId, vehicle.LicensePlate, vehicle.VehicleType, level.LevelNumber, spot.SpotNumber, DateTime.Now);

                    this._tickets.Add(ticketId, ticket);
                    return ticket;
                }
            }

            return null;
        }

        internal Ticket? FindTicket(string ticketOrPlate)
        {
            lock (this._parkingLock)
            {
                if (this._tickets.TryGetValue(ticketOrPlate, out Ticket? ticket))
                {
                    return ticket;
                }

                return this._tickets.Values.FirstOrDefault(item => item.LicensePlate.Equals(ticketOrPlate, StringComparison.OrdinalIgnoreCase));
            }
        }

        internal CompletedStay? RemoveVehicle(string ticketOrPlate)
        {
            lock (this._parkingLock)
            {
                Ticket? ticket = FindTicket(ticketOrPlate);

                if (ticket == null)
                {
                    return null;
                }

                DateTime exitTime = DateTime.Now;
                TimeSpan duration = exitTime - ticket.EntryTime;
                decimal fee = CalculateFee(ticket.VehicleType, duration);

                ParkingLevel level = this._levels.First(item => item.LevelNumber == ticket.LevelNumber);
                ParkingSpot spot = level.Spots.First(item => item.SpotNumber == ticket.SpotNumber);
                spot.IsOccupied = false;
                spot.LicensePlate = null;

                this._tickets.Remove(ticket.TicketId);

                return new CompletedStay
                {
                    TicketId = ticket.TicketId,
                    LicensePlate = ticket.LicensePlate,
                    VehicleType = ticket.VehicleType,
                    LevelNumber = ticket.LevelNumber,
                    SpotNumber = ticket.SpotNumber,
                    EntryTime = ticket.EntryTime,
                    ExitTime = exitTime,
                    Duration = duration,
                    Fee = fee
                };
            }
        }

        internal List<ParkingLevel> GetLevels()
        {
            lock (this._parkingLock)
            {
                return this._levels;
            }
        }

        internal int GetActiveVehicleCount()
        {
            lock (this._parkingLock)
            {
                return this._tickets.Count;
            }
        }

        private bool CanPark(VehicleType vehicleType, int levelNumber)
        {
            return vehicleType switch
            {
                VehicleType.Motorcycle => true,
                VehicleType.Car => levelNumber == 1 || levelNumber == 2,
                VehicleType.Van => levelNumber == 1,
                _ => false
            };
        }

        private decimal CalculateFee(VehicleType vehicleType, TimeSpan duration)
        {
            int blocks = Math.Max(1, (int)Math.Ceiling(duration.TotalMinutes / 15));

            decimal firstHourPrice;
            decimal additionalBlockPrice;

            switch (vehicleType)
            {
                case VehicleType.Motorcycle:
                    firstHourPrice = 20;
                    additionalBlockPrice = 5;
                    break;
                case VehicleType.Car:
                    firstHourPrice = 40;
                    additionalBlockPrice = 10;
                    break;
                case VehicleType.Van:
                    firstHourPrice = 60;
                    additionalBlockPrice = 15;
                    break;
                default:
                    return 0;
            }

            decimal fee;

            if (blocks <= 4)
            {
                fee = firstHourPrice;
            }
            else
            {
                fee = firstHourPrice + ((blocks - 4) * additionalBlockPrice);
            }

            decimal dailyCap = firstHourPrice + (28 * additionalBlockPrice);
            return Math.Min(fee, dailyCap);
        }
    }
}
