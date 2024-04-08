namespace RotterdamDetectives_DataInterface
{
    public interface IDataSource
    {
        public bool AddPlayer(string name, string passwordHash);
        public IPlayerData? GetPlayerData(string name);
        public bool UpdatePassword(string name, string passwordHash);
        public bool UpdateGameMode(string name, int gameMode);
        public bool MovePlayerToStation(string playerName, string stationName);
        public bool DeletePlayer(string name);

        public bool AddStation(string name, double latitude, double longitude);
        public bool MapStation(string name, int mapX, int mapY);
        public IStationData? GetStationData(string name);
        public IEnumerable<IStationData>? GetStations();
        public bool ConnectStations(string from, string to, int transportType);
        public IEnumerable<IConnectedStation>? GetConnectedStations(string stationName);
        public bool DisconnectStations(string from, string to);
        public bool DeleteStation(string name);

        public bool AddPlayerToGame(string playerName, string gameMasterName);
        public IEnumerable<IPlayerData>? GetPlayersInGame(string gameMasterName);
        public bool RemovePlayerFromGame(string playerName);
        public bool EndGame(string gameMasterName);

        public bool AddTransportType(string name, int maxTickets);
        public bool DeleteTransportType(string name);

        public bool AddTicket(string playerName, string transportType);
        public int? GetTicketCount(string playerName, string transportType);
        public bool DeleteTicket(string playerName, string transportType);
    }
}
