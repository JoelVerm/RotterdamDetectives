using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Connection(Station _destination, ModeOfTransport _modeOfTransport) : IConnection
    {
        public ModeOfTransport ModeOfTransport { get; private set; } = _modeOfTransport;
        private readonly Station destination = _destination;
        public IStation Destination => destination;
    }
}
