using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IProcessedDataSource
    {
        public bool UserExists(string username);
        public void RegisterUser(string username, string password);
        public bool LoginUser(string username, string password);
        public bool IsAdmin(string username);

        public string GetStationByPlayer(string username);
        public bool MovePlayerToStation(string username, string station, string tansportType);
        public List<IStation> GetStationsAndPlayers(string username);
        public List<IStationConnection> GetConnectedStations(string station);

        public string? GetGameMasterByPlayer(string username);
        public List<string> GetPlayersInGame(string username);
        public void AddPlayerToGame(string gameMaster, string playerName);
        public void StartGame(string gameMaster);
        public void LeaveGame(string username);
        public void EndGame(string gameMaster);

        public List<ITicket> GetTicketsByPlayer(string username);

        public string? GetLastError();
    }
}
