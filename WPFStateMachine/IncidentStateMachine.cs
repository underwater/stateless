using ReactiveUI;
using Stateless;
using Stateless.Graph;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPFStateMachine
{
    public enum Triggers
    {
        Create,
        Validate,
        Delete,
        Execute,
        Archive,
        Abandon
    }

    public enum States
    {
        Undefined,
        Active,
        Validated,
        Executed,
        Archived,
        Abandoned,
        Deleted
    }
    public class IncidentStateMachine : INotifyPropertyChanged
    {
        StateMachine<States, Triggers> _machine;
        private States currentState;

        public States CurrentState { 
            get => currentState; 
            private set { currentState = value; 
                OnPropertyChanged(); 
            } }


        public event PropertyChangedEventHandler PropertyChanged;




        public bool AllowCreate => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Create);
        public bool AllowExecute => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Execute);
        public bool AllowValidate => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Validate);
        public bool AllowAbandon => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Abandon);
        public bool AllowArchive => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Archive);
        public bool AllowDelete => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Delete);



        public IncidentStateMachine()
        {
            _machine = new StateMachine<States, Triggers>(() => CurrentState, s => CurrentState = s);
            DefineTransitions();
            InitializeLogger();
            var graph = GetInfo();
        }
        // TODO: User Ilogger
        protected virtual void InitializeLogger()
        {
            _machine.OnTransitioned(t => Console.WriteLine($"{t.Trigger}"));
            _machine.OnTransitionCompleted(t => Console.WriteLine($"  {t.Source}-->{t.Destination}\n"));
        }
        protected virtual void DefineTransitions()
        {
            // configure transitions
            _machine.Configure(States.Undefined)
                .Permit(Triggers.Create, States.Active);

            //.OnEntry(() => { })
            //.OnExit(() => { });

            _machine.Configure(States.Active)
                .Permit(Triggers.Validate, States.Validated)
                .Permit(Triggers.Delete, States.Deleted);


            _machine.Configure(States.Validated)
                .Permit(Triggers.Execute, States.Executed)
                 .Permit(Triggers.Abandon, States.Abandoned);

            _machine.Configure(States.Executed)
                .Permit(Triggers.Archive, States.Archived);

        }

        // generic method to transition
        //public void Handle(Triggers trigger)
        //{
        //    _machine.Fire(trigger);
        //    NotifyPropertiesChanged();
        //}

        public void Create()
        {
            _machine.Fire(Triggers.Create);
            NotifyPropertiesChanged();
        }
        public void Excecute()
        {
            _machine.Fire(Triggers.Execute);
            NotifyPropertiesChanged();
        }

        public void Abandon()
        {
            _machine.Fire(Triggers.Abandon);
            NotifyPropertiesChanged();
        }
        public void Validate()
        {
            _machine.Fire(Triggers.Validate);
            NotifyPropertiesChanged();
        }
        public void Archive()
        {
            _machine.Fire(Triggers.Archive);
            NotifyPropertiesChanged();
        }

        public void Delete()
        {
            _machine.Fire(Triggers.Delete);
            NotifyPropertiesChanged();
        }

        public void Execute()
        {
            _machine.Fire(Triggers.Execute);
            NotifyPropertiesChanged();
        }
        private void NotifyPropertiesChanged()
        {
            OnPropertyChanged();
            OnPropertyChanged(nameof(AllowCreate));
            OnPropertyChanged(nameof(AllowExecute));
            OnPropertyChanged(nameof(AllowValidate));
            OnPropertyChanged(nameof(AllowAbandon));
            OnPropertyChanged(nameof(AllowArchive));
            OnPropertyChanged(nameof(AllowDelete));
            // OnPropertyChanged(nameof(CurrentState));
        }


        public string GetInfo()
        {
            string graph = UmlDotGraph.Format(_machine.GetInfo());
            return graph;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
