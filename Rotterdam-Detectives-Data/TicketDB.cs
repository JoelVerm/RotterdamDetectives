using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    public class TicketDB(string connectionString) : ITicketDB
    {
        readonly DB db = new(connectionString);

        public void AddHistory(string player, string transportType)
        {
            db.Execute("INSERT INTO Tickets (PlayerId, TransportTypeId) VALUES (" +
                "(SELECT Id FROM Players WHERE Name = @player)," +
                "(SELECT Id FROM TransportTypes WHERE Name = @transportType))",
                new { player, transportType });
        }

        public IEnumerable<string> GetTicketHistory(string player)
        {
            return db.Rows("SELECT TransportTypes.Name FROM Tickets LEFT JOIN TransportTypes ON Tickets.TransportTypeId = TransportTypes.Id WHERE Tickets.PlayerId = (SELECT Id FROM Players WHERE Name = @player)", new { player }, row => row["Name"].ToString()) ?? [];
        }

        public int GetHistoryCount(string player, string transportType)
        {
            return db.Field<int>("SELECT COUNT(*) FROM Tickets WHERE PlayerId = (SELECT Id FROM Players WHERE Name = @player) AND TransportTypeId = (SELECT Id FROM TransportTypes WHERE Name = @transportType)", new { player, transportType }) ?? 0;
        }

        public void ClearHistory(string player)
        {
            db.Execute("DELETE FROM Tickets WHERE PlayerId = (SELECT Id FROM Players WHERE Name = @player)", new { player });
        }

        public Dictionary<string, int> GetMaxTickets()
        {
            return db.Rows("SELECT TransportTypes.Name, MaxTickets FROM TransportTypes", new {}, row => (row["Name"].ToString()!, (int)row["MaxTickets"]))?.ToDictionary() ?? [];
        }
    }
}
