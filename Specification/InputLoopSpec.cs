namespace Specification
{
    using Moq;
    using NUnit.Framework;
    using Tarnas.ConsoleUi;

    [TestFixture]
    class InputLoopSpec
    {
        private Mock<Subscriber> _subscriberMock;
        private Mock<Console> _consoleMock;
        private ConsoleUi _consoleUi;

        [SetUp]
        public void Setup()
        {
            _subscriberMock = new Mock<Subscriber>();
            _consoleMock = new Mock<Console>();
        }

        [Test]
        public void ShouldTakeInputFromUserAndPassItToConsoleUiUntilExitCommand()
        {
            //given
            _consoleMock.SetupSequence(mock => mock.ReadLine())
                .Returns("/test")
                .Returns("/test")
                .Returns("/quit");
            _consoleUi = new ConsoleUi();
            _consoleUi.Subscribe(_subscriberMock.Object, "test");
            var loop = new InputLoop(_consoleMock.Object, _consoleUi);

            //when
            loop.Loop();

            //then
            _subscriberMock.Verify(sub => sub.Execute(It.Is<UserCommand>(command => command.Name == "test")), Times.Exactly(2));
        }

        [Test]
        public void ShouldNotForwardEmptyInput()
        {
            //given
            _consoleMock.SetupSequence(mock => mock.ReadLine())
                .Returns(string.Empty)
                .Returns("")
                .Returns("/quit");

            var factoryMock = new Mock<UserCommandFactory>();
            _consoleUi = new ConsoleUi(factoryMock.Object);
            _consoleUi.Subscribe(_subscriberMock.Object, "test");
            var loop = new InputLoop(_consoleMock.Object, _consoleUi);

            //when
            loop.Loop();

            //then
            factoryMock.Verify(factory => factory.CreateUserCommand(It.IsAny<string>()), Times.Never);
        }
    }
}
