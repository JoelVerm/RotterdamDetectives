using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IGame
    {
        public string? GameMasterOf(string player);
        public Result Join(string gameMaster, string player);
        public Result Leave(string player);
        public IEnumerable<string> GetPlayers(string gameMaster);
        public Dictionary<string, IEnumerable<string>> GetPlayerTickets(string gameMaster);
        public Result Create(string gameMaster);
        public bool IsStarted(string gameMaster);
        public Result Start(string gameMaster);
        public Result End(string gameMaster);
    }
}
