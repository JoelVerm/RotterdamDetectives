using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IPlayer
    {
        public string Name { get; }
        public bool IsMrX { get; }
        public IPlayer? GameMaster { get; }
        public IReadOnlyList<ITicket> Tickets { get; }
        public IReadOnlyList<ITicket> TicketHistory { get; }
        public IStation CurrentStation { get; }

        public void ResetTickets();
        public void ResetGameMaster();
    }
}
