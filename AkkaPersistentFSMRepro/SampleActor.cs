using System;
using Akka.Actor;
using Akka.Persistence.Fsm;

namespace AkkaPersistentFSMRepro
{
    public class SampleActor : PersistentFSM<IState, Data, DomainEvents.IDomainEvent>
    {
        public override string PersistenceId => Self.Path.Name;

        public SampleActor(IActorRef dependency)
        {
            StartWith(DefaultState.Instance, Data.Create());

            When(DefaultState.Instance, (evt, state) =>
            {
                if (evt.FsmEvent is Commands.SetRef)
                {
                    var command = (Commands.SetRef)evt.FsmEvent;
                    var @event = new DomainEvents.RefSet(command.Ref);

                    return Stay().Applying(@event).AndThen(x => Context.System.EventStream.Publish(@event));
                }

                if (evt.FsmEvent is Commands.UpdateStuff)
                {
                    var command = (Commands.UpdateStuff) evt.FsmEvent;
                    var @event = new DomainEvents.StuffSet(command.Stuff);

                    var hasNoRef = StateData.Ref.IsNobody();

                    if (hasNoRef)
                    {
                        //return GoTo(WaitingState.Instance)
                        return Stay()
                            .Applying(@event)
                            .AndThen(x =>
                            {
                                dependency.Tell(new Commands.GimmeRef());
                                Context.System.EventStream.Publish(@event);
                            });
                    }

                    return Stay()
                        .Applying(@event)
                        .AndThen(x => Context.System.EventStream.Publish(@event));
                }
                return Stay();
            });
        }

        protected override Data ApplyEvent(DomainEvents.IDomainEvent e, Data currentData)
        {
            if (e is DomainEvents.StuffSet)
            {
                return currentData.SetStuff(((DomainEvents.StuffSet) e).NewStuff);
            }

            if (e is DomainEvents.RefSet)
            {
                return currentData.SetRef(((DomainEvents.RefSet) e).NewRef);
            }

            return currentData;
        }
    }

    public interface IState : PersistentFSM.IFsmState { }

    public class DefaultState : IState
    {
        public static readonly DefaultState Instance = new DefaultState();
        private DefaultState() { }
        public string Identifier => "Default";
    }

    public class WaitingState : IState
    {
        public static readonly WaitingState Instance = new WaitingState();
        private WaitingState() { }
        public string Identifier => "Waiting";
    }
}
