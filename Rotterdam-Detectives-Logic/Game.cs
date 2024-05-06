using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    internal class Game(Player gameMaster) : IGame
    {
        private readonly Player gameMaster = gameMaster;
        public IPlayer GameMaster => gameMaster;
        private readonly List<Player> players = [];
        public IReadOnlyList<IPlayer> Players => players;
        public bool IsStarted { get; private set; }

        internal Result AddPlayer(Player player)
        {
            if (IsStarted)
                return Result.Err("Game has already started");
            if (players.Contains(player))
                return Result.Err("Player is already in the game");
            if (player == gameMaster)
                return Result.Err("Player is the game master");
            players.Add(player);
            player.game = this;
            return Result.Ok();
        }

        internal Result RemovePlayer(Player player)
        {
            if (!players.Contains(player))
                return Result.Err("Player is not in the game");
            players.Remove(player);
            return Result.Ok();
        }

        internal Result Start()
        {
            List<Player> playersInGame = [.. players, gameMaster];
            if (playersInGame.Count < 2)
                return Result.Err("Not enough players in the game");
            foreach (var p in playersInGame)
                p.ResetTickets();
            IsStarted = true;
            return Result.Ok();
        }

        internal void End()
        {
            IsStarted = false;
            foreach (var p in players)
            {
                p.game = null;
                p.ResetTickets();
            }
            gameMaster.game = null;
            gameMaster.ResetTickets();
            players.Clear();
        }
    }
}
