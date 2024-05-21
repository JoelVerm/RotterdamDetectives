using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class StationWithConnections(string name, List<IConnection> connections) : IStationWithConnections
    {
        public string Name { get; } = name;

        public List<IConnection> Connections { get; } = connections;
    }
}
