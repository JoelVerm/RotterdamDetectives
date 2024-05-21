using RotterdamDetectives_DataInterface;
using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class Ticket(ITicketDB db) : ITicket
    {
        readonly Dictionary<string, int> maxTickets = db.GetMaxTickets();

        public Result Use(string player, string modeOfTransport)
        {
            var ticket = maxTickets.GetValueOrDefault(modeOfTransport, -100);
            if (ticket < 0)
                return Result.Err("No ticket for this mode of transport");
            ticket -= db.GetHistoryCount(player, modeOfTransport);
            if (ticket <= 0)
                return Result.Err("No more tickets for this mode of transport");
            db.AddHistory(player, modeOfTransport);
            return Result.Ok();
        }

        public void ResetTickets(string player)
        {
            db.ClearHistory(player);
        }

        public IReadOnlyDictionary<string, int> GetSpare(string player)
        {
            return maxTickets.ToDictionary(x => x.Key, x => x.Value - db.GetHistoryCount(player, x.Key));
        }

        public IEnumerable<string> GetHistory(string player)
        {
            return db.GetTicketHistory(player);
        }
    }
}
