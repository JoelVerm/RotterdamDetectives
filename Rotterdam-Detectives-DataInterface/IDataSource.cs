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
        public bool ConnectStations(int from, int to, int transportType);
        public IEnumerable<IConnectedStation>? GetConnectedStations(string stationName);
        public bool DisconnectStations(int from, int to);
        public bool DeleteStation(string name);
        public bool CreateGame(string organiserName);
        public bool AddPlayerToGame(string playerName, string organiserName);
        public IEnumerable<IPlayerData>? GetPlayersInGame(string organiserName);
        public bool DeleteGame(string organiserName);
        public bool AddTransportType(string name, int maxTickets);
        public bool DeleteTransportType(string name);
        public bool AddTicket(string playerName, string transportType);
        public int? GetTicketCount(string playerName, string transportType);
        public bool DeleteTicket(string playerName, string transportType);
    }
}
