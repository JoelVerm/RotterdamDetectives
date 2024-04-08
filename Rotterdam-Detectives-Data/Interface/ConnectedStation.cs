using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Interface
{
    internal class ConnectedStation : IConnectedStation
    {
        public IStationData Station { get; set; }
        public string TransportType { get; set; }
    }
}
