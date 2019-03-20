using System;

namespace seq
{
    public static class BeautifulLogger
    {
        public static void Input(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            ResetColor();
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            ResetColor();
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            ResetColor();
        }

        public static void Err(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            ResetColor();
        }

        private static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}