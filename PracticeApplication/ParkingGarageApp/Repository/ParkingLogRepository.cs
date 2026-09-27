using ParkingGarageApp.Models;

namespace ParkingGarageApp.Repository
{
    internal class ParkingLogRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _logSemaphoreSlim = new SemaphoreSlim(1, 1);

        public ParkingLogRepository(string filePath)
        {
            this._filePath = filePath;
        }

        internal async Task LogAsync(CompletedStay stay, CancellationToken cancellationToken)
        {
            await _logSemaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                if (!File.Exists(this._filePath))
                {
                    await File.AppendAllTextAsync(this._filePath,
                        "TicketId,LicensePlate,VehicleType,Level,Spot,EntryTime,ExitTime,DurationMinutes,Fee\n",
                        cancellationToken);
                }

                string line = $"{stay.TicketId},{stay.LicensePlate},{stay.VehicleType},{stay.LevelNumber},{stay.SpotNumber},{stay.EntryTime:dd-MM-yyyy HH:mm:ss},{stay.ExitTime:dd-MM-yyyy HH:mm:ss},{stay.Duration.TotalMinutes:F0},{stay.Fee:F2}\n";
                await File.AppendAllTextAsync(this._filePath, line, cancellationToken);
            }
            finally
            {
                _logSemaphoreSlim.Release();
            }
        }
    }
}
