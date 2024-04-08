using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private List<string> lastErrors = new();

        public ProcessedDataSource(IDataSource _dataSource, IPasswordHasher _passwordHasher)
        {
            dataSource = _dataSource;
            passwordHasher = _passwordHasher;
        }

        public bool UserExists(string username)
        {
            return dataSource.GetPlayerData(username) != null;
        }

        public void RegisterUser(string username, string password)
        {
            var passwordHash = passwordHasher.HashPassword(username, password);
            dataSource.AddPlayer(username, passwordHash);
        }

        public bool LoginUser(string username, string password)
        {
            var player = dataSource.GetPlayerData(username);
            if (player == null)
                return false;
            return passwordHasher.VerifyHashedPassword(username, password, player.PasswordHash);
        }

        public bool IsAdmin(string username)
        {
            return dataSource.GetPlayerData(username)?.IsAdmin ?? false;
        }

        public string GetStationByPlayer(string username)
        {
            var station = dataSource.GetPlayerData(username)?.Station?.Name;
            if (station == null)
                lastErrors.Add("Could not get your station");
            return station ?? "";
        }

        public bool MovePlayerToStation(string username, string station)
        {
            var currentStation = dataSource.GetPlayerData(username)?.Station;
            if (currentStation != null)
            {
                if (currentStation.Name == station)
                {
                    lastErrors.Add("You are already at this station");
                    return false;
                }
                if (!dataSource.GetConnectedStations(currentStation.Name)?.Any(s => s.Station.Name == station) ?? true)
                {
                    lastErrors.Add("You cannot move to this station");
                    return false;
                }
            }
            return dataSource.MovePlayerToStation(username, station);
        }

        public List<IStation> GetStationsAndPlayers(string username)
        {
            var stations = dataSource.GetStations()?.Select(s =>
                new Interface.Station {
                    Name = s.Name
                }).ToList();
            if (stations == null)
            {
                lastErrors.Add("Could not get stations");
                return new List<IStation>();
            }

            foreach (var station in stations)
            {
                var connections = dataSource.GetConnectedStations(station.Name)?.Select(cs => new Interface.StationConnection { Name = cs.Station.Name, TransportType = cs.TransportType }).ToList<IStationConnection>();
                if (connections == null)
                    lastErrors.Add("Could not get connections for station " + station.Name);
                station.Connections = connections ?? new List<IStationConnection>();
            }

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

            return stations.ToList<IStation>();
        }

        public string? GetGameMasterByPlayer(string username)
        {
            return dataSource.GetPlayerData(username)?.GameMaster?.Name;
        }

        public string? GetLastError()
        {
            if (lastErrors.Count == 0)
                return null;
            var error = string.Join("; ", lastErrors);
            lastErrors.Clear();
            return error;
        }
    }
}
