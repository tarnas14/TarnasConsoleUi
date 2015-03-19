namespace Specification
{
    using System;
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
            const string userInput = "/testName --f";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Params.Count, Is.EqualTo(0));
            Assert.That(userCommand.Flags.Count, Is.EqualTo(1));
            Assert.That(userCommand.Flags, Contains.Item('f'));
        }

        [Test]
        public void ShouldSplitFlagExpressionIntoSingleCharacterFlags()
        {
            //given
            const string userInput = "/testName --ci";

            //when
            var userCommand = _factory.CreateUserCommand(userInput);

            //then
            Assert.That(userCommand.Flags.Count, Is.EqualTo(2));
            Assert.That(userCommand.HasFlag('c'));
            Assert.That(userCommand.HasFlag('i'));
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public void ShouldNotAcceptEmptyInput(string userInput)
        {
            //when
            TestDelegate action = () => _factory.CreateUserCommand(userInput);

            //then
            Assert.That(action, Throws.Exception.TypeOf<InvalidCommandStringException>());
        }

        [Test]
        public void ShouldNotAcceptInputsWithoutLeadingSlash()
        {
            //when
            TestDelegate action = () => _factory.CreateUserCommand("something something");

            //then
            Assert.That(action, Throws.Exception.TypeOf<NotACommandException>());
        }

        [Test]
        public void ShouldNotAcceptInputsWithNullCommandName()
        {
            //when
            TestDelegate action = () => _factory.CreateUserCommand("/ something");

            //then
            Assert.That(action, Throws.Exception.TypeOf<InvalidCommandStringException>());
        }

        [Test]
        public void ShouldExtractNamedParameters()
        {
            //given
            const string expectedParamValue = "namedParamValue";
            string userInputWithNamedParam = string.Format("/testName -namedParam {0}", expectedParamValue);
            var userCommand = _factory.CreateUserCommand(userInputWithNamedParam);

            //when
            string output = String.Empty;
            var hasValue = userCommand.TryGetParam("namedParam", out output);

            //then
            Assert.True(hasValue);
            Assert.That(output, Is.EqualTo(expectedParamValue));
        }

        [Test]
        public void NamedParamsShouldNotBeIncludedInTheRegularParamsList()
        {
            //given
            const string namedParamValue = "namedParamValue";
            string userInputWithNamedParam = string.Format("/testName -namedParam {0}", namedParamValue);

            //when
            var userCommand = _factory.CreateUserCommand(userInputWithNamedParam);

            //then
            Assert.False(userCommand.Params.Contains(namedParamValue));
        }
    }
}