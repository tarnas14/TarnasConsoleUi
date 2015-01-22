namespace Tarnas.ConsoleUi
{
    public class ConsoleUi
    {
        private readonly UserCommandFactory _userCommandFactory;
        private readonly SubscriberStore _subscribers;

        public ConsoleUi() : this(new StandardUserCommandFactory())
        {}

        public ConsoleUi(UserCommandFactory userCommandFactory)
        {
            _userCommandFactory = userCommandFactory;
            _subscribers = new SubscriberStore();
        }

        public void UserInput(string userInput)
        {
            var userCommand = _userCommandFactory.CreateUserCommand(userInput);
            var subscribers = _subscribers.GetSubsFor(userCommand.Name);

            foreach (var sub in subscribers)
            {
                sub.Execute(userCommand);
            }
        }

        public void Subscribe(Subscriber subscriber, string commandName)
        {
            _subscribers.Store(subscriber, commandName);
        }
    }
}