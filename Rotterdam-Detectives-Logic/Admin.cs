using RotterdamDetectives_DataInterface;
using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class Admin(IGameDB gameDB, IPlayerDB playerDB, IStationDB stationDB, ITicketDB ticketDB) : IAdmin
    {
        public void AddStation(string station)
        {
            stationDB.Add(station);
        }

        public Result ConnectStations(string station1, string station2, string name, string modeOfTransport)
        {
            if (!stationDB.Exists(station1) || !stationDB.Exists(station2))
                return Result.Err("One or both of the stations do not exist.");
            stationDB.AddConnection(station1, station2, name, modeOfTransport);
            return Result.Ok();
        }

        public Result DisconnectStations(string station1, string station2)
        {
            if (!stationDB.Exists(station1) || !stationDB.Exists(station2))
                return Result.Err("One or both of the stations do not exist.");
            stationDB.RemoveConnections(station1, station2);
            return Result.Ok();
        }

        public Result DeleteStation(string station)
        {
            if (!stationDB.Exists(station))
                return Result.Err("Station does not exist.");
            stationDB.Delete(station);
            return Result.Ok();
        }

        public void SetCoordinates(string station, double latitude, double longitude)
        {
            stationDB.SetCoordinatesOf(station, latitude, longitude);
        }

        public void SetMapPosition(string station, int x, int y)
        {
            stationDB.SetMapPositionOf(station, x, y);
        }

        public List<IStationWithConnections> GetStations()
        {
            return stationDB.GetStations()
                .Select(s => new StationWithConnections(s,
                    stationDB.GetCoordinatesOf(s),
                    stationDB.GetMapPositionOf(s),
                    stationDB.GetConnectionsFrom(s)
                    ?.Select(c => new Connection(c))
                    .ToList<RotterdamDetectives_LogicInterface.IConnection>() ?? [])).ToList<IStationWithConnections>();
        }

        public List<string> GetModesOfTransport()
        {
            return stationDB.GetModesOfTransport();
        }
    }
}
