using CoffeeShopApp.Services;

namespace CoffeeShopApp.View
{
    internal class UserView
    {
        private readonly UserServices _userServices;

        private readonly CoffeeView _coffeeView;

        public UserView(CoffeeView coffeeView, UserServices userServices)
        {
            this._coffeeView = coffeeView;
            this._userServices = userServices;
        }
        internal void SignUp()
        {
            Console.WriteLine($@"
==================================
            SIGN UP
==================================
");
            Console.Write($"User Id: ");
            int.TryParse(Console.ReadLine(), out int userId);
            Console.Write($"User Name: ");
            string userName = Console.ReadLine()!;
            Console.Write($"Password: ");
            string password = Console.ReadLine()!;
            if (this._userServices.AddUser(userId, userName, password))
            {
                ViewHelper.WriteMessage($"User is signed up");
            }
            else
            {
                ViewHelper.WriteWarning($"Invalid user data");
            }
            ViewHelper.CleanConsole();
        }

        internal void LogIn()
        {
            Console.WriteLine($@"
==================================
            LOG IN
==================================
");
            Console.Write($"User Id: ");
            int.TryParse(Console.ReadLine(), out int userId);

            Console.Write($"Password: ");
            string password = Console.ReadLine()!;

            if (this._userServices.CheckUser(userId, password))
            {
                this._coffeeView.AssignCurrentUser(userId);
                ViewHelper.WriteMessage($"User is logged in");
                ViewHelper.CleanConsole();
                this._coffeeView.UserMenu(userId);
            }
            else
            {
                ViewHelper.WriteWarning($"Invalid user data");
                ViewHelper.CleanConsole();
                ViewHelper.CleanConsole();
            }
        }
    }
}
