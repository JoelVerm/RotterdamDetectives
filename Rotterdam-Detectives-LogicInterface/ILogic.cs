using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface ILogic
    {
        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<IStation> Stations { get; }

        public IAdminController AdminController { get; }

        public IPlayer? GetPlayer(string name);
        public Result RegisterPlayer(string name, string password);
        public Result LoginPlayer(string name, string password);
        public Result MovePlayerToStation(string name, string station, ModeOfTransport modeOfTransport);
        public IReadOnlyList<IStationWithPlayers> GetStationsWithPlayers();
        public IReadOnlyList<IPlayer> GetPlayersInGameWith(string gameMaster);
        public Result SetGameMaster(string name, string gameMasterName);
        public Result StartGame(string gameMasterName);
        public Result EndGame(string gameMasterName);
    }
}
