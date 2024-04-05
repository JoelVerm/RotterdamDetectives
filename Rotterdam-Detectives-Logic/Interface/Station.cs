using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic.Interface
{
    internal class Station : IStation
    {
        public string Name { get; set; }

        public List<string> Players { get; set; }
    }
}
