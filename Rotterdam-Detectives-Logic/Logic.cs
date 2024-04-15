using RotterdamDetectives_DataInterface;
using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class Logic: ILogic
    {
        private readonly IDataSource dataSource;
        private readonly IPasswordHasher passwordHasher;

        internal List<Player> players = new();
        public IReadOnlyList<IPlayer> Players => players;
        internal List<Station> stations = new();
        public IReadOnlyList<IStation> Stations => stations;

        internal AdminController adminController;
        public IAdminController AdminController => adminController;

        public Logic(IDataSource _dataSource, IPasswordHasher _passwordHasher)
        {
            dataSource = _dataSource;
            passwordHasher = _passwordHasher;
            adminController = new(this);
        }

        public IPlayer? GetPlayer(string name)
        {
            return players.FirstOrDefault(p => p.Name == name);
        }

        public Result RegisterPlayer(string name, string password)
        {
            if (Players.Any(p => p.Name == name))
                return Result.Err("Account already exists");

            var firstStation = stations.FirstOrDefault();
            if (firstStation == null)
                return Result.Err("No stations to place the player");

            var passwordHash = passwordHasher.HashPassword(name, password);
            var player = new Player(name, passwordHash, firstStation);
            player.AddStartTickets();
            players.Add(player);

            return Result.Ok();
        }

        public Result LoginPlayer(string name, string password)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
                return Result.Err("Account does not exist");

            if (!passwordHasher.VerifyHashedPassword(name, password, player.Password))
                return Result.Err("Wrong password");

            return Result.Ok();
        }

        public Result MovePlayerToStation(string name, string station, ModeOfTransport modeOfTransport)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            var stationToMove = stations.FirstOrDefault(s => s.Name == station);
            if (player == null || stationToMove == null)
                return Result.Err("Player or station does not exist");
            return player.MoveToStation(stationToMove, modeOfTransport);
        }

        public IReadOnlyList<IStationWithPlayers> GetStationsWithPlayers()
        {
            return stations.Select(s => new StationWithPlayers(s, players.Where(p => p.CurrentStation == s).ToList())).ToList();
        }

        public IReadOnlyList<IPlayer> GetPlayersInGameWith(string gameMaster)
        {
            return players.Where(p => p.GameMaster?.Name == gameMaster).ToList();
        }

        public Result SetGameMaster(string name, string gameMasterName)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
                return Result.Err("Player does not exist");
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Game master does not exist");
            player.gameMaster = gameMaster;
            return Result.Ok();
        }

        public Result StartGame(string gameMasterName)
        {
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Player does not exist");
            if (gameMaster.GameMaster != null)
                return Result.Err("Player is not a game master");
            var playersInGame = players.Where(p => p.GameMaster == gameMaster).ToList();
            if (playersInGame.Count < 2)
                return Result.Err("Not enough players in the game");
            foreach (var p in playersInGame)
                p.ResetTickets();
            return Result.Ok();
        }

        public Result EndGame(string gameMasterName)
        {
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Player does not exist");
            if (gameMaster.GameMaster != null)
                return Result.Err("Player is not a game master");
            var playersInGame = players.Where(p => p.GameMaster == gameMaster).ToList();
            foreach (var p in playersInGame)
                p.ResetGameMaster();
            return Result.Ok();
        }
    }
}
