using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace AkkaPersistentFSMRepro.Tests
{
    [TestFixture]
    public class SampleActorTests : TestKit
    {
        [TearDown]
        public async Task TearDown()
        {
            await Sys.Terminate();
        }


        [Test]
        public void ShouldUpdateRef()
        {
            var depProbe = CreateTestProbe();
            var refProbe = CreateTestProbe();

            var subject = ActorOfAsTestActorRef<SampleActor>(Props.Create(() => new SampleActor(depProbe.Ref)));

            subject.Tell(new Commands.SetRef(refProbe.Ref));

            AwaitAssert(() => subject.UnderlyingActor.StateData.Ref.Should().Be(refProbe.Ref));
        }

        [Test]
        public void ShouldUpdateStuffWithRef()
        {
            var depProbe = CreateTestProbe();
            var refProbe = CreateTestProbe();
            var newStuff = "Heyy";

            var subject = ActorOfAsTestActorRef<SampleActor>(Props.Create(() => new SampleActor(depProbe.Ref)));
            subject.Tell(new Commands.SetRef(refProbe.Ref));
            AwaitAssert(() => subject.UnderlyingActor.StateData.Ref.Should().Be(refProbe.Ref));

            subject.Tell(new Commands.UpdateStuff(newStuff));

            AwaitAssert(() => subject.UnderlyingActor.StateData.Stuff.Should().Be(newStuff));
        }

        [Test]
        public void ShouldUpdateStuffWithoutRef()
        {
            var depProbe = CreateTestProbe();
            var newStuff = "Heyya";
            var subject = ActorOfAsTestActorRef<SampleActor>(Props.Create(() => new SampleActor(depProbe.Ref)));
            subject.Tell(new Commands.UpdateStuff(newStuff));
            AwaitAssert(() => subject.UnderlyingActor.StateData.Stuff.Should().Be(newStuff));
        }
    }
}
