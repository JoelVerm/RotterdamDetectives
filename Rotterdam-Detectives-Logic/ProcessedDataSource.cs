using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RotterdamDetectives_DataInterface;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Logic
{
    public class ProcessedDataSource: IProcessedDataSource
    {
        private readonly IDataSource dataSource;
        private readonly IPasswordHasher passwordHasher;

        public ProcessedDataSource(IDataSource _dataSource, IPasswordHasher _passwordHasher)
        {
            dataSource = _dataSource;
            passwordHasher = _passwordHasher;
        }

        public bool UserExists(string username)
        {
            return dataSource.GetPlayerData(username) != null;
        }

        public bool LoginUser(string username, string password)
        {
            var player = dataSource.GetPlayerData(username);
            if (player == null)
                return false;
            return passwordHasher.VerifyHashedPassword(username, password, player.PasswordHash);
        }

        public void RegisterUser(string username, string password)
        {
            var passwordHash = passwordHasher.HashPassword(username, password);
            dataSource.AddPlayer(username, passwordHash);
        }

        public bool IsAdmin(string username)
        {
            return dataSource.GetPlayerData(username)?.IsAdmin ?? false;
        }

        public string GetStationByPlayer(string username)
        {
            return dataSource.GetPlayerData(username)?.Station?.Name ?? "";
        }

        public string GetGameMasterByPlayer(string username)
        {
            return dataSource.GetPlayerData(username)?.GameMaster?.Name ?? "";
        }

        public List<IStation> GetStationsAndPlayers(string username)
        {
            var stations = dataSource.GetStations()?.Select(s =>
                new Interface.Station { Name = s.Name }).ToList<IStation>();
            if (stations == null)
                return new List<IStation>();
            var player = dataSource.GetPlayerData(username)!;
            if (player.GameMaster != null)
            {
                var gamePlayers = dataSource.GetPlayersInGame(player.GameMaster?.Name!);
                if (gamePlayers != null)
                {
                    foreach (var gamePlayer in gamePlayers)
                    {
                        var station = stations.FirstOrDefault(s => s.Name == gamePlayer.Station?.Name);
                        station?.Players.Add(gamePlayer.Name);
                    }
                }
            }
            return stations;
        }
    }
}
