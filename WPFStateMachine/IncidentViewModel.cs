using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;

namespace WPFStateMachine
{
    public class IncidentViewModel : ReactiveObject
    {
        private string title;
        private IncidentStateMachine _incident;

        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        public IncidentViewModel()
        {
            Title = "State Machine Example";
            Debug.WriteLine("State Machine Example!\n\n");

            _incident = new IncidentStateMachine();

               var canExcecute = this.WhenAnyValue(x => x._incident, i=> i.AllowExecute);
               ExecuteCommand = ReactiveCommand.CreateFromTask(onExecuteAsync, canExcecute);

            var canEdit = this.WhenAnyValue(x => x._incident, i => i.AllowEdit);
            EditCommand = ReactiveCommand.CreateFromTask(onEditAsync, canEdit);


            var canValidate = this.WhenAnyValue(x => x._incident, i => i.AllowValidate);
            ValidateCommand = ReactiveCommand.CreateFromTask(onValidateAsync, canValidate);


            var canDelete = this.WhenAnyValue(x => x._incident, i => i.AllowDelete);
            DeleteCommand = ReactiveCommand.CreateFromTask(onDeleteAsync, canDelete);


            var canArchive = this.WhenAnyValue(x => x._incident, i => i.AllowArchive);
            ArchiveCommand = ReactiveCommand.CreateFromTask(onArchiveAsync, canArchive);


            var canAbandon = this.WhenAnyValue(x => x._incident, i => i.AllowAbandon);
            AbandonCommand= ReactiveCommand.CreateFromTask(onAbandonAsync, canAbandon);

        }

   
        public ReactiveCommand<Unit, Unit> ValidateCommand { get; }
        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> AbandonCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> ArchiveCommand { get; }



        private async Task onEditAsync()
        {
            _incident.Edit();
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
