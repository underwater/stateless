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
        Edit,
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
    public class IncidentStateMachine :INotifyPropertyChanged
    {
        StateMachine<States, Triggers> _machine;

        public event PropertyChangedEventHandler PropertyChanged;

        public States CurrentState { get; private set; }

        public IncidentStateMachine()
        {
            _machine = new StateMachine<States, Triggers>(() => CurrentState, s => CurrentState = s);
            InitializeTransitions();
            InitializeLogger();
            var graph = GetInfo();
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


        public bool AllowCreate =>  _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Create);
        public bool AllowEdit => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Edit);
        public bool AllowExecute => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Execute);
        public bool AllowValidate => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Validate);
        public bool AllowAbandon => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Abandon);
        public bool AllowArchive => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Archive);
        public bool AllowDelete => _machine.PermittedTriggers.Any(trigger => trigger == Triggers.Delete);

        public void Create() { 
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
        public void Edit()
        {
            _machine.Fire(Triggers.Edit);
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
            OnPropertyChanged(nameof(AllowCreate));
            OnPropertyChanged(nameof(AllowEdit));
            OnPropertyChanged(nameof(AllowExecute));
            OnPropertyChanged(nameof(AllowValidate));
            OnPropertyChanged(nameof(AllowAbandon));
            OnPropertyChanged(nameof(AllowArchive));
            OnPropertyChanged(nameof(AllowDelete));
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
