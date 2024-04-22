using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Data.Data
{
    internal class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int GameMode { get; set; }
        public bool IsAdmin { get; set; }
        public int? StationId { get; set; } = null;
        public int? GameMasterId { get; set; } = null;
    }
}
