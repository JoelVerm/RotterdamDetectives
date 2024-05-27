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
            db.Execute("DELETE FROM Connections " +
                "WHERE [From] = (SELECT Id FROM Stations WHERE Name = @Name) " +
                "OR [To] = (SELECT Id FROM Stations WHERE Name = @Name)",
                new { Name = station });
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
            return db.Rows("SELECT Connections.Name, Stations.Name AS 'ToName', TransportTypes.Name AS 'TransportType' FROM Connections " +
                "LEFT JOIN Stations ON Connections.[To] = Stations.Id " +
                "LEFT JOIN TransportTypes ON Connections.TransportTypeId = TransportTypes.Id " +
                "WHERE Connections.[From] = (SELECT Id FROM Stations WHERE Name = @From)",
                new { From = station },
                row => new Connection(row["Name"].ToString()!, row["ToName"].ToString()!, row["TransportType"].ToString()!)
            );
        }

        public void AddConnection(string from, string to, string name, string transportType)
        {
            int fromId = db.Field<int>("SELECT Id FROM Stations WHERE Name = @Name", new { Name = from }) ?? 0;
            int toId = db.Field<int>(db.LastQuery, new { Name = to }) ?? 0;
            int transportTypeId = db.Field<int>("SELECT Id FROM TransportTypes WHERE Name = @Name", new { Name = transportType }) ?? 0;
            if (fromId == 0 || toId == 0 || transportTypeId == 0)
                return;
            db.Execute("INSERT INTO Connections (Name, [From], [To], TransportTypeId) VALUES (@name, @From, @To, @transportTypeId)",
                               new { name, From = fromId, To = toId, transportTypeId });
            db.Execute(db.LastQuery, new { name, From = toId, To = fromId, transportTypeId });
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

        public void SetCoordinatesOf(string station, double latitude, double longitude)
        {
            db.Execute("UPDATE Stations SET Latitude = @Latitude, Longitude = @Longitude " +
                "WHERE Name = @Station",
                new { Latitude = latitude, Longitude = longitude, Station = station });
        }

        public (double latitude, double longitude) GetCoordinatesOf(string station)
        {
            return db.Row("SELECT Latitude, Longitude FROM Stations WHERE Name = @Station",
                new { Station = station },
                row => ((double)row["Latitude"], (double)row["Longitude"])
            );
        }

        public void SetMapPositionOf(string station, int x, int y)
        {
            db.Execute("UPDATE Stations SET MapX = @X, MapY = @Y " +
                "WHERE Name = @Station",
                new { X = x, Y = y, Station = station });
        }

        public (int x, int y) GetMapPositionOf(string station)
        {
            return db.Row("SELECT MapX, MapY FROM Stations WHERE Name = @Station",
                new { Station = station },
                row => ((int)row["MapX"], (int)row["MapY"])
            );
        }

        public List<IStationWithPlayers> GetWithPlayers(string username)
        {
            var gameId = db.Field<int>("SELECT Game FROM Players WHERE Name = @username", new { username }) ?? 0;
            var rows = db.Rows("SELECT Stations.Name AS 'Station', Players.Name AS 'Player' FROM Stations " +
                "LEFT JOIN Players ON Stations.Id = Players.StationId " +
                "AND Players.Game = @gameId " +
                "AND (SELECT MrX FROM Games WHERE Id = @gameId) <> Players.Id",
                new { username, gameId },
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
