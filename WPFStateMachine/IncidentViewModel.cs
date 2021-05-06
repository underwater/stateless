using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace WPFStateMachine
{
    public class IncidentViewModel : ReactiveObject
    {
        private string title;
        public IncidentStateMachine _incident { get; private set; }


        private ObservableAsPropertyHelper<States> currentState;
        public States CurrentState => currentState.Value;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => this.RaiseAndSetIfChanged(ref isBusy, value);
        }

        public IncidentViewModel()
        {

            _incident = new IncidentStateMachine();

            this.currentState = _incident
                  .WhenAnyValue(x => x.CurrentState)
                  .ToProperty(this, x => x.CurrentState);



            var canCreate = _incident.WhenAnyValue(i => i.AllowCreate);
            CreateCommand = ReactiveCommand.CreateFromTask(onCreateAsync, canCreate);

            var canExcecute = _incident.WhenAnyValue(i => i.AllowExecute);
            ExecuteCommand = ReactiveCommand.CreateFromTask(onExecuteAsync, canExcecute);

            var canValidate = _incident.WhenAnyValue(i => i.AllowValidate);
            ValidateCommand = ReactiveCommand.CreateFromTask(onValidateAsync, canValidate);

            var canDelete = _incident.WhenAnyValue(i => i.AllowDelete);
            DeleteCommand = ReactiveCommand.CreateFromTask(onDeleteAsync, canDelete);

            var canArchive = _incident.WhenAnyValue(i => i.AllowArchive);
            ArchiveCommand = ReactiveCommand.CreateFromTask(onArchiveAsync, canArchive);

            var canAbandon = _incident.WhenAnyValue(i => i.AllowAbandon);
            AbandonCommand = ReactiveCommand.CreateFromTask(onAbandonAsync, canAbandon);



        }


        public ReactiveCommand<Unit, Unit> ValidateCommand { get; }
        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> AbandonCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> ArchiveCommand { get; }


        private async Task onCreateAsync()
        {
            await DoSlowWork();
            _incident.Create();

        }

        private async Task onDeleteAsync()
        {
            await DoSlowWork();
            _incident.Delete();

        }

        private async Task onArchiveAsync()
        {
            await DoSlowWork();
            _incident.Archive();

        }

        private async Task onAbandonAsync()
        {
            await DoSlowWork();
            _incident.Abandon();

        }

        private async Task onExecuteAsync()
        {
            await DoSlowWork();
            _incident.Excecute();

        }

        private async Task onValidateAsync()
        {
            await DoSlowWork();
            _incident.Validate();

        }

        private async Task DoSlowWork()
        {
            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;
        }
    }
}
