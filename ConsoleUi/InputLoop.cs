namespace Tarnas.ConsoleUi
{
    using System;
    using System.IO;
    using System.Linq;

    public class InputLoop : Subscriber
    {
        private readonly Console _console;
        private readonly ConsoleUi _consoleUi;
        private readonly FileReader _fileReader;
        private const string BatchCommand = "batchInput";

        public InputLoop(ConsoleUi consoleUi) : this(new SystemConsole(), consoleUi, new SystemFileReader())
        {
            
        }

        public InputLoop(Console console, ConsoleUi consoleUi, FileReader fileReader)
        {
            _console = console;
            _consoleUi = consoleUi;
            _fileReader = fileReader;

            SubscribeToBatchInput();
        }

        private void SubscribeToBatchInput()
        {
            _consoleUi.Subscribe(this, BatchCommand);
        }

        public void Loop()
        {
            string input = string.Empty;
            while ((input = _console.ReadLine()) != "/quit")
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    _consoleUi.UserInput(input);
                }
            }
        }

        public void Execute(UserCommand userCommand)
        {
            try
            {
                var filePath = userCommand.Params[0];
                var inputs = _fileReader.GetLines(filePath);

                inputs.ToList().ForEach(input => _consoleUi.UserInput(input));
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is ArgumentNullException || e is DirectoryNotFoundException)
                {
                    _console.WriteLine("File not found.");
                    return;
                }

                throw;
            }
        }
    }
}