using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSimulationEngine
{
    public class Node
    {
        public List<string> Resistances { get; set; }
        public List<string> VoltageSources { get; set; }
        public List<string> CurrentSources { get; set; }
        public List<string> Inductors { get; set; }
        public List<string> Capacitors { get; set; }

        public Node()
        {
            Resistances = new List<string>();
            VoltageSources = new List<string>();
            CurrentSources = new List<string>();
            Inductors = new List<string>();
            Capacitors = new List<string>();
        }


    }
}
