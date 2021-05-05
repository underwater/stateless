using ReactiveUI;
using System.Diagnostics;

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
        }
    }
}
