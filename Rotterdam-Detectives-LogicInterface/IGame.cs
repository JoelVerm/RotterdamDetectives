using RotterdamDetectives_Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IGame
    {
        public IPlayer GameMaster { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public bool IsStarted { get; }
    }
}
