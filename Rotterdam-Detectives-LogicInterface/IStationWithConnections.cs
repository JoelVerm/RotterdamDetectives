using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStationWithConnections
    {
        public string Name { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public int MapX { get; }
        public int MapY { get; }
        public List<IConnection> Connections { get; }
    }
}
