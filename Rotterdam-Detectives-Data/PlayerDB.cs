using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data
{
    public class PlayerDB(string connectionString) : IPlayerDB
    {
        readonly DB db = new(connectionString);

        public string? GetCurrentStation(string player)
        {
            return db.String("SELECT Stations.Name FROM Players LEFT JOIN Stations ON Players.StationId = Stations.Id WHERE Players.Name = @player", new { player });
        }

        public void SetCurrentStation(string player, string station)
        {
            db.Execute("UPDATE Players SET StationId = (SELECT Id FROM Stations WHERE Name = @station) WHERE Name = @player", new { player, station });
        }

        public string? GetPasswordHash(string player)
        {
            return db.String("SELECT PasswordHash FROM Players WHERE Name = @player", new { player });
        }

        public bool IsMrX(string player)
        {
            return db.Field<bool>("SELECT IsMrX FROM Players WHERE Name = @player", new { player }) ?? false;
        }

        public void Register(string player, string hash)
        {
            db.Execute("INSERT INTO Players (Name, PasswordHash) VALUES (@player, @hash)", new { player, hash });
        }
    }
}
