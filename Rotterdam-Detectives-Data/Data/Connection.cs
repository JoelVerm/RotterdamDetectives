using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Data
{
    internal class Connection
    {
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int TransportType { get; set; }
    }
}
