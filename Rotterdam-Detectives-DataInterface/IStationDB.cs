using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface IStationDB
    {
        public void Add(string station);
        public void Delete(string station);
        public bool Exists(string station);
        public IEnumerable<string> GetStations();
        public IEnumerable<IConnection>? GetConnectionsFrom(string station);
        public void AddConnection(string from, string to, string name, string transportType);
        public void RemoveConnections(string from, string to);
        public void SetCoordinatesOf(string station, double latitude, double longitude);
        public (double latitude, double longitude) GetCoordinatesOf(string station);
        public void SetMapPositionOf(string station, int x, int y);
        public (int x, int y) GetMapPositionOf(string station);
        public List<IStationWithPlayers> GetWithPlayers(string username);
        public List<string> GetModesOfTransport();
    }
}
