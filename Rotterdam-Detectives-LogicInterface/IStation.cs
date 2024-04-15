using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStation
    {
        public string Name { get; }
        public int Latitude { get; }
        public int Longitude { get; }
        public IReadOnlyList<IConnection> Connections { get; }

        public IReadOnlyList<IConnection> GetConnectionsTo(IStation station);

        public void SetCoordinates(int latitude, int longitude);
    }
}
