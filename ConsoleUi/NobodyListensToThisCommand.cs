namespace Tarnas.ConsoleUi
{
    public class NobodyListensToThisCommand : TarnasConsoleUiException
    {
        public NobodyListensToThisCommand(string commandName)
            : base(string.Format("Nobody registered for commad '{0}'", commandName))
        {
        }
    }
}