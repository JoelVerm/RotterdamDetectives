using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class StationWithConnections(string name, double latitude, double longitude, int mapX, int mapY, List<IConnection> connections) : IStationWithConnections
    {
        public string Name { get; } = name;

        public double Latitude { get; } = latitude;

        public double Longitude { get; } = longitude;

        public int MapX { get; } = mapX;

        public int MapY { get; } = mapY;

        public List<IConnection> Connections { get; } = connections;

        public StationWithConnections(string name, (double latitude, double longitude) coords, (int mapX, int mapY) map, List<IConnection> connections) : this(name, coords.latitude, coords.longitude, map.mapX, map.mapY, connections) { }
    }
}
