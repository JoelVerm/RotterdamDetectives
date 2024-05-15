using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface ITicketDB
    {
        public void AddHistory(string player, string transportType);
        public IEnumerable<string> GetTicketHistory(string player);
        public int GetHistoryCount(string player, string transportType);
        public void ClearHistory(string player);
        public Dictionary<string, int> GetMaxTickets();
    }
}
