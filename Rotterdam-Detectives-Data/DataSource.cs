using RotterdamDetectives_DataInterface;
using System.Diagnostics;

namespace RotterdamDetectives_Data
{
    public class DataSource : IDataSource
    {
        DB DB;

        public DataSource(string connectionString)
        {
            DB = new(connectionString);
        }

        public bool AddPlayer(string name, string passwordHash)
        {
           return DB.Execute("INSERT INTO Players (Name, PasswordHash) OUTPUT INSERTED.Id VALUES (@Name, @PasswordHash)", new Data.Player { Name = name, PasswordHash = passwordHash });
        }
        
        public IPlayerData? GetPlayerData(string name)
        {
            var playerData = DB.Rows("SELECT * FROM Players WHERE Name = @Name", new Data.Player { Name = name })?.First();
            if (playerData == null)
                return null;
            return ConvertPlayer(playerData);
        }

        Interface.Player ConvertPlayer(Data.Player playerData)
        {
            var player = new Interface.Player { Name = playerData.Name, PasswordHash = playerData.PasswordHash, GameMode = playerData.GameMode };
            if (playerData.StationId != null)
                player.Station = DB.Rows("SELECT * FROM Stations WHERE Id = @Id", new Data.Station { Id = playerData.StationId.Value })?.First();
            if (playerData.GameId != null)
            {
                var game = DB.Rows("SELECT * FROM Games WHERE Id = @Id", new Data.Game { Id = playerData.GameId.Value })?.First();
                if (game != null)
                {
                    var organiser = DB.Rows("SELECT * FROM Players WHERE Id = @Id", new Data.Player { Id = game.OrganiserId })?.First();
                    if (organiser != null)
                        player.Game = new Interface.Game { OrganiserName = organiser.Name };
                }
            }
            return player;
        }
        
        public bool UpdatePassword(string name, string passwordHash)
        {
            return DB.Execute("UPDATE Players SET PasswordHash = @PasswordHash WHERE Name = @Name", new Data.Player { Name = name, PasswordHash = passwordHash });
        }

        public bool UpdateGameMode(string name, int gameMode)
        {
            return DB.Execute("UPDATE Players SET GameMode = @GameMode WHERE Id = @Id", new Data.Player { Name = name, GameMode = gameMode });
        }

