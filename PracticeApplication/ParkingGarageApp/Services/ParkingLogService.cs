using ParkingGarageApp.Models;
using ParkingGarageApp.Repository;
using System.Threading.Channels;

namespace ParkingGarageApp.Services
{
    internal class ParkingLogService
    {
        private readonly ParkingLogRepository _parkingLogRepository;

        public ParkingLogService(ParkingLogRepository parkingLogRepository)
        {
            this._parkingLogRepository = parkingLogRepository;
        }

        internal async Task StartAsync(Channel<CompletedStay> stayChannel, CancellationToken cancellationToken)
        {
            try
            {
                await foreach (CompletedStay stay in stayChannel.Reader.ReadAllAsync())
                {
                    await this._parkingLogRepository.LogAsync(stay, CancellationToken.None);
                    Console.WriteLine($"[Logger] {stay.TicketId} saved to CSV.");
                }
            }
            catch (OperationCanceledException)
            {
                // Shutdown requested.
            }
        }
    }
}
