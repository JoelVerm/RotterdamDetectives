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
        public IReadOnlyList<RotterdamDetectives_LogicInterface.IConnection> GetConnectionsOf(string station)
        {
            return db.GetConnectionsFrom(station)?.Select(c => new Connection(c)).ToList() ?? [];
        }

        public Result AddConnection(string from, string to, string name, string modeOfTransport)
        {
            if (db.GetConnectionsFrom(from)?.Any(c => c.Destination == to) ?? false)
                return Result.Err("Connection already exists");
            db.AddConnection(from, to, name, modeOfTransport);
            return Result.Ok();
        }

        public void RemoveConnections(string from, string to)
        {
            db.RemoveConnections(from, to);
        }

        public void SetCoordinates(string station, int latitude, int longitude)
        {
            db.SetCoordinatesOf(station, latitude, longitude);
        }

        public List<RotterdamDetectives_LogicInterface.IStationWithPlayers> GetWithPlayers(string username)
        {
            return db.GetWithPlayers(username)
                .Select(s => new StationWithPlayers(s.Station, s.Players,
                    db.GetConnectionsFrom(s.Station)?.Select(c => new Connection(c)).ToList() ?? []
                ))
                .ToList<RotterdamDetectives_LogicInterface.IStationWithPlayers>();
        }

        internal string RandomStation()
        {
            return db.GetStations().OrderBy(s => Guid.NewGuid()).First();
        }
    }
}
