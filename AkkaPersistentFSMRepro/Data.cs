using Akka.Actor;

namespace AkkaPersistentFSMRepro
{
    public class Data
    {
        public static Data Create() => new Data(ActorRefs.Nobody, string.Empty);

        private Data(IActorRef @ref, string stuff)
        {
            Ref = @ref;
            Stuff = stuff;
        }

        public IActorRef Ref { get; }

        public string Stuff { get; }

        public Data SetRef(IActorRef newRef) => new Data(newRef, Stuff);

        public Data SetStuff(string newStuff) => new Data(Ref, newStuff);
    }
}
