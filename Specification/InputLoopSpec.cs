namespace Specification
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Moq;
    using NUnit.Framework;
    using Tarnas.ConsoleUi;
    using Console = Tarnas.ConsoleUi.Console;

    [TestFixture]
    class InputLoopSpec
    {
        private Mock<Subscriber> _subscriberMock;
        private Mock<Console> _consoleMock;
        private ConsoleUi _consoleUi;
        private Mock<UserCommandFactory> _factoryMock;

        [SetUp]
        public void Setup()
        {
            _subscriberMock = new Mock<Subscriber>();
            _consoleMock = new Mock<Console>();
            _factoryMock = new Mock<UserCommandFactory>();
            _factoryMock.Setup(factory => factory.CreateUserCommand(It.IsAny<string>())).Returns((string commandName) => new UserCommand{Name = commandName.Substring(1, commandName.Length-1)});
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
            var loop = new InputLoop(_consoleMock.Object, _consoleUi, Mock.Of<FileReader>());

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

            _consoleUi = new ConsoleUi(_factoryMock.Object);
            var loop = new InputLoop(_consoleMock.Object, _consoleUi, Mock.Of<FileReader>());

            //when
            loop.Loop();

            //then
            _factoryMock.Verify(factory => factory.CreateUserCommand(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ShouldAllowBatchInputFromTxtFile()
        {
            //given
            var fileReaderMock = new Mock<FileReader>();
            SetupBatchInputTest(fileReaderMock);
            
            _consoleUi.Subscribe(_subscriberMock.Object, "batch");
            _consoleUi.Subscribe(_subscriberMock.Object, "input");

            var loop = new InputLoop(_consoleMock.Object, _consoleUi, fileReaderMock.Object);
            
            //when
            loop.Loop();

            //then
            _factoryMock.Verify(factory => factory.CreateUserCommand("/batch"), Times.Once);
            _factoryMock.Verify(factory => factory.CreateUserCommand("/input"), Times.Once);
        }

        private void SetupBatchInputTest(Mock<FileReader> fileReaderMock)
        {
            const string filePath = "filePath";

            _consoleMock.SetupSequence(mock => mock.ReadLine())
                .Returns("/batchInput " + filePath)
                .Returns("/quit");

            _factoryMock.Setup(factory => factory.CreateUserCommand("/batchInput " + filePath)).Returns(new UserCommand
            {
                Name = "batchInput",
                Params = new[] { filePath }
            });

            fileReaderMock.Setup(mock => mock.GetLines(filePath)).Returns(new List<string>
            {
                "/batch",
                "/input"
            });

            _consoleUi = new ConsoleUi(_factoryMock.Object);
        }

        [Test]
        public void ShouldNotRunTheBatchIfFileDoesNotExist()
        {
            //given
            var fileReaderMock = new Mock<FileReader>();
            SetupBatchInputTest(fileReaderMock);

            fileReaderMock.Setup(reader => reader.GetLines(It.IsAny<string>())).Throws<FileNotFoundException>();

            var loop = new InputLoop(_consoleMock.Object, _consoleUi, fileReaderMock.Object);

            //when
            loop.Loop();

            //then
            _consoleMock.Verify(console => console.WriteLine("File not found."));
        }

        [Test]
        public void ShouldNotRunTheBatchIfFileIsNotReachable()
        {
            //given
            var fileReaderMock = new Mock<FileReader>();
            SetupBatchInputTest(fileReaderMock);

            fileReaderMock.Setup(reader => reader.GetLines(It.IsAny<string>())).Throws<DirectoryNotFoundException>();

            var loop = new InputLoop(_consoleMock.Object, _consoleUi, fileReaderMock.Object);

            //when
            loop.Loop();

            //then
            _consoleMock.Verify(console => console.WriteLine("File not found."));
        }

        [Test]
        public void ShouldNotRunTheBatchIfNoFileSpecified()
        {
            //given
            var fileReaderMock = new Mock<FileReader>();
            SetupBatchInputTest(fileReaderMock);

            fileReaderMock.Setup(reader => reader.GetLines(It.IsAny<string>())).Throws<ArgumentNullException>();

            var loop = new InputLoop(_consoleMock.Object, _consoleUi, fileReaderMock.Object);

            //when
            loop.Loop();

            //then
            _consoleMock.Verify(console => console.WriteLine("File not found."));
        }
    }
}
