using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace RotterdamDetectives_LogicInterface
{
    public interface IStationWithPlayers
    {
        IStation Station { get; }
        IReadOnlyList<IPlayer> Players { get; }
    }
}
