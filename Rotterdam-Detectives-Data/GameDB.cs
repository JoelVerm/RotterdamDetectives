using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    public class GameDB(string connectionString) : IGameDB
    {
        readonly DB db = new(connectionString);

        public bool Exists(string gameMaster)
        {
            return db.Field<int>("SELECT COUNT(*) FROM Games WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster)", new { gameMaster }) > 0;
        }

        public void AddPlayer(string gameMaster, string player)
        {
            db.Execute("UPDATE Players SET Game = (SELECT Id FROM Games WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster)) WHERE Name = @player", new { gameMaster, player });
        }

        public void CreateGame(string gameMaster)
        {
            db.Execute("INSERT INTO Games (GameMaster) VALUES ((SELECT Id FROM Players WHERE Name = @gameMaster))", new { gameMaster });
        }

        public void DeleteGame(string gameMaster)
        {
            db.Execute("DELETE FROM Games WHERE GameMaster =  (SELECT Id FROM Players WHERE Name = @gameMaster)", new { gameMaster });
        }

        public string? GameMasterOf(string player)
        {
            return db.String("SELECT Name FROM Players WHERE Id = (SELECT GameMaster FROM Games WHERE Id = (SELECT Game FROM Players WHERE Name = @player))", new { player });
        }

        public IEnumerable<string> GetPlayers(string gameMaster)
        {
            return db.Rows("SELECT Name FROM Players WHERE Game = (SELECT Id FROM Games WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster))", new { gameMaster }, row => row["Name"].ToString()!) ?? [];
        }

        public bool IsGameStarted(string gameMaster)
        {
            return db.Field<bool>("SELECT Started FROM Games WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster)", new { gameMaster }) ?? false;
        }

        public bool IsPlayerInGame(string gameMaster, string player)
        {
            return db.Field<int>("SELECT COUNT(*) FROM Players WHERE Game = (SELECT Id FROM Games WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster)) AND Name = @player", new { gameMaster, player }) > 0;
        }

        public void RemovePlayer(string gameMaster, string player)
        {
            db.Execute("UPDATE Players SET Game = NULL WHERE Name = @player", new { player });
        }

        public void SetStarted(string gameMaster, bool started)
        {
            db.Execute("UPDATE Games SET Started = @started WHERE GameMaster = (SELECT Id FROM Players WHERE Name = @gameMaster)", new { gameMaster, started });
        }
    }
}
