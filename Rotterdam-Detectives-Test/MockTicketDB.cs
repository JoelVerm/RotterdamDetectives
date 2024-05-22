using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockTicketDB : ITicketDB
    {
        public void AddHistory(string player, string transportType) {}

        public void ClearHistory(string player) {}

        public int MockGetHistoryCount { get; set; } = 0;

        public int GetHistoryCount(string player, string transportType) => MockGetHistoryCount;

        public Dictionary<string, int> MockGetMaxTickets { get; set; } = [];

        public Dictionary<string, int> GetMaxTickets() => MockGetMaxTickets;

        public List<string> MockGetTicketHistory { get; set; } = [];

        public IEnumerable<string> GetTicketHistory(string player) => MockGetTicketHistory;
    }
}
