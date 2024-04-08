using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface ITransportType
    {
        public string Name { get; }
        public int MaxTickets { get; }
    }
}
