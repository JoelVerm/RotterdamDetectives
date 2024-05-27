using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockStationDB : IStationDB
    {
        public bool MockExists { get; set; } = false;

        public bool Exists(string station) => MockExists;

        public List<IConnection> MockConnectionsFrom { get; set; } = [];

        public IEnumerable<IConnection>? GetConnectionsFrom(string station) => MockConnectionsFrom;

        public List<string> MockModesOfTransport { get; set; } = [];

        public List<string> GetModesOfTransport() => MockModesOfTransport;

        public List<string> MockStations { get; set; } = [];

        public IEnumerable<string> GetStations() => MockStations;

        public List<IStationWithPlayers> MockWithPlayers { get; set; } = [];

        public List<IStationWithPlayers> GetWithPlayers(string username) => MockWithPlayers;

        public (double latitude, double longitude) MockCoordinates { get; set; } = (0, 0);

        public (double latitude, double longitude) GetCoordinatesOf(string station) => MockCoordinates;

        public (int x, int y) MockMapPosition { get; set; } = (0, 0);

        public (int x, int y) GetMapPositionOf(string station) => MockMapPosition;

        public void Add(string station) { }

        public void AddConnection(string from, string to, string name, string transportType) { }

        public void Delete(string station) { }

        public void RemoveConnections(string from, string to) { }

        public void SetCoordinatesOf(string station, double latitude, double longitude) {}

        public void SetMapPositionOf(string station, int x, int y) {}
    }

    internal class MockConnection : IConnection
    {
        public string Name { get; set; } = "";

        public string ModeOfTransport { get; set; } = "";

        public string Destination { get; set; } = "";
    }
}
