using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Connection(string _destination, string _modeOfTransport) : IConnection
    {
        public string Destination { get; set; } = _destination;
        public string ModeOfTransport { get; set; } = _modeOfTransport;
    }
}
