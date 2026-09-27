using ParkingGarageApp.Enums;
using ParkingGarageApp.Models;
using ParkingGarageApp.Services;

namespace ParkingGarageApp.View
{
    internal class ParkingView
    {
        private readonly ParkingService _parkingService;
        private readonly GateService _gateService;
        private readonly CancellationTokenSource _cts;

        public ParkingView(ParkingService parkingService, GateService gateService, CancellationTokenSource cts)
        {
            this._parkingService = parkingService;
            this._gateService = gateService;
            this._cts = cts;
        }

        internal async Task RunAsync()
        {
            while (!this._cts.IsCancellationRequested)
            {
                Console.WriteLine(@"
==================================
       Parking Garage
==================================
[1] Vehicle Entry
[2] Vehicle Exit
[3] Show Parking
[4] Shutdown
==================================");

                Console.Write("Enter Choice: ");
                string? input = await Task.Run(() => Console.ReadLine());

                if (!int.TryParse(input, out int choice))
                {
                    ConsolePrinter.Error("Invalid choice.");
                    continue;
                }

                switch ((MenuOption)choice)
                {
                    case MenuOption.Entry:
                        await VehicleEntryAsync();
                        break;
                    case MenuOption.Exit:
                        await VehicleExitAsync();
                        break;
                    case MenuOption.ShowParking:
                        ShowParking();
                        break;
                    case MenuOption.Shutdown:
                        this._cts.Cancel();
                        break;
                    default:
                        ConsolePrinter.Error("Invalid choice.");
                        break;
                }
            }
        }

        private async Task VehicleEntryAsync()
        {
            Console.Write("Enter License Plate: ");
            string licensePlate = (await Task.Run(() => Console.ReadLine())) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                ConsolePrinter.Error("License plate cannot be empty.");
                return;
            }

            Console.WriteLine(@"
1. Motorcycle
2. Car
3. Van");
            Console.Write("Enter Vehicle Type: ");
            string? input = await Task.Run(() => Console.ReadLine());

            if (!int.TryParse(input, out int vehicleChoice) || vehicleChoice < 1 || vehicleChoice > 3)
            {
                ConsolePrinter.Error("Invalid vehicle type.");
                return;
            }

            Vehicle vehicle = new Vehicle(licensePlate.ToUpper(), (VehicleType)vehicleChoice);
            Ticket? ticket = this._parkingService.ParkVehicle(vehicle);

            if (ticket == null)
            {
                ConsolePrinter.Error("No suitable parking spot is available.");
                return;
            }

            PrintTicket(ticket);

            _ = RunEntryGateAsync(ticket);
        }

        private async Task VehicleExitAsync()
        {
            Console.Write("Enter Ticket ID or License Plate: ");
            string searchValue = (await Task.Run(() => Console.ReadLine())) ?? string.Empty;

            CompletedStay? stay = this._parkingService.RemoveVehicle(searchValue);

            if (stay == null)
            {
                ConsolePrinter.Error("Vehicle not found.");
                return;
            }

            PrintReceipt(stay);

            await this._parkingService.StayChannel.Writer.WriteAsync(stay, this._cts.Token);
            _ = RunExitGateAsync(stay);
        }

        private async Task RunEntryGateAsync(Ticket ticket)
        {
            try
            {
                await this._gateService.RunEntryGateAsync(ticket.LevelNumber, ticket.TicketId, this._cts.Token);
            }
            catch (OperationCanceledException)
            {
                ConsolePrinter.Notification($"Entry gate cancelled for {ticket.TicketId}.");
            }
        }

        private async Task RunExitGateAsync(CompletedStay stay)
        {
            try
            {
                await this._gateService.RunExitGateAsync(stay.LevelNumber, stay.TicketId, this._cts.Token);
            }
            catch (OperationCanceledException)
            {
                ConsolePrinter.Notification($"Exit gate cancelled for {stay.TicketId}.");
            }
        }

        private void PrintTicket(Ticket ticket)
        {
            Console.WriteLine($@"
---------- TICKET ----------
Ticket ID     : {ticket.TicketId}
License Plate : {ticket.LicensePlate}
Vehicle Type  : {ticket.VehicleType}
Level         : {ticket.LevelNumber}
Spot          : {ticket.SpotNumber}
Entry Time    : {ticket.EntryTime:dd-MM-yyyy HH:mm:ss}
----------------------------");
        }

        private void PrintReceipt(CompletedStay stay)
        {
            Console.WriteLine($@"
---------- RECEIPT ----------
Ticket ID     : {stay.TicketId}
License Plate : {stay.LicensePlate}
Vehicle Type  : {stay.VehicleType}
Level / Spot  : {stay.LevelNumber} / {stay.SpotNumber}
Entry Time    : {stay.EntryTime:dd-MM-yyyy HH:mm:ss}
Exit Time     : {stay.ExitTime:dd-MM-yyyy HH:mm:ss}
Duration      : {stay.Duration.TotalMinutes:F0} minutes
Fee           : {stay.Fee:F2}
-----------------------------");
        }

        private void ShowParking()
        {
            Console.WriteLine("\n========== PARKING ==========");

            foreach (ParkingLevel level in this._parkingService.GetLevels())
            {
                int occupied = level.Spots.Count(item => item.IsOccupied);
                Console.WriteLine($"Level {level.LevelNumber}: {occupied}/{level.Capacity} occupied");

                foreach (ParkingSpot spot in level.Spots)
                {
                    string status = spot.IsOccupied ? $"Occupied ({spot.LicensePlate})" : "Available";
                    Console.WriteLine($"  Spot {spot.SpotNumber:D2}: {status}");
                }
            }

            Console.WriteLine($"Active Vehicles: {this._parkingService.GetActiveVehicleCount()}");
            Console.WriteLine("=============================");
        }
    }
}