        public bool MovePlayerToStation(string playerName, string stationName)
        {
            int stationId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = stationName }) ?? 0;
            if (stationId == 0)
                return false;
            return DB.Execute("UPDATE Players SET StationId = @StationId WHERE Name = @Name", new Data.Player { Name = playerName, StationId = stationId });
        }

        public bool DeletePlayer(string name)
        {
            return DB.Execute("DELETE FROM Players WHERE Name = @Name", new Data.Player { Name = name });
        }

        public bool AddStation(string name, double latitude, double longitude)
        {
            return DB.Execute("INSERT INTO Stations (Name, Description, Latitude, Longitude) VALUES (@Name, @Description, @Latitude, @Longitude)", new Data.Station { Name = name, Latitude = latitude, Longitude = longitude });
        }

        public bool MapStation(string name, int mapX, int mapY)
        {
            return DB.Execute("UPDATE Stations SET MapX = @MapX, MapY = @MapY WHERE Name = @Name", new Data.Station { Name = name, MapX = mapX, MapY = mapY });
        }

        public IStationData? GetStationData(string name)
        {
            return DB.Rows("SELECT * FROM Stations WHERE Name = @Name", new Data.Station { Name = name })?.First();
        }

        public IEnumerable<IStationData>? GetAllStationDatas(string name)
        {
            return DB.Rows("SELECT * FROM Stations WHERE Name = @Name", new Data.Station { Name = name });
        }

        public bool ConnectStations(int from, int to, int transportType)
        {
            return DB.Execute("INSERT INTO Connections (Station1, Station2, TransportType) VALUES (@Station1, @Station2, @TransportType)", new Data.Connection { From = from, To = to, TransportType = transportType })
                && DB.Execute(DB.LastQuery, new Data.Connection { From = to, To = from, TransportType = transportType });
        }

        public IEnumerable<IConnectedStation>? GetConnectedStations(string stationName)
        {
            int stationId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = stationName }) ?? 0;
            if (stationId == 0)
                return null;
            var connections = DB.Rows("SELECT * FROM Connections WHERE From = @From", new Data.Connection { From = stationId });
            if (connections == null)
                return null;
            List<IConnectedStation> connectedStations = new();
            foreach (var connection in connections)
            {
                var station = DB.Rows("SELECT * FROM Stations WHERE Id = @Id", new Data.Station { Id = connection.To })?.First();
                if (station == null)
                    continue;
                connectedStations.Add(new Interface.ConnectedStation { Station = station, TransportType = connection.TransportType });
            }
            return connectedStations;
        }

        public bool DisconnectStations(int from, int to)
        {
            return DB.Execute("DELETE FROM Connections WHERE Station1 = @Station1 AND Station2 = @Station2", new Data.Connection { From = from, To = to })
                && DB.Execute(DB.LastQuery, new Data.Connection { From = to, To = from });
        }

        public bool DeleteStation(string name)
        {
            return DB.Execute("DELETE FROM Stations WHERE Name = @Name", new Data.Station { Name = name });
        }

        public bool CreateGame(string organiserName) {
            int organiserId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = organiserName }) ?? 0;
            if (organiserId == 0)
                return false;
            return DB.Execute("INSERT INTO Games (OrganiserId) VALUES (@OrganiserId)", new Data.Game { OrganiserId = organiserId }) && AddPlayerToGame(organiserName, organiserName);
        }

        public bool AddPlayerToGame(string playerName, string organiserName)
        {
            int organiserId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = organiserName }) ?? 0;
            if (organiserId == 0)
                return false;
            int gameId = DB.Field<Data.Game, int>("SELECT Id FROM Games WHERE OrganiserId = @OrganiserId", new Data.Game { OrganiserId = organiserId }) ?? 0;
            if (gameId == 0)
                return false;
            return DB.Execute("UPDATE Players SET GameId = @GameId WHERE Name = @Name", new Data.Player { Name = playerName, GameId = gameId });
        }

        public IEnumerable<IPlayerData>? GetPlayersInGame(string organiserName)
        {
            int organiserId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = organiserName }) ?? 0;
            if (organiserId == 0)
                return null;
            int gameId = DB.Field<Data.Game, int>("SELECT Id FROM Games WHERE OrganiserId = @OrganiserId", new Data.Game { OrganiserId = organiserId }) ?? 0;
            if (gameId == 0)
                return null;
            var players = DB.Rows("SELECT * FROM Players WHERE GameId = @GameId", new Data.Player { GameId = gameId });
            return players?.Select(ConvertPlayer);
        }

        public bool DeleteGame(string organiserName)
        {
            int organiserId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = organiserName }) ?? 0;
            if (organiserId == 0)
                return false;
            return DB.Execute("DELETE FROM Games WHERE OrganiserId = @OrganiserId", new Data.Game { OrganiserId = organiserId });
        }

        public bool AddTransportType(string name, int maxTickets)
        {
            return DB.Execute("INSERT INTO TransportTypes (Name) VALUES (@Name)", new Data.TransportType { Name = name, MaxTickets = maxTickets });
        }

        public bool DeleteTransportType(string name)
        {
            return DB.Execute("DELETE FROM TransportTypes WHERE Name = @Name", new Data.TransportType { Name = name });
        }

        public bool AddTicket(string playerName, string transportType)
        {
            int playerId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = playerName }) ?? 0;
            if (playerId == 0)
                return false;
            int transportTypeId = DB.Field<Data.TransportType, int>("SELECT Id FROM TransportTypes WHERE Name = @Name", new Data.TransportType { Name = transportType }) ?? 0;
            if (transportTypeId == 0)
                return false;
            return DB.Execute("INSERT INTO Tickets (PlayerId, TransportTypeId) VALUES (@PlayerId, @TransportTypeId)", new Data.Ticket { PlayerId = playerId, TransportTypeId = transportTypeId });
        }

        public int? GetTicketCount(string playerName, string transportType)
        {
            int playerId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = playerName }) ?? 0;
            if (playerId == 0)
                return null;
            int transportTypeId = DB.Field<Data.TransportType, int>("SELECT Id FROM TransportTypes WHERE Name = @Name", new Data.TransportType { Name = transportType }) ?? 0;
            if (transportTypeId == 0)
                return null;
            return DB.Field<Data.Ticket, int>("SELECT COUNT(*) FROM Tickets WHERE PlayerId = @PlayerId AND TransportTypeId = @TransportTypeId", new Data.Ticket { PlayerId = playerId, TransportTypeId = transportTypeId });
        }

        public bool DeleteTicket(string playerName, string transportType)
        {
            int playerId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = playerName }) ?? 0;
            if (playerId == 0)
                return false;
            int transportTypeId = DB.Field<Data.TransportType, int>("SELECT Id FROM TransportTypes WHERE Name = @Name", new Data.TransportType { Name = transportType }) ?? 0;
            if (transportTypeId == 0)
                return false;
            return DB.Execute("DELETE TOP(1) FROM Tickets WHERE PlayerId = @PlayerId AND TransportTypeId = @TransportTypeId", new Data.Ticket { PlayerId = playerId, TransportTypeId = transportTypeId });
        }
    }
}
