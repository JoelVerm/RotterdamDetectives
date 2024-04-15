using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Player: IPlayer
    {
        public string Name { get; private set; }
        internal string Password { get; private set; }
        public bool IsMrX { get; private set; }
        internal Player? gameMaster;
        public IPlayer? GameMaster => gameMaster;
        internal List<Ticket> tickets = new();
        public IReadOnlyList<ITicket> Tickets => tickets;
        internal List<Ticket> ticketHistory = new();
        public IReadOnlyList<ITicket> TicketHistory => ticketHistory;
        internal Station currentStation;
        public IStation CurrentStation => currentStation;

        public Player(string name, string password, Station station)
        {
            Name = name;
            Password = password;
            currentStation = station;
        }

        internal Result MoveToStation(Station station, ModeOfTransport modeOfTransport)
        {
            var connections = CurrentStation.GetConnectionsTo(station);
            if (connections.Count == 0)
                return Result.Err("Stations are not connected");

            var transportConnection = connections.FirstOrDefault(c => c.ModeOfTransport == modeOfTransport);
            if (transportConnection == null)
                return Result.Err("Wrong mode of transport");

            var ticket = tickets.FirstOrDefault(t => t.ModeOfTransport == modeOfTransport);
            if (ticket == null)
                return Result.Err("No ticket for this mode of transport");

            tickets.Remove(ticket);
            ticketHistory.Add(ticket);
            currentStation = station;

            return Result.Ok();
        }

        public void ResetTickets()
        {
            tickets.AddRange(ticketHistory);
            ticketHistory.Clear();
        }

        public void ResetGameMaster() => gameMaster = null;
    }
}
