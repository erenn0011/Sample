using ParkingGarageApp.Repository;
using ParkingGarageApp.Services;
using ParkingGarageApp.View;

namespace ParkingGarageApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            int level1Capacity = ReadCapacity("Level 1", 20);
            int level2Capacity = ReadCapacity("Level 2", 15);
            int level3Capacity = ReadCapacity("Level 3", 10);

            ParkingService parkingService = new ParkingService(level1Capacity, level2Capacity, level3Capacity);
            GateService gateService = new GateService();
            ParkingLogRepository parkingLogRepository = new ParkingLogRepository("CompletedStays.csv");
            ParkingLogService parkingLogService = new ParkingLogService(parkingLogRepository);
            ParkingView parkingView = new ParkingView(parkingService, gateService, cts);

            Task loggerTask = parkingLogService.StartAsync(parkingService.StayChannel, cts.Token);

            try
            {
                await parkingView.RunAsync();
            }
            finally
            {
                parkingService.StayChannel.Writer.TryComplete();

                try
                {
                    await loggerTask;
                }
                catch (OperationCanceledException)
                {
                    // Shutdown requested.
                }

                Console.WriteLine("Parking garage shutting down...");
                await Task.Delay(500);
            }
        }

        private static int ReadCapacity(string levelName, int defaultCapacity)
        {
            Console.Write($"Enter capacity for {levelName} (default {defaultCapacity}): ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int capacity) && capacity > 0)
            {
                return capacity;
            }

            return defaultCapacity;
        }
    }
}
