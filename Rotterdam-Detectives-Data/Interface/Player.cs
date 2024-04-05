using RotterdamDetectives_Data.Data;
using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Interface
{
    internal class Player: IPlayerData
    {
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int GameMode { get; set; }
        public bool IsAdmin { get; set; }
        public IStationData? Station { get; set; }
        public IPlayerData? GameMaster { get; set; }
    }
}
