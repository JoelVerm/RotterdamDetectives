using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Test
{
    internal class MockProcessedDataSource : IProcessedDataSource
    {
        public bool ShouldReturnLoggedinUser { get; set; } = true;
        public bool LoginUser(string username, string password) => ShouldReturnLoggedinUser;

        public void RegisterUser(string username, string password) { }

        public bool ShouldReturnUserExists { get; set; } = false;
        public bool UserExists(string username) => ShouldReturnUserExists;
    }
}
