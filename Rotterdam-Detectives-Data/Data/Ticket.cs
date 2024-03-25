using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Data
{
    internal class Ticket
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TransportTypeId { get; set; }
    }
}
