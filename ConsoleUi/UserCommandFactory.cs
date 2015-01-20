namespace Tarnas.ConsoleUi
{
    public interface UserCommandFactory
    {
        UserCommand CreateUserCommand(string userInput);
    }
}