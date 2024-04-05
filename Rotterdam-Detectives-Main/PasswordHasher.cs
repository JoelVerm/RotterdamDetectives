using Microsoft.AspNetCore.Identity;

namespace RotterdamDetectives_Main
{
    public class PasswordHasher : RotterdamDetectives_LogicInterface.IPasswordHasher
    {
        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        public string HashPassword(string username, string password)
        {
            return passwordHasher.HashPassword(username, password);
        }

        public bool VerifyHashedPassword(string username, string password, string hash)
        {
            return passwordHasher.VerifyHashedPassword(username, hash, password) == PasswordVerificationResult.Success;
        }
    }
}
