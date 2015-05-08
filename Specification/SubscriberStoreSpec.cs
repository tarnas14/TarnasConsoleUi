namespace Specification
{
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Tarnas.ConsoleUi;

    [TestFixture]
    class SubscriberStoreSpec
    {
        [Test]
        public void ShouldStoreSubscribersForCommands()
        {
            //given
            var subMock = new Mock<Subscriber>();
            var subStore = new SubscriberStore();

            //when
            subStore.Store(subMock.Object, "commandName");
            var subs = subStore.GetSubsFor("commandName");

            //then
            Assert.That(subs.Count(), Is.EqualTo(1));
            Assert.That(subs.First(), Is.EqualTo(subMock.Object));
        }

        [Test]
        public void ShouldThrowOnCommandNotInRegister()
        {
            //given
            const string notRegisteredCommand = "/asdf";
            var subStore = new SubscriberStore();

            //when
            TestDelegate gettingSubscribersForUnregisteredCommand = () => subStore.GetSubsFor(notRegisteredCommand);

            //then
            Assert.That(gettingSubscribersForUnregisteredCommand, Throws.InstanceOf<NobodyListensToThisCommand>());
        }
    }
}
