using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStation
    {
        public IReadOnlyList<IConnection> GetConnectionsOf(string station);
        public Result AddConnection(string from, string to, string modeOfTransport);
        public void RemoveConnections(string from, string to);
        public void SetCoordinates(string station, int latitude, int longitude);
        public List<IStationWithPlayers> GetWithPlayers();
    }
}
