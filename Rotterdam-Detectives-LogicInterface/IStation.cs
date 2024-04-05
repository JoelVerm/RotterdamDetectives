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
        public List<string> Players { get; }
    }
}
