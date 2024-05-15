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
    public class Player(IPlayerDB db, Station stations, Ticket tickets, IPasswordHasher pw) : IPlayer
    {
        public Result MoveToStation(string player, string station, string modeOfTransport)
        {
            string? currentStation = db.GetCurrentStation(player);
            if (currentStation == null)
                return Result.Err("Player does not exist");

            var connections = stations.GetConnectionsOf(currentStation);
            if (connections.Count == 0)
                return Result.Err("Station is not connected");
            connections = connections.Where(c => c.Destination == station).ToList();

            var transportConnection = connections.FirstOrDefault(c => c.ModeOfTransport == modeOfTransport);
            if (transportConnection == null)
                return Result.Err("Wrong mode of transport");

            db.SetCurrentStation(player, station);
            return tickets.Use(player, modeOfTransport);
        }

        public string? GetCurrentStation(string player)
        {
            return db.GetCurrentStation(player);
        }

        public bool IsMrX(string player)
        {
            return db.IsMrX(player);
        }

        public bool Login(string username, string password)
        {
            string? hash = db.GetPasswordHash(username);
            if (hash == null)
                return false;
            return pw.VerifyHashedPassword(username, password, hash);
        }

        public Result Register(string username, string password)
        {
            string? existing = db.GetPasswordHash(username);
            if (existing != null)
                return Result.Err("Username already exists");
            string hash = pw.HashPassword(username, password);
            db.Register(username, hash);
            db.SetCurrentStation(username, stations.RandomStation());
            return Result.Ok();
        }

        public bool Exists(string player)
        {
            return db.GetPasswordHash(player) != null;
        }
    }
}
