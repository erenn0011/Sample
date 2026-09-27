using CoffeeShopApp.Repository;
using CoffeeShopApp.Services;
using CoffeeShopApp.View;

namespace CoffeeShopApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) => { e.Cancel = true; };

            using CancellationTokenSource cts = new CancellationTokenSource();
            UserRepository userRepository = new UserRepository("Users.json");
            JsonLogger jsonLogger = new JsonLogger("Log.json");
            InventoryRepository inventoryRepository = new InventoryRepository("Inventory.json");
            LogDataLogger logDataLogger = new LogDataLogger("LogData.csv");

            UserServices userServices = new UserServices(userRepository);
            NotificationService notificationService = new NotificationService();
            OrderServices orderServices = new OrderServices();
            InventoryService inventoryService = new InventoryService(inventoryRepository);
            CoffeeServices coffeeServices = new CoffeeServices(notificationService, orderServices, jsonLogger, inventoryService, logDataLogger, cts);

            CoffeeView coffeeView = new CoffeeView(coffeeServices, notificationService, cts);
            UserView userView = new UserView(coffeeView, userServices);

            while (true)
            {
                string userMenu = $@"
==================================
            User Auth
==================================
[S]ign Up
[L]og In
[E]xit
Enter Choice: ";
                Console.Write(userMenu);
                ConsoleKey userChoice = Console.ReadKey().Key;
                Console.Clear();
                switch (userChoice)
                {
                    case ConsoleKey.S:
                        userView.SignUp();
                        break;
                    case ConsoleKey.L:
                        userView.LogIn();
                        break;
                    case ConsoleKey.E:
                        ViewHelper.WriteColored($"Exiting...", ConsoleColor.Blue);
                        Thread.Sleep(1200);
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
