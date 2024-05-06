using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class AdminController(Logic _logic) : IAdminController
    {
        private readonly Logic logic = _logic;

        public void AddStation(string station)
        {
            if (string.IsNullOrEmpty(station))
                return;
            logic.AdminStations.Add(new Station(station));
        }

        public void DeleteStation(string station)
        {
            var s = logic.AdminStations.Find(x => x.Name == station);
            if (s != null)
                logic.AdminStations.Remove(s);
        }

        public List<IStation> GetStations() {
            return [..logic.AdminStations];
        }

        public void ConnectStations(string from, string to, string transportMode)
        {
            if (from == to)
                return;
            var s1 = logic.AdminStations.Find(x => x.Name == from);
            var s2 = logic.AdminStations.Find(x => x.Name == to);
            if (s1 != null && s2 != null)
            {
                var modeOfTransport = (ModeOfTransport)Enum.Parse(typeof(ModeOfTransport), transportMode);
                s1.AddConnection(s2, modeOfTransport);
                s2.AddConnection(s1, modeOfTransport);
            }
        }

        public void DisconnectStations(string from, string to)
        {
            var s1 = logic.AdminStations.Find(x => x.Name == from);
            var s2 = logic.AdminStations.Find(x => x.Name == to);
            if (s1 != null && s2 != null)
            {
                s1.RemoveConnections(s2);
                s2.RemoveConnections(s1);
            }
        }
    }
}
