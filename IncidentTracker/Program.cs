using Stateless;
using Stateless.Graph;
using System;
using System.Linq;
using System.Transactions;

namespace IncidentTracker
{
    class Program
    {

        static StateMachine<States, Triggers> _machine;
        private static IncidentStateMachine _incident;

        static public States CurrentState { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("State Machine Example!\n\n");
           
            _incident = new IncidentStateMachine();
           
       

            _incident.Create();
            _incident.Validate();
            _incident.Execute();
            _incident.Archive();


            Console.WriteLine("Press Any Key....");
            Console.ReadLine();


        }




    }
}
