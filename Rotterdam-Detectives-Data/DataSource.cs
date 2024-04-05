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
            var playerData = DB.Rows("SELECT * FROM Players WHERE Name = @Name", new Data.Player { Name = name })?.FirstOrDefault();
            if (playerData == null)
                return null;
            return ConvertPlayer(playerData);
        }

        Interface.Player ConvertPlayer(Data.Player playerData)
        {
            var player = new Interface.Player { Name = playerData.Name, PasswordHash = playerData.PasswordHash, GameMode = playerData.GameMode };
            if (playerData.StationId != null)
                player.Station = DB.Rows("SELECT * FROM Stations WHERE Id = @Id", new Data.Station { Id = playerData.StationId.Value })?.FirstOrDefault();
            if (playerData.GameMasterId != null)
            {
                var gameMaster = DB.Rows("SELECT * FROM Players WHERE Id = @Id", new Data.Player { Id = playerData.GameMasterId.Value })?.FirstOrDefault();
                if (gameMaster != null)
                    player.GameMaster = ConvertPlayer(gameMaster);
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
            int playerId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = name }) ?? 0;
            return DB.Execute("DELETE FROM Players WHERE Name = @Name", new Data.Player { Name = name })
                && DB.Execute("DELETE FROM Tickets WHERE PlayerId = @PlayerId", new Data.Ticket { PlayerId = playerId });
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
            return DB.Rows("SELECT * FROM Stations WHERE Name = @Name", new Data.Station { Name = name })?.FirstOrDefault();
        }

        public IEnumerable<IStationData>? GetStations()
        {
            return DB.Rows("SELECT * FROM Stations", new Data.Station());
        }

        public IEnumerable<IStationData>? GetAllStationDatas(string name)
        {
            return DB.Rows("SELECT * FROM Stations WHERE Name = @Name", new Data.Station { Name = name });
        }

        public bool ConnectStations(string from, string to, int transportType)
        {
            int fromId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = from }) ?? 0;
            if (fromId == 0)
                return false;
            int toId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = to }) ?? 0;
            if (toId == 0)
                return false;
            return DB.Execute("INSERT INTO Connections ([From], [To], TransportType) VALUES (@From, @To, @TransportType)", new Data.Connection { From = fromId, To = toId, TransportTypeId = transportType })
                && DB.Execute(DB.LastQuery, new Data.Connection { From = toId, To = fromId, TransportTypeId = transportType });
        }

        public IEnumerable<IConnectedStation>? GetConnectedStations(string stationName)
        {
            int stationId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = stationName }) ?? 0;
            if (stationId == 0)
                return null;
            var connections = DB.Rows("SELECT * FROM Connections WHERE [From] = @From", new Data.Connection { From = stationId });
            if (connections == null)
                return null;
            List<IConnectedStation> connectedStations = new();
            foreach (var connection in connections)
            {
                var station = DB.Rows("SELECT * FROM Stations WHERE Id = @Id", new Data.Station { Id = connection.To })?.FirstOrDefault();
                if (station == null)
                    continue;
                connectedStations.Add(new Interface.ConnectedStation { Station = station, TransportType = connection.TransportTypeId });
            }
            return connectedStations;
        }

        public bool DisconnectStations(string from, string to)
        {
            int fromId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = from }) ?? 0;
            if (fromId == 0)
                return false;
            int toId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = to }) ?? 0;
            if (toId == 0)
                return false;
            return DB.Execute("DELETE FROM Connections WHERE [From] = @From AND [To] = @To", new Data.Connection { From = fromId, To = toId })
                && DB.Execute(DB.LastQuery, new Data.Connection { From = toId, To = fromId });
        }

        public bool DeleteStation(string name)
        {
            int stationId = DB.Field<Data.Station, int>("SELECT Id FROM Stations WHERE Name = @Name", new Data.Station { Name = name }) ?? 0;
            return DB.Execute("DELETE FROM Stations WHERE Name = @Name", new Data.Station { Name = name })
                && DB.Execute("DELETE FROM Connections WHERE [From] = @From OR [To] = @To", new Data.Connection { From = stationId, To = stationId });
        }

        public bool AddPlayerToGame(string playerName, string gameMasterName)
        {
            int gameMasterId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = gameMasterName }) ?? 0;
            if (gameMasterId == 0)
                return false;
            return DB.Execute("UPDATE Players SET GameMasterId = @GameMasterId WHERE Name = @Name", new Data.Player { Name = playerName, GameMasterId = gameMasterId });
        }

        public IEnumerable<IPlayerData>? GetPlayersInGame(string gameMasterName)
        {
            int gameMasterId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = gameMasterName }) ?? 0;
            if (gameMasterId == 0)
                return null;
            var players = DB.Rows("SELECT * FROM Players WHERE GameMasterId = @GameMasterId", new Data.Player { GameMasterId = gameMasterId });
            return players?.Select(ConvertPlayer);
        }

        public bool EndGame(string gameMasterName)
        {
            int gameMasterId = DB.Field<Data.Player, int>("SELECT Id FROM Players WHERE Name = @Name", new Data.Player { Name = gameMasterName }) ?? 0;
            if (gameMasterId == 0)
                return false;
            return DB.Execute("UPDATE Players SET GameMasterId = NULL WHERE GameMasterId = @GameMasterId", new Data.Player { GameMasterId = gameMasterId });
        }

        public bool AddTransportType(string name, int maxTickets)
        {
            return DB.Execute("INSERT INTO TransportTypes (Name) VALUES (@Name)", new Data.TransportType { Name = name, MaxTickets = maxTickets });
        }

        public bool DeleteTransportType(string name)
        {
            int typeId = DB.Field<Data.TransportType, int>("SELECT Id FROM TransportTypes WHERE Name = @Name", new Data.TransportType { Name = name }) ?? 0;
            bool inUseTickets = DB.Field<Data.Ticket, bool>("SELECT COUNT(*) > 0 FROM Tickets WHERE TransportTypeId = @TransportTypeId", new Data.Ticket { TransportTypeId = typeId }) ?? false;
            if (inUseTickets)
                return false;
            bool inUseConnections = DB.Field<Data.Connection, bool>("SELECT COUNT(*) > 0 FROM Connections WHERE TransportType = @TransportType", new Data.Connection { TransportTypeId = typeId }) ?? false;
            if (inUseConnections)
                return false;
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
