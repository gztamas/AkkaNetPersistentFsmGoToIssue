I created this small sample to reproduce an issue that I thought was a bug in Akka.net.
The issue was that the PersistentFSM state data did not properly change under some circumstance.
The problem only showed itself when one used GoTo to transition to a new state in a state handler.
Further investigation revealed that the underlying problem was that there was no state handler defined
for the state the FSM was supposed to transition to.
This is an issue on the developer's side, as the FSM documentation states that all FSM states should
have a handler.
Unfortunately when one does not have this in mind, without a proper error being thrown it is quite hard
to understand why it just won't work.

I keep the sample in case someone runs into the same issue.
