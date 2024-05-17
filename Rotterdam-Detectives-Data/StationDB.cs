using RotterdamDetectives_DataInterface;
using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    public class StationDB(string connectionString): IStationDB
    {
        readonly DB db = new(connectionString);

        public void Add(string station)
        {
            db.Execute("INSERT INTO Stations (Name) VALUES (@Name)", new { Name = station });
        }

        public void Delete(string station)
        {
            db.Execute("DELETE FROM Stations WHERE Name = @Name", new { Name = station });
        }

        public bool Exists(string station)
        {
            return db.Field<int>("SELECT COUNT(*) FROM Stations WHERE Name = @Name", new { Name = station }) > 0;
        }

        public IEnumerable<string> GetStations()
        {
            return db.Rows("SELECT Name FROM Stations", new {}, row => row["Name"].ToString()!) ?? [];
        }

        public IEnumerable<IConnection>? GetConnectionsFrom(string station)
        {
            return db.Rows("SELECT *, Stations.Name AS 'ToName', TransportTypes.Name AS 'TransportType' FROM Connections " +
                "LEFT JOIN Stations ON Connections.[To] = Stations.Id " +
                "LEFT JOIN TransportTypes ON Connections.TransportTypeId = TransportTypes.Id " +
                "WHERE Connections.[From] = (SELECT Id FROM Stations WHERE Name = @From)",
                new { From = station },
                row => new Connection(row["ToName"].ToString()!, row["TransportType"].ToString()!)
            );
        }

        public void AddConnection(string from, string to, string modeOfTransport)
        {
            int fromId = db.Field<int>("SELECT Id FROM Stations WHERE Name = @Name", new { Name = from }) ?? 0;
            int toId = db.Field<int>(db.LastQuery, new { Name = to }) ?? 0;
            int modeOfTransportId = db.Field<int>("SELECT Id FROM ModeOfTransports WHERE Name = @Name", new { Name = modeOfTransport }) ?? 0;
            if (fromId == 0 || toId == 0 || modeOfTransportId == 0)
                return;
            db.Execute("INSERT INTO Connections ([From], [To], ModeOfTransport) VALUES (@From, @To, @ModeOfTransport)",
                               new { From = fromId, To = toId, ModeOfTransport = modeOfTransport });
            db.Execute(db.LastQuery, new { From = toId, To = fromId, ModeOfTransport = modeOfTransport });
        }

        public void RemoveConnections(string station1, string station2)
        {
            int station1Id = db.Field<int>("SELECT Id FROM Stations WHERE Name = @Name", new { Name = station1 }) ?? 0;
            int station2Id = db.Field<int>(db.LastQuery, new { Name = station2 }) ?? 0;
            if (station1Id == 0 || station2Id == 0)
                return;
            db.Execute("DELETE FROM Connections WHERE [From] = @From AND [To] = @To",
                                              new { From = station1Id, To = station2Id });
            db.Execute(db.LastQuery, new { From = station2Id, To = station1Id });
        }

        public void SetCoordinatesOf(string station, int latitude, int longitude)
        {
            db.Execute("UPDATE Stations SET Latitude = @Latitude, Longitude = @Longitude " +
                "WHERE Name = @Station",
                new { Latitude = latitude, Longitude = longitude, Station = station });
        }

        public List<IStationWithPlayers> GetWithPlayers()
        {
            var rows = db.Rows("SELECT Stations.Name AS 'Station', Players.Name AS 'Player' FROM Stations " +
                "LEFT JOIN Players ON Stations.Id = Players.StationId",
                new {},
                row => (row["Station"].ToString()!, row["Player"].ToString()!)
            ) ?? [];
            var stations = new List<StationWithPlayers>();
            foreach (var row in rows)
            {
                var station = stations.FirstOrDefault(s => s.Station == row.Item1);
                if (station == null)
                {
                    station = new StationWithPlayers(row.Item1, []);
                    stations.Add(station);
                }
                if (!string.IsNullOrEmpty(row.Item2))
                    station.players.Add(row.Item2);
            }
            return [.. stations];
        }

        public List<string> GetModesOfTransport()
        {
            return db.Rows("SELECT Name FROM TransportTypes", new {}, row => row["Name"].ToString()!)?.ToList() ?? [];
        }
    }
}
