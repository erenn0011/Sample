namespace CoffeeShopApp.Services
{
    internal class NotificationService
    {
        public delegate void Notifier(string message, int userId);

        public event Notifier? NotifierEvent;

        public void NotifyUser(string message, int userId)
        {
            NotifierEvent?.Invoke(message, userId);
        }
    }
}
