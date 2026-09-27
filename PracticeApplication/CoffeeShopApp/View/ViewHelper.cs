using CoffeeShopApp.Views;

namespace CoffeeShopApp.View
{
    internal static class ViewHelper
    {
        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{message}");
            Console.ResetColor();
        }

        public static void WriteMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{message}");
            Console.ResetColor();
        }
        public static void WriteColored(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"{message}");
            Console.ResetColor();
        }

        public static void CleanConsole()
        {
            ViewHelper.WriteColored($"\nEnter a key to continue...", ConsoleColor.Yellow);
            Console.ReadKey();
            Console.Clear();
        }
    }
}
