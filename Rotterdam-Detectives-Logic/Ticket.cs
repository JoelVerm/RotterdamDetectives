using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Ticket(ModeOfTransport modeOfTransport) : ITicket
    {
        public ModeOfTransport ModeOfTransport { get; private set; } = modeOfTransport;
    }
}
