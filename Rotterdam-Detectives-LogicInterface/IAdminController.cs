using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IAdminController
    {
        public void AddStation(string station);
        public void DeleteStation(string station);
        public List<IStation> GetStations();
        public void ConnectStations(string from, string to, string transportMode);
        public void DisconnectStations(string from, string to);
    }
}
