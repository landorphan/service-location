namespace Landorphan.Ioc.Example.ConsoleApp
{
    public interface IConsole
    {
        void Write(string message);
        void WriteLine();
        void WriteLine(string message);
    }
}
