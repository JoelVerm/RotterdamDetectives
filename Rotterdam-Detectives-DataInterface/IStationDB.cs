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
        public void AddConnection(string from, string to, string modeOfTransport);
        public void RemoveConnections(string from, string to);
        public void SetCoordinatesOf(string station, int latitude, int longitude);
        public List<IStationWithPlayers> GetWithPlayers();
        public List<string> GetModesOfTransport();
    }
}
