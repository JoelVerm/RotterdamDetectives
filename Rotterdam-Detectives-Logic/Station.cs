using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Station: IStation
    {
        public string Name { get; private set; }
        public int Latitude { get; private set; }
        public int Longitude { get; private set; }
        public List<Connection> connections = new();
        public IReadOnlyList<IConnection> Connections => connections;

        public Station(string name)
        {
            Name = name;
        }

        public IReadOnlyList<IConnection> GetConnectionsTo(IStation station)
        {
            return connections.Where(c => c.destination == station).ToList();
        }

        public void SetCoordinates(int latitude, int longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
