using System;
using System.Diagnostics;
using Niche.Shared;

namespace vsgraph
{
    public class ConsoleLogger : ILogger
    {
        public void Heading(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.White, string.Empty, template, arguments);
        }

        public void Subheading(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.Gray, string.Empty, template, arguments);
        }

        public void Action(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.Gray, "  - ", template, arguments);
        }

        public void Detail(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.DarkGray, "  - ", template, arguments);
        }

        [Conditional("Debug")]
        public void Debug(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.Magenta, "  * ", template, arguments);
        }

        public void Success(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.Green, "  + ", template, arguments);
        }

        public void Failure(string template, params object[] arguments)
        {
            WriteLine(ConsoleColor.Red, "  x ", template, arguments);
        }

        private static void WriteLine(ConsoleColor foreground, string prefix, string template, object[] arguments)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            Console.WriteLine(prefix + template, arguments);
            Console.ForegroundColor = oldColor;
        }
    }
}
