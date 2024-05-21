using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal record class StationWithPlayers(string Station, IReadOnlyList<string> Players, IReadOnlyList<IConnection> Connections) : IStationWithPlayers;
}
