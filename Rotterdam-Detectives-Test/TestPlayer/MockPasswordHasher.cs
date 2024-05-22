using Microsoft.AspNetCore.Identity;
using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test.TestPlayer
{
    internal class MockPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string user, string password)
        {
            return $"{user}-{password}";
        }

        bool IPasswordHasher.VerifyHashedPassword(string username, string password, string hash)
        {
            return hash == $"{username}-{password}";
        }
    }
}
