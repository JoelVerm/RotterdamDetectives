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
        public string GetGameMasterByPlayer(string username);
        public List<IStation> GetStationsAndPlayers(string username);
    }
}
