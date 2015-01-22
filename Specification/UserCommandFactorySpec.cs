namespace Specification
{
    using NUnit.Framework;
    using Tarnas.ConsoleUi;

    [TestFixture]
    class UserCommandFactorySpec
    {
        private StandardUserCommandFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new StandardUserCommandFactory();
        }

        [Test]
        [TestCase("/testName")]
        [TestCase("   /testName")]
        public void ShouldCreateUserCommand(string userInput)
        {
            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Name, Is.EqualTo("testName"));
        }

        [Test]
        public void ShouldCreateUserCommandWithParams()
        {
            //given
            const string userInput = "/testName param1 param2 param3";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Name, Is.EqualTo("testName"));
            Assert.That(new []{"param1", "param2", "param3"}, Is.EquivalentTo(userCommand.Params));
        }

        [Test]
        public void ShouldCreateUserCommandWithMultiWordParams()
        {
            //given
            const string userInput = "/testName 'multi word param' param1";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Params[0], Is.EqualTo("multi word param"));
            Assert.That(userCommand.Params[1], Is.EqualTo("param1"));
        }

        [Test]
        public void ShouldCreateUserCommandWithManyMultiWordParams()
        {
            //given
            const string userInput = "/testName 'multi word 1' param1 'multi word 2' param2";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Name, Is.EqualTo("testName"));
            Assert.That(userCommand.Params[0], Is.EqualTo("multi word 1"));
            Assert.That(userCommand.Params[1], Is.EqualTo("param1"));
            Assert.That(userCommand.Params[2], Is.EqualTo("multi word 2"));
            Assert.That(userCommand.Params[3], Is.EqualTo("param2"));
        }

        [Test]
        public void ShouldRecognizeFlagByTwoDashes()
        {
            //given
            const string userInput = "/testName --flag";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Params.Count, Is.EqualTo(0));
            Assert.That(userCommand.Flags.Count, Is.EqualTo(1));
            Assert.That(userCommand.Flags, Contains.Item("flag"));
        }

        [Test]
        [ExpectedException(typeof(InvalidCommandStringException))]
        [TestCase("")]
        [TestCase("   ")]
        public void ShouldNotAcceptEmptyInput(string userInput)
        {
            //when
            _factory.CreateUserCommand(userInput);
        }

        [Test]
        [ExpectedException(typeof(InvalidCommandStringException))]
        public void ShouldNotAcceptInputsWithoutLeadingSlash()
        {
            //when
            _factory.CreateUserCommand("something something");
        }

        [Test]
        [ExpectedException(typeof(InvalidCommandStringException))]
        public void ShouldNotAcceptInputsWithNullCommandName()
        {
            //when
            _factory.CreateUserCommand("/ something");
        }
    }
}
