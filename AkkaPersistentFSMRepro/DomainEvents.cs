using Akka.Actor;

namespace AkkaPersistentFSMRepro
{
    public static class DomainEvents
    {
        public interface IDomainEvent { }

        public class StuffSet : IDomainEvent
        {
            public StuffSet(string newStuff)
            {
                NewStuff = newStuff;
            }

            public string NewStuff { get; }
        }

        public class RefSet : IDomainEvent
        {
            public IActorRef NewRef { get; }

            public RefSet(IActorRef newRef)
            {
                NewRef = newRef;
            }
        }
    }
}
