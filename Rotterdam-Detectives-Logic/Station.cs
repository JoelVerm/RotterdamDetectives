using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class Station(IStationDB db) : IStation
    {
        public List<RotterdamDetectives_LogicInterface.IConnection> GetConnectionsOf(string station)
        {
            return db.GetConnectionsFrom(station)?.Select(c => new Connection(c)).ToList<RotterdamDetectives_LogicInterface.IConnection>() ?? [];
        }

        public List<RotterdamDetectives_LogicInterface.IStationWithPlayers> GetWithPlayers(string username)
        {
            return db.GetWithPlayers(username)
                .Select(s => new StationWithPlayers(
                    new StationWithConnections(
                        s.Station,
                        db.GetCoordinatesOf(s.Station),
                        db.GetMapPositionOf(s.Station),
                        GetConnectionsOf(s.Station)
                    ),
                    s.Players
                ))
                .ToList<RotterdamDetectives_LogicInterface.IStationWithPlayers>();
        }

        internal string? RandomStation()
        {
            return db.GetStations().OrderBy(s => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
