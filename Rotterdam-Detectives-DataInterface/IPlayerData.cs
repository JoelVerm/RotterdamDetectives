namespace RotterdamDetectives_DataInterface
{
    public interface IPlayerData
    {
        public string Name { get; }
        public string PasswordHash { get; }
        public int GameMode { get; }
        public bool IsAdmin { get; }
        public IStationData? Station { get; }
        public IGameData? Game { get; }
    }
}
