using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class AdminController: IAdminController
    {
        private readonly Logic logic;

        public AdminController(Logic _logic)
        {
            logic = _logic;
        }

        public void AddStation(string station)
        {
            logic.stations.Add(new Station(station));
        }

        public void DeleteStation(string station)
        {
            var s = logic.stations.Find(x => x.Name == station);
            if (s != null)
                logic.stations.Remove(s);
        }

        public List<IStation> GetStations() {
            return [..logic.stations];
        }

        public void ConnectStations(string from, string to, string transportMode)
        {
            if (from == to)
                return;
            var s1 = logic.stations.Find(x => x.Name == from);
            var s2 = logic.stations.Find(x => x.Name == to);
            if (s1 != null && s2 != null)
            {
                var modeOfTransport = (ModeOfTransport)Enum.Parse(typeof(ModeOfTransport), transportMode);
                s1.connections.Add(new Connection(s2, modeOfTransport));
                s2.connections.Add(new Connection(s1, modeOfTransport));
            }
        }

        public void DisconnectStations(string from, string to)
        {
            var s1 = logic.stations.Find(x => x.Name == from);
            var s2 = logic.stations.Find(x => x.Name == to);
            if (s1 != null && s2 != null)
            {
                s1.connections.RemoveAll(x => x.destination == s2);
                s2.connections.RemoveAll(x => x.destination == s1);
            }
        }
    }
}
