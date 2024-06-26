﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_DataInterface
{
    public interface IPlayerDB
    {
        public string? GetCurrentStation(string player);
        public void SetCurrentStation(string player, string station);
        public string? GetPasswordHash(string player);
        public void Register(string player, string hash);
    }
}
