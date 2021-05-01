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
        private static NetworkTaskStateMachine<Incident> _sut;

        static public States CurrentState { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("State Machine Example!\n\n");
           
            _sut = new NetworkTaskStateMachine<Incident>();
           
       

            _sut.Create();
            _sut.Validate();
            _sut.Execute();
            _sut.Archive();


            Console.WriteLine("Press Any Key....");
            Console.ReadLine();


        }




    }
}
