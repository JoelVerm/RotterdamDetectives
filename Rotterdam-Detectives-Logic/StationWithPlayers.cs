using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal record class StationWithPlayers(IStation Station, IReadOnlyList<IPlayer> Players) : IStationWithPlayers;
}
