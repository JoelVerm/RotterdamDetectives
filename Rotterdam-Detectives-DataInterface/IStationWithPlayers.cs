using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace RotterdamDetectives_DataInterface
{
    public interface IStationWithPlayers
    {
        string Station { get; }
        IReadOnlyList<string> Players { get; }
    }
}
