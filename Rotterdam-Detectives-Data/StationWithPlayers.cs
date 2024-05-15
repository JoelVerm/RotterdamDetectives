using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    internal class StationWithPlayers(string station, List<string> _players): IStationWithPlayers
    {
        internal List<string> players = _players;
        public string Station { get; set; } = station;
        public IReadOnlyList<string> Players { get; set; } = _players;
    }
}
