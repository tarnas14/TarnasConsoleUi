namespace Specification
{
    using Moq;
    using NUnit.Framework;
    using Tarnas.ConsoleUi;

    [TestFixture]
    class InputLoopSpec
    {
        [Test]
        public void ShouldTakeInputFromUserAndPassItToConsoleUiUntilExitCommand()
        {
            //given
            var subscriberMock = new Mock<Subscriber>();
            var consoleMock = new Mock<Console>();
            consoleMock.SetupSequence(mock => mock.ReadLine())
                .Returns("/test")
                .Returns("/test")
                .Returns("/quit");
            var consoleUi = new ConsoleUi();
            consoleUi.Subscribe(subscriberMock.Object, "test");
            var loop = new InputLoop(consoleMock.Object, consoleUi);

            //when
            loop.Loop();

            //then
            subscriberMock.Verify(sub => sub.Execute(It.Is<UserCommand>(command => command.Name == "test")), Times.Exactly(2));
        }
    }
}
