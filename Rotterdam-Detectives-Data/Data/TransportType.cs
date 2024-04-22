using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Data
{
    internal class TransportType: ITransportType
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int MaxTickets { get; set; }
    }
}
