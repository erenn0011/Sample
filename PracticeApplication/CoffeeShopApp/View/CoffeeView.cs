using CoffeeShopApp.Enums;
using CoffeeShopApp.Services;
using CoffeeShopApp.Views;

namespace CoffeeShopApp.View
{
    internal class CoffeeView
    {
        private readonly CoffeeServices _coffeeServices;

        private readonly NotificationService _notificationService;

        private int? _currentUserId = null;

        public CancellationTokenSource cts;

        public CoffeeView(CoffeeServices coffeeServices, NotificationService notificationService, CancellationTokenSource cancellationTokenSource)
        {
            this._coffeeServices = coffeeServices;
            this._notificationService = notificationService;
            this.cts = cancellationTokenSource;
            this._notificationService.NotifierEvent += MessageNotifier;
        }
        internal void AssignCurrentUser(int userId)
        {
            this._currentUserId = userId;
        }

        public void UserMenu(int userId)
        {
            while (true)
            {
                Console.WriteLine($@"
==================================
            Coffee Shop
==================================");
                foreach (var userOptions in Enum.GetValues(typeof(CoffeeMenuOption)))
                {
                    ConsolePrinter.WriteLine($"{(int)userOptions}. {userOptions}");
                }
                ConsolePrinter.Write($"Enter Choice: ");
                int.TryParse(Console.ReadLine(), out int userChoice);

                if (userChoice is (int)CoffeeMenuOption.Logout)
                {
                    this._currentUserId = null;
                    Console.Clear();
                    return;
                }
                else if (userChoice == (int)CoffeeMenuOption.Cancel_orders)
                {
                    continue;
                }
                Console.Clear();
                this._coffeeServices.PrepareCoffee(userChoice, userId);
            }
        }

        private void MessageNotifier(string message, int userId)
        {

            if (this._currentUserId == userId)
            {
                ConsolePrinter.Notification($"{message}");
            }
        }
    }
}
