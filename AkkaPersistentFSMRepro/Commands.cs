using Akka.Actor;

namespace AkkaPersistentFSMRepro
{
    public static class Commands
    {
        public class UpdateStuff
        {
            public string Stuff { get; }

            public UpdateStuff(string stuff)
            {
                Stuff = stuff;
            }
        }

        public class SetRef
        {
            public SetRef(IActorRef @ref)
            {
                Ref = @ref;
            }

            public IActorRef Ref { get; }
        }

        public class GimmeRef
        {            
        }
    }
}
