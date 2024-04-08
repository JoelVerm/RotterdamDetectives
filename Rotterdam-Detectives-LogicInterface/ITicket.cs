using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface ITicket
    {
        string Name { get; }
        int Amount { get; }
    }
}
