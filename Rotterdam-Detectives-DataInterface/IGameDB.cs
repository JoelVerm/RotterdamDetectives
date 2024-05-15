using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface IGameDB
    {
        public bool Exists(string gameMaster);
        public void CreateGame(string gameMaster);
        public void DeleteGame(string gameMaster);
        public string? GameMasterOf(string player);
        public bool IsPlayerInGame(string gameMaster, string player);
        public bool IsGameStarted(string gameMaster);
        public void SetStarted(string gameMaster, bool started);
        public void AddPlayer(string gameMaster, string player);
        public void RemovePlayer(string gameMaster, string player);
        public IEnumerable<string> GetPlayers(string gameMaster);
    }
}
