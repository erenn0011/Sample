using System.Text.Json;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repository
{
    internal class JsonLogger
    {
        private readonly string _filePath;

        private readonly SemaphoreSlim _logSemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public JsonLogger(string filePath)
        {
            this._filePath = filePath;
        }

        internal async Task LogAsync(int userId, string Action, string Message, int OrderId, int MachineId)
        {
            await _logSemaphoreSlim.WaitAsync();
            try
            {
                List<LogEntry> _logData = await LoadAsync();

                LogEntry newLog = new LogEntry(userId, Action, DateTime.Now, Message, OrderId, MachineId);

                _logData.Add(newLog);

                string fileData = JsonSerializer.Serialize(_logData, this._jsonSerializerOptions);
                await File.WriteAllTextAsync(this._filePath, fileData);
            }
            finally
            {
                _logSemaphoreSlim.Release();
            }
        }

        internal async Task<List<LogEntry>> LoadAsync()
        {
            if (!File.Exists(this._filePath))
            {
                return new List<LogEntry>();
            }
            string fileData = await File.ReadAllTextAsync(this._filePath);
            return JsonSerializer.Deserialize<List<LogEntry>>(fileData, this._jsonSerializerOptions) ?? new List<LogEntry>();
        }
    }
}