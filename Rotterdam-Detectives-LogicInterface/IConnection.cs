using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IConnection
    {
        public string ModeOfTransport { get; }
        public string Destination { get; }
    }
}
