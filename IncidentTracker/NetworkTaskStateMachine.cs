using Stateless;
using Stateless.Graph;
using System;
using System.Linq;

namespace IncidentTracker
{

    public class NetworkTaskStateMachine<T> where T : class
    {
        StateMachine<States, Triggers> _machine;
        public States CurrentState { get; private set; }

        public NetworkTaskStateMachine()
        {
            _machine = new StateMachine<States, Triggers>(() => CurrentState, s => CurrentState = s);
            InitializeTransitions();
            InitializeLogger();
        }
        // TODO: User Ilogger
        protected virtual void InitializeLogger()
        {
            _machine.OnTransitioned(t => Console.WriteLine($"{t.Trigger}"));
            _machine.OnTransitionCompleted(t => Console.WriteLine($"  {t.Source}-->{t.Destination}\n"));
        }
        protected virtual void InitializeTransitions()
        {
            // configure transitions
            _machine.Configure(States.Undefined)
                .Permit(Triggers.Create, States.Active);

            //.OnEntry(() => { })
            //.OnExit(() => { });

            _machine.Configure(States.Active)
                .PermitReentry(Triggers.Edit)
                .Permit(Triggers.Validate, States.Validated)
                .Permit(Triggers.Delete, States.Deleted);


            _machine.Configure(States.Validated)
                .Permit(Triggers.Execute, States.Executed)
                 .Permit(Triggers.Abandon, States.Abandoned);

            _machine.Configure(States.Executed)
                .Permit(Triggers.Archive, States.Archived);

        }

        protected string GetAllowedTransitions() => string.Join(",", _machine.GetPermittedTriggers().ToList());
        protected string AllowedTransitions() =>    $"State: {CurrentState} --> Allowed [{GetAllowedTransitions()}]";

        public void Fire(Triggers trigger) {
            
            Console.WriteLine(AllowedTransitions());

            _machine.Fire(trigger);
        }
        


        public string GetInfo()
        {
            string graph = UmlDotGraph.Format(_machine.GetInfo());
            return graph;
        }


    }
}
