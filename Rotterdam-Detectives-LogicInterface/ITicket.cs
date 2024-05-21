using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface ITicket
    {
        public Result Use(string player, string modeOfTransport);
        public void ResetTickets(string player);
        public IReadOnlyDictionary<string, int> GetSpare(string player);
        public IEnumerable<string> GetHistory(string player);
    }
}
