namespace ParkingGarageApp.Services
{
    internal class GateService
    {
        internal async Task RunEntryGateAsync(int levelNumber, string ticketId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Gate] Level {levelNumber} entry gate opening for {ticketId}...");
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            Console.WriteLine($"[Gate] Level {levelNumber} entry gate closed for {ticketId}.");
        }

        internal async Task RunExitGateAsync(int levelNumber, string ticketId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Gate] Level {levelNumber} exit gate opening for {ticketId}...");
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            Console.WriteLine($"[Gate] Level {levelNumber} exit gate closed for {ticketId}.");
        }
    }
}
