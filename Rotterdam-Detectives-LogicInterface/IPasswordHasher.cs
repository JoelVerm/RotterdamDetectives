using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_LogicInterface
{
    public interface IPasswordHasher
    {
        public string HashPassword(string username, string password);
        public bool VerifyHashedPassword(string username, string password, string hash);
    }
}
