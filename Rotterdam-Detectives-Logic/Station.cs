using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Station(string name) : IStation
    {
        public string Name { get; private set; } = name;
        public int Latitude { get; private set; }
        public int Longitude { get; private set; }
        private readonly List<Connection> connections = [];
        public IReadOnlyList<IConnection> Connections => connections;

        public IReadOnlyList<IConnection> GetConnectionsTo(IStation station)
        {
            return connections.Where(c => c.Destination == station).ToList();
        }

        internal Result AddConnection(Station station, ModeOfTransport modeOfTransport)
        {
            if (connections.Any(c => c.Destination == station))
                return Result.Err("Connection already exists");
            connections.Add(new Connection(station, modeOfTransport));
            return Result.Ok();
        }

        internal void RemoveConnections(Station station)
        {
            connections.RemoveAll(c => c.Destination == station);
        }

        public void SetCoordinates(int latitude, int longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
