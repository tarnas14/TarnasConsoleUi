namespace Specification
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Tarnas.ConsoleUi;

    [TestFixture]
    class UserCommandSpec
    {
        [Test]
        public void ShouldReturnNamedParamsWithTryGetMethod()
        {
            //given
            const string expectedParamValue = "testNamedParamValue";
            const string testParamName = "testNamedParam";
            var userCommand = new UserCommand
            {
                NamedParams = new Dictionary<string, string> { { testParamName, expectedParamValue } }
            };

            //when
            string input = String.Empty;
            var hasValue = userCommand.TryGetParam(testParamName, out input);

            //then
            Assert.That(hasValue, Is.True);
            Assert.That(input, Is.EqualTo(expectedParamValue));
        }

        [Test]
        public void ShouldComplyWithTryGetPatternForNamedParamsThatDontExist()
        {
            //given
            const string testParamName = "testNamedParam";
            var userCommand = new UserCommand();

            //when
            string input = String.Empty;
            var hasValue = userCommand.TryGetParam(testParamName, out input);

            //then
            Assert.False(hasValue);
            Assert.That(string.IsNullOrEmpty(input));
        }
    }
}
