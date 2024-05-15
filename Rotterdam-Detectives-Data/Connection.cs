using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    internal class Connection(string destination, string modeOfTransport) : IConnection
    {
        public string ModeOfTransport { get; } = modeOfTransport;
        public string Destination { get; } = destination;
    }
}
