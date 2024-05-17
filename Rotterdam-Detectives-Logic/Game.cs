using RotterdamDetectives_DataInterface;
using RotterdamDetectives_Globals;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic
{
    public class Game(IGameDB db, Ticket tickets) : IGame
    {
        public string? GameMasterOf(string player)
        {
            return db.GameMasterOf(player);
        }
        
        public Result Join(string gameMaster, string player)
        {
            if (!db.Exists(gameMaster))
                return Result.Err("Game does not exist");
            if (player == gameMaster)
                return Result.Err("Player is the game master");
            bool isStarted = db.IsGameStarted(gameMaster);
            if (isStarted)
                return Result.Err("Game has already started");
            bool isPlayerInGame = db.IsPlayerInGame(gameMaster, player);
            if (isPlayerInGame)
                return Result.Err("Player is already in the game");
            db.AddPlayer(gameMaster, player);
            return Result.Ok();
        }

        public Result Leave(string player)
        {
            string? gameMaster = db.GameMasterOf(player);
            if (gameMaster == null)
                return Result.Err("Player is not in a game");
            db.RemovePlayer(gameMaster, player);
            return Result.Ok();
        }

        public IEnumerable<string> GetPlayers(string gameMaster)
        {
            return db.GetPlayers(gameMaster);;
        }

        public Dictionary<string, IEnumerable<string>> GetPlayerTickets(string gameMaster)
        {
            var players = db.GetPlayers(gameMaster);
            var result = new Dictionary<string, IEnumerable<string>>();
            foreach (var p in players)
                result[p] = tickets.GetHistory(p);
            return result;
        }

        public Result Create(string gameMaster)
        {
            if (db.GameMasterOf(gameMaster) != null)
                return Result.Err("Player is already a game master");
            db.CreateGame(gameMaster);
            db.AddPlayer(gameMaster, gameMaster);
            return Result.Ok();
        }

        public Result Start(string gameMaster)
        {
            var playersInGame = db.GetPlayers(gameMaster).ToList();
            playersInGame.Add(gameMaster);
            if (playersInGame.Count < 2)
                return Result.Err("Not enough players in the game");
            foreach (var p in playersInGame)
                tickets.ResetTickets(p);
            db.SetStarted(gameMaster, true);
            return Result.Ok();
        }

        public bool IsStarted(string gameMaster)
        {
            return db.IsGameStarted(gameMaster);
        }

        public Result End(string gameMaster)
        {
            if (!IsStarted(gameMaster))
                return Result.Err("Game is not started");
            if (gameMaster != db.GameMasterOf(gameMaster))
                return Result.Err("Player is not the game master");
            db.SetStarted(gameMaster, false);
            var playersInGame = db.GetPlayers(gameMaster).ToList();
            playersInGame.Add(gameMaster);
            foreach (var p in playersInGame)
            {
                tickets.ResetTickets(p);
                db.RemovePlayer(gameMaster, p);
            }
            db.DeleteGame(gameMaster);
            return Result.Ok();
        }
    }
}
