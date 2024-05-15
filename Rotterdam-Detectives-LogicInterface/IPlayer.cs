using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IPlayer
    {
        public Result MoveToStation(string player, string station, string modeOfTransport);
        public bool IsMrX(string player);
        public string? GetCurrentStation(string player);
        public Result Register(string username, string password);
        public bool Login(string username, string password);
        public bool Exists(string username);
    }
}
