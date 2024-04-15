using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockLogic : ILogic
    {
        public IReadOnlyList<IPlayer> Players => throw new NotImplementedException();

        public IReadOnlyList<IStation> Stations => throw new NotImplementedException();

        public IPlayer? GetPlayer(string name) => throw new NotImplementedException();

        public Result RegisterPlayer(string name, string password) => throw new NotImplementedException();

        public Result LoginPlayer(string name, string password) => throw new NotImplementedException();

        public Result MovePlayerToStation(string name, string station, ModeOfTransport modeOfTransport) => throw new NotImplementedException();

        public IReadOnlyList<IStationWithPlayers> GetStationsWithPlayers() => throw new NotImplementedException();

        public IReadOnlyList<IPlayer> GetPlayersInGameWith(string gameMaster) => throw new NotImplementedException();

        public Result SetGameMaster(string name, string gameMasterName) => throw new NotImplementedException();

        public Result StartGame(string gameMasterName) => throw new NotImplementedException();

        public Result EndGame(string gameMasterName) => throw new NotImplementedException();

    }
}
