using RotterdamDetectives_DataInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockPlayerDB : IPlayerDB
    {
        public string? MockCurrentStation { get; set; } = null;

        public string? GetCurrentStation(string player) => MockCurrentStation;

        public string? MockPasswordHash { get; set; } = null;

        public string? GetPasswordHash(string player) => MockPasswordHash;

        public void Register(string player, string hash) {}

        public void SetCurrentStation(string player, string station) {}
    }
}
