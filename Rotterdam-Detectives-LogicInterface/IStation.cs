using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStation
    {
        public List<IConnection> GetConnectionsOf(string station);
        public List<IStationWithPlayers> GetWithPlayers(string username);
    }
}
