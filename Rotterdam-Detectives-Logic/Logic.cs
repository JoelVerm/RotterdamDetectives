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

        private readonly List<Player> players = [];
        public IReadOnlyList<IPlayer> Players => players;
        private readonly List<Station> stations = [];
        internal List<Station> AdminStations => stations;
        public IReadOnlyList<IStation> Stations => stations;

        private readonly AdminController adminController;
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

        public IReadOnlyList<IStationWithPlayers> GetStationsWithPlayers(string playerName)
        {
            var player = players.FirstOrDefault(p => p.Name == playerName);
            return stations.Select(s => new StationWithPlayers(s, players.Where(p => p.Game == player?.Game && p.CurrentStation == s).ToList())).ToList();
        }

        public IReadOnlyList<IPlayer> GetPlayersInGameWith(string gameMaster)
        {
            return players.Where(p => (p.Game?.GameMaster.Name) == gameMaster).ToList();
        }

        public Result CreateGame(string gameMasterName)
        {
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Player does not exist");
            if (gameMaster.Game != null)
                return Result.Err("Player is already in a game");
            gameMaster.game = new Game(gameMaster);
            return Result.Ok();
        }

        public Result JoinGame(string name, string gameMasterName)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
                return Result.Err("Player does not exist");
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Game master does not exist");
            var game = gameMaster.game;
            if (game == null)
                return Result.Err("Game does not exist");
            if (player.game != null)
                return Result.Err("Player is already in a game");
            return game.AddPlayer(player);
        }

        public Result LeaveGame(string name)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
                return Result.Err("Player does not exist");
            var game = player.game;
            if (game == null)
                return Result.Err("Player is not in a game");
            player.game = null;
            game.RemovePlayer(player);
            return Result.Ok();
        }

        public Result StartGame(string gameMasterName)
        {
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Game master does not exist");
            if (gameMaster.game?.GameMaster != gameMaster)
                return Result.Err("Player is not a game master");
            return gameMaster.game.Start();
        }

        public Result EndGame(string gameMasterName)
        {
            var gameMaster = players.FirstOrDefault(p => p.Name == gameMasterName);
            if (gameMaster == null)
                return Result.Err("Player does not exist");
            var game = gameMaster.game;
            if (game == null)
                return Result.Err("Player is not in a game");
            if (game.GameMaster != gameMaster)
                return Result.Err("Player is not a game master");
            game.End();
            return Result.Ok();
        }
    }
}
