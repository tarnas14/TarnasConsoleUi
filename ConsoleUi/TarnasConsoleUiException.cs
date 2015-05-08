namespace Tarnas.ConsoleUi
{
    using System;

    public class TarnasConsoleUiException : Exception
    {
        public TarnasConsoleUiException()
        {
        }

        public TarnasConsoleUiException(string message) : base(message)
        {
        }
    }
}