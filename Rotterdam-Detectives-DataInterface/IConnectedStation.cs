using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface IConnectedStation
    {
        public IStationData Station { get; }
        public string TransportType { get; }
    }
}
