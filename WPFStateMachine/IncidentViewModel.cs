using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace WPFStateMachine
{
    public class IncidentViewModel : ReactiveObject
    {
        private string title;
        public IncidentStateMachine _incident { get; private set; }


        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }


        private ObservableAsPropertyHelper<States> currentState;
        public States CurrentState => currentState.Value;

        public IncidentViewModel()
        {
            Title = "State Machine Example";


            _incident = new IncidentStateMachine();

            this.currentState = _incident
                  .WhenAnyValue(x => x.CurrentState)
                  .ToProperty(this, x => x.CurrentState);

            var canCreate = _incident.WhenAnyValue(i => i.AllowCreate);
            CreateCommand = ReactiveCommand.CreateFromTask(onCreateAsync, canCreate);

            var canExcecute = _incident.WhenAnyValue(i => i.AllowExecute); 
            ExecuteCommand = ReactiveCommand.CreateFromTask(onExecuteAsync, canExcecute, outputScheduler: RxApp.MainThreadScheduler);

            var canValidate = _incident.WhenAnyValue(i => i.AllowValidate);
            ValidateCommand = ReactiveCommand.CreateFromTask(onValidateAsync, canValidate, outputScheduler: RxApp.MainThreadScheduler);

            var canDelete = _incident.WhenAnyValue(i => i.AllowDelete);
            DeleteCommand = ReactiveCommand.CreateFromTask(onDeleteAsync, canDelete, outputScheduler: RxApp.MainThreadScheduler);

            var canArchive = _incident.WhenAnyValue(i => i.AllowArchive);
            ArchiveCommand = ReactiveCommand.CreateFromTask(onArchiveAsync, canArchive, outputScheduler: RxApp.MainThreadScheduler);

            var canAbandon = _incident.WhenAnyValue(i => i.AllowAbandon);
            AbandonCommand = ReactiveCommand.CreateFromTask(onAbandonAsync, canAbandon, outputScheduler: RxApp.MainThreadScheduler);

        }

        private void onCreate()
        {
            Debug.WriteLine("Created");
        }

        public ReactiveCommand<Unit, Unit> ValidateCommand { get; }
        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> AbandonCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> ArchiveCommand { get; }


        private async Task onCreateAsync()
        {
            _incident.Create();
            await DoSlowWork();
        }

        private async Task onDeleteAsync()
        {
            _incident.Delete();
            await DoSlowWork();
        }

        private async Task onArchiveAsync()
        {
            _incident.Archive();
            await DoSlowWork();
        }

        private async Task onAbandonAsync()
        {
            _incident.Abandon();
            await DoSlowWork();
        }

        private async Task onExecuteAsync()
        {
            _incident.Excecute();
            await DoSlowWork();
        }

        private async Task onValidateAsync()
        {
            await DoSlowWork();
        }

        private async Task DoSlowWork() => await Task.Delay(2000);

    }
}
