using System.Text.Json;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repository
{
    internal class LogDataLogger
    {
        private readonly string _filePath;

        private readonly SemaphoreSlim _logSemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public LogDataLogger(string filePath)
        {
            this._filePath = filePath;
            if (!File.Exists(this._filePath))
            {
                File.AppendAllTextAsync(this._filePath, $"UserId, OrderId, MachineId, Sourcing Time, Preparation Time, Served Time\n");
            }
        }

        internal async Task LogAsync(LogData log)
        {
            await _logSemaphoreSlim.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(this._filePath, $"{log.UserId},{log.OrderId},{log.MachineId},{log.SourcingTime},{log.PreparationTime},{log.ServedTime}\n");
            }
            finally
            {
                _logSemaphoreSlim.Release();
            }
        }
    }
}