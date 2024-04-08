using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStationConnection
    {
        public string Name { get; }
        public string TransportType { get; }
    }
}
