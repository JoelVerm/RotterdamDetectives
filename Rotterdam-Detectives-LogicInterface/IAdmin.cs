using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IAdmin
    {
        public List<IStationWithConnections> GetStations();
        public void AddStation(string station);
        public Result ConnectStations(string station1, string station2, string name, string modeOfTransport);
        public Result DisconnectStations(string station1, string station2);
        public Result DeleteStation(string station);
        public void SetCoordinates(string station, double latitude, double longitude);
        public void SetMapPosition(string station, int x, int y);
        public List<string> GetModesOfTransport();
    }
}
