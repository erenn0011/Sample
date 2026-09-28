namespace CoffeeShopApp.Views
{
    /// <summary>
    /// Display class is used for displaying output
    /// </summary>
    internal static class ConsolePrinter
    {
        private const int NotificationWidth = 70;
        private static int _nextNotificationLine = 0;
        private const int MaxNotificationRows = 10;
        private static readonly object LockObject = new object();

        public static void Write(string message)
        {
            lock (LockObject)
            {
                Console.Write(message);
            }
        }

        public static void WriteLine(string message)
        {
            lock (LockObject)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// to print the message in red
        /// </summary>
        /// <param name="message">the message that has to be printed in red</param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// to print the message in Green
        /// </summary>
        /// <param name="message">the message that has to be printed in Green</param>
        public static void Success(string message)
        {
            lock (LockObject)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void Notification(string message)
        {
            int assignedRow;

            lock (LockObject)
            {
                int originalLeft = Console.CursorLeft;
                int originalTop = Console.CursorTop;

                assignedRow = _nextNotificationLine;
                _nextNotificationLine = (_nextNotificationLine + 1) % MaxNotificationRows;

                int rightCorner = Console.WindowWidth - NotificationWidth;

                Console.SetCursorPosition(rightCorner, assignedRow);
                Console.Write(new string(' ', NotificationWidth));
                Console.SetCursorPosition(rightCorner, assignedRow);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{message}]");
                Console.ResetColor();
                Console.SetCursorPosition(originalLeft, originalTop);
            }
            StartClearTimer(assignedRow);
        }

        private static void StartClearTimer(int rowToClear)
        {
            System.Timers.Timer timer = new System.Timers.Timer(4000);
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) =>
            {
                lock (LockObject)
                {
                    int originalLeft = Console.CursorLeft;
                    int originalTop = Console.CursorTop;

                    int rightCorner = Console.WindowWidth - NotificationWidth;
                    Console.SetCursorPosition(rightCorner, rowToClear);
                    Console.Write(new string(' ', NotificationWidth));
                    Console.SetCursorPosition(originalLeft, originalTop);
                }
                timer.Dispose();
            };
            timer.Start();
        }
    }
}