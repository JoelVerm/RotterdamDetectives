using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Connection: IConnection
    {
        public ModeOfTransport ModeOfTransport { get; private set; }
        internal Station destination;
        public IStation Destination => destination;

        public Connection(Station _destination, ModeOfTransport _modeOfTransport)
        {
            destination = _destination;
            ModeOfTransport = _modeOfTransport;
        }
    }
}
