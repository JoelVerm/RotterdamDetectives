using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockGameDB : IGameDB
    {
        public bool MockExists { get; set; } = true;

        public bool Exists(string gameMaster) => MockExists;

        public string? MockGameMasterOf { get; set; } = null;

        public string? GameMasterOf(string player) => MockGameMasterOf;

        public string? MockGetMrX { get; set; } = null;

        public string? GetMrX(string gameMaster) => MockGetMrX;

        public List<string> MockGetPlayers { get; set; } = [];

        public IEnumerable<string> GetPlayers(string gameMaster) => MockGetPlayers;

        public bool MockIsGameStarted { get; set; } = false;

        public bool IsGameStarted(string gameMaster) => MockIsGameStarted;

        public bool MockIsPlayerInGame { get; set; } = false;

        public bool IsPlayerInGame(string gameMaster, string player) => MockIsPlayerInGame;

        // Void methods

        public void AddPlayer(string gameMaster, string player) { }

        public void CreateGame(string gameMaster) { }

        public void DeleteGame(string gameMaster) { }

        public void RemovePlayer(string gameMaster, string player) {}

        public void SetMrX(string gameMaster, string mrX) {}

        public void SetStarted(string gameMaster, bool started) {}
    }
}
