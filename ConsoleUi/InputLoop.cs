namespace Tarnas.ConsoleUi
{
    public class InputLoop
    {
        private readonly Console _console;
        private readonly ConsoleUi _consoleUi;

        public InputLoop(Console console, ConsoleUi consoleUi)
        {
            _console = console;
            _consoleUi = consoleUi;
        }

        public void Loop()
        {
            string input = string.Empty;
            while ((input = _console.ReadLine()) != "/quit")
            {
                _consoleUi.UserInput(input);
            }
        }
    }
}