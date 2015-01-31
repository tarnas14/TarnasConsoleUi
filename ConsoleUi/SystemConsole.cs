namespace Tarnas.ConsoleUi
{
    public class SystemConsole : Console
    {
        public void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }

        public string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}
