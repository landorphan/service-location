namespace Landorphan.Ioc.Example.Tests.Simulators
{
    using System.Text;
    using Landorphan.Ioc.Example.ConsoleApp;

    internal class ConsoleSimulator : IConsole
    {
        private StringBuilder builder = new StringBuilder();

        public void Write(string message)
        {
            builder.Append(message);
        }

        public void WriteLine()
        {
            builder.AppendLine();
        }

        public void WriteLine(string message)
        {
            builder.AppendLine(message);
        }

        public void Clear()
        {
            builder = new StringBuilder();
        }

        public string GetConsoleOutput()
        {
            return builder.ToString();
        }
    }
}
