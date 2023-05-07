using System;

namespace Common.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string errorMessage)
        {
            var tempForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = tempForeColor;
        }

        public void LogWarn(string warnMessage)
        {
            var tempForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(warnMessage);
            Console.ForegroundColor = tempForeColor;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

    }
}