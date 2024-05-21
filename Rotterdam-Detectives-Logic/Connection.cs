using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Connection(string name, string destination, string modeOfTransport) : IConnection
    {
        public string Name { get; set; } = name;
        public string Destination { get; set; } = destination;
        public string ModeOfTransport { get; set; } = modeOfTransport;

        public Connection(RotterdamDetectives_DataInterface.IConnection connection): this(connection.Name, connection.Destination, connection.ModeOfTransport) { }
    }
}
